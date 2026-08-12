using PeopleApp.MinimalApi.Models;
using static PeopleApp.MinimalApi.Program;

namespace PeopleApp.MinimalApi.Repositories
{
    public class LocatieRepository
    {
        private List<Locatie> _locaties = new List<Locatie>
        {
                new Locatie { Id = 1, Straat = "Main St", Stad = "New York", Land = "USA" },
                new Locatie { Id = 2, Straat = "Second St", Stad = "Los Angeles", Land = "USA" }
        };
        public IEnumerable<Locatie> GetAll()
        {
            return _locaties;
        }
        public Locatie? GetById(int id)
        {
            return _locaties.FirstOrDefault(l => l.Id == id);
        }

        public void Add(Locatie locatie)
        {
            _locaties.Add(locatie);
        }
    }
}
