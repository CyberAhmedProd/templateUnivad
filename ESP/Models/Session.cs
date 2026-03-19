namespace ESP.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Designation { get; set; }
        public string Annee { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
        public string Etat { get; set; }
        public string Type { get; set; }
        public string Periode { get; set; }
        public int UseGroupe { get; set; }
        public string Observations { get; set; }
        public string Unite { get; set; }
        public int IdOrganisation { get; set; }
        public int IntervalleMinOccupationSalle { get; set; }
        public int IntervalleMinSurveillancesSucc { get; set; }
    }
}
