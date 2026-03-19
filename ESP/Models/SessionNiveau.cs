namespace ESP.Models
{
    public class SessionNiveau
    {
        public int Id { get; set; }
        public int IdSession { get; set; }
        public int IdSessionFiliere { get; set; }
        public string Code { get; set; }
        public string IntituleFr { get; set; }
        public string IntituleAbrg { get; set; }
        public string IntituleAr { get; set; }
    }
}
