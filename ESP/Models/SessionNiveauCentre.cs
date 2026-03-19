namespace ESP.Models
{
    public class SessionNiveauCentre
    {
        public int Id { get; set; }
        public int IdSession { get; set; }
        public int IdSessionNiveau { get; set; }
        public int IdSessionCentre { get; set; }
        // Pour l'affichage
        public string NiveauIntitule { get; set; }
        public string CentreIntitule { get; set; }
    }
}
