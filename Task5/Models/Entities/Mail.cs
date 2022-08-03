namespace Task5.Models.Entities
{
    public partial class Mail
    {
        public int Id { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime Date { get; set; }

        public virtual User FromNavigation { get; set; } = null!;
        public virtual User ToNavigation { get; set; } = null!;
    }
}
