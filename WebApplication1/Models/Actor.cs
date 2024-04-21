namespace WebApplication1.Models
{
    public class Actor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public int Rank { get; set; }

        public Actor()
        {

        }

        public Actor(string id, string name, string details, string type, string source, int rank)
        {
            Id = id;
            Name = name;
            Details = details;
            Type = type;
            Source = source;
            Rank = rank;
        }
    }
}
