let connection = new signalR.HubConnectionBuilder().withUrl("/mail").build();

connection.start().then(function () {
    connection.invoke("Login", userId);
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("NewMessage", function (mail) {
    let tr = '<tr id="'+mail.id+'">\n' +
        '                        <td>'+mail.from+'</td>\n' +
        '                        <td class="title" id="'+mail.id+'">'+mail.title+'</td>\n' +
        '                        <td>'+mail.date+'</td>\n' +
        '                    </tr>'
    $('tbody').prepend(tr);
});

