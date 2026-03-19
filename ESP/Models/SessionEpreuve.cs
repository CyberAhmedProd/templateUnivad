namespace ESP.Models
{
    public class SessionEpreuve
    {
        public int Id { get; set; }
        public int IdSession { get; set; }
        public int IdNiveau { get; set; }
        public string Code { get; set; }
        public string Nature { get; set; }
        public string Intitule { get; set; }
        public string IntituleAbrg { get; set; }
        public string Duree { get; set; }
        public string Type { get; set; }
        public int? IdSeance { get; set; }
        public int? IdJour { get; set; }
        public string HeureDebut { get; set; }
        public string HeureFin { get; set; }
        public int? IdEpreuveCommune { get; set; }
        public int? NbCandidat { get; set; }
        public int? NbCandidatCredits { get; set; }
        public int? NbEnveloppes { get; set; }
        public string EtatDepotSujet { get; set; }
        public int NbEnveloppesADeposer { get; set; }
        public int TotalNbEnveloppesDeposees { get; set; }
    }
}
