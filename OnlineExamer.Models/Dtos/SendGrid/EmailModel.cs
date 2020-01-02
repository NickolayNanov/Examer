namespace OnlineExamer.Models.Dtos.SendGrid
{
    public class EmailModel
    {
        public EmailModel(string from, string name, string subject, string body, string to = "nickolaynanov17@gmail.com")
        {
            this.From = from;
            this.To = to;
            this.Name = name;
            this.Subject = subject;
            this.Body = body;
        }

        public string From { get; set; }

        public string To { get; set; }

        public string Name { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
