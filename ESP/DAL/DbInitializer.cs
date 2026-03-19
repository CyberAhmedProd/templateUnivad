using Microsoft.Data.Sqlite;

namespace ESP.DAL
{
    public static class DbInitializer
    {
        public static void Initialize(IConfiguration configuration)
        {
            var connStr = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=esp.db";
            using var conn = new SqliteConnection(connStr);
            conn.Open();

            var sql = @"
CREATE TABLE IF NOT EXISTS Session (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Designation TEXT NOT NULL,
    Annee TEXT NOT NULL,
    DateDebut DATETIME NOT NULL,
    DateFin DATETIME NOT NULL,
    Etat TEXT NOT NULL,
    Type TEXT NOT NULL,
    Periode TEXT NOT NULL,
    UseGroupe INTEGER NOT NULL DEFAULT 0,
    Observations TEXT,
    Unite TEXT,
    IdOrganisation INTEGER NOT NULL DEFAULT 1,
    IntervalleMinOccupationSalle INTEGER DEFAULT 0,
    IntervalleMinSurveillancesSucc INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS SessionSeance (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Designation TEXT NOT NULL,
    DesignationAbregee TEXT,
    HeureDebut TEXT NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);
CREATE TABLE IF NOT EXISTS SessionFiliere (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Code TEXT NOT NULL,
    IntituleFr TEXT NOT NULL,
    IntituleAr TEXT,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);
CREATE TABLE IF NOT EXISTS SessionNiveau (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdSessionFiliere INTEGER NOT NULL,
    Code TEXT NOT NULL,
    IntituleFr TEXT NOT NULL,
    IntituleAbrg TEXT NOT NULL,
    IntituleAr TEXT,
    FOREIGN KEY (IdSession) REFERENCES Session(Id),
    FOREIGN KEY (IdSessionFiliere) REFERENCES SessionFiliere(Id)
);
CREATE TABLE IF NOT EXISTS SessionJour (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Jour DATETIME NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);
CREATE TABLE IF NOT EXISTS SessionEpreuve (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdNiveau INTEGER NOT NULL,
    Code TEXT,
    Nature TEXT,
    Intitule TEXT,
    IntituleAbrg TEXT,
    Duree TEXT,
    Type TEXT,
    IdSeance INTEGER,
    IdJour INTEGER,
    HeureDebut TEXT,
    HeureFin TEXT,
    IdEpreuveCommune INTEGER,
    NbCandidat INTEGER,
    NbCandidatCredits INTEGER,
    NbEnveloppes INTEGER,
    EtatDepotSujet TEXT,
    NbEnveloppesADeposer INTEGER DEFAULT 0,
    TotalNbEnveloppesDeposees INTEGER DEFAULT 0
);
CREATE TABLE IF NOT EXISTS SessionCentre (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdCentre INTEGER NOT NULL,
    Intitule TEXT NOT NULL,
    IntituleAbrg TEXT NOT NULL,
    Responsable TEXT NOT NULL,
    Localisation TEXT NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);";

            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
    }
}
