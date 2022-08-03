namespace Task5.Models.Entities
{
    public partial class User
    {
        public User()
        {
            MailFromNavigations = new HashSet<Mail>();
            MailToNavigations = new HashSet<Mail>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;

        public virtual ICollection<Mail> MailFromNavigations { get; set; }
        public virtual ICollection<Mail> MailToNavigations { get; set; }
    }
}
