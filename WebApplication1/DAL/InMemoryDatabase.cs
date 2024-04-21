using WebApplication1.Models;

namespace WebApplication1.DAL
{
    public static class InMemoryDatabase
    {
        public static List<Actor> Actors { get; } = new List<Actor>();
    }
}
