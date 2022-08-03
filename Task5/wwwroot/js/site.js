let userNotFound = '<div class="alert alert-danger" role="alert">\n' +
    '  User not found\n' +
    '</div>';

let sendSuccess = '<div class="alert alert-success" role="alert">\n' +
    '  Sent\n' +
    '</div>';

$("#recipient").autocomplete({
    source: function (request, response) {
        $.ajax({
            url: searchUrl,
            dataType: "json",
            data: {
                query: request.term
            },
            success: function (data) {
                response(data);
            }
        });
    },
    minLength: 1,
    open: function () {
        $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
    },
    close: function () {
        $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
    }
});

$("#sendForm").submit(function (e) {
    e.preventDefault(); // prevent actual form submit
    let form = $(this);
    $.ajax({
        type: "POST",
        url: sendUrl,
        data: form.serialize(),
        timeout: 5000,
        success: function (data) {
            showAlert(sendSuccess);
            $('#sendForm').each(function () {
                this.reset();
            });
        },
        error: function (data) {
            if(data.responseText === "User not found") {
                showAlert(userNotFound);
            }
        }
        
    });
});

function showAlert(msg){
    $(".recipient").prepend(msg);
    setTimeout(function(){
        $(".recipient").find(".alert").remove();
    },5000);
}

$(document).on('click','.title', async function () {
    let contentId = this.id;
    if ($('.body-'+contentId).length === 0) {
        await CreateMailBody(contentId);
    }
    else {
        $(".body-"+contentId).remove();
    }
});

async function CreateMailBody(contentId){
    let mailBody = await GetMailBody(contentId);
    let mail = '<tr class="body-'+contentId+'">\n' +
        '            <td colspan="3">\n' +
        '                <div>\n' +
        '                    <p>' + mailBody + '</p>\n' +
        '                </div>\n' +
        '            </td>\n' +
        '        </tr>'
    $("#"+contentId).after(mail)
}

async function GetMailBody(mailId) {
    return await $.ajax({
        type: "POST",
        dataType: "text",
        url: mailUrl,
        data: {
            mailId: mailId,
            userId: userId
        }
    });
}
