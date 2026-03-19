namespace ESP.Models
{
    public class SessionCredits
    {
        public int Id { get; set; }
        public int IdSession { get; set; }
        public int IdSessionEpreuve { get; set; }
        public int IdCentre { get; set; }
        public int IdNiveau { get; set; }
        public int NbCandidat { get; set; }
        public string Observation { get; set; }
        // Pour l'affichage
        public string EpreuveIntitule { get; set; }
        public string NiveauIntitule { get; set; }
    }
}
