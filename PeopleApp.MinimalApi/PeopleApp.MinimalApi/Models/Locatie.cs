namespace PeopleApp.MinimalApi.Models
{
    public class Locatie
    {
        public int Id { get; set; }
       public string Straat { get; set; }= string.Empty;
        public string Stad { get; set; }= string.Empty;
        public string Land { get; set; }= string.Empty;
    }
}
