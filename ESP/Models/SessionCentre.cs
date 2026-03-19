namespace ESP.Models
{
    public class SessionCentre
    {
        public int Id { get; set; }
        public int IdSession { get; set; }
        public int IdCentre { get; set; }
        public string Intitule { get; set; }
        public string IntituleAbrg { get; set; }
        public string Responsable { get; set; }
        public string Localisation { get; set; }
    }
}
