-- Conversion de la base MySQL vers SQLite
-- Suppression des contraintes de clés étrangères temporairement pour la création

-- Table Session
CREATE TABLE Session (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Designation TEXT NOT NULL,
    Annee TEXT NOT NULL,
    DateDebut DATETIME NOT NULL,
    DateFin DATETIME NOT NULL,
    Etat TEXT NOT NULL,
    Type TEXT NOT NULL,
    Periode TEXT NOT NULL,
    UseGroupe INTEGER NOT NULL,
    Observations TEXT,
    Unite TEXT,
    IdOrganisation INTEGER NOT NULL,
    IntervalleMinOccupationSalle INTEGER DEFAULT 0,
    IntervalleMinSurveillancesSucc INTEGER DEFAULT 0
);

-- Table SessionCentre
CREATE TABLE SessionCentre (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdCentre INTEGER NOT NULL,
    Intitule TEXT NOT NULL,
    IntituleAbrg TEXT NOT NULL,
    Responsable TEXT NOT NULL,
    Localisation TEXT NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);

-- Table SessionFiliere
CREATE TABLE SessionFiliere (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Code TEXT NOT NULL,
    IntituleFr TEXT NOT NULL,
    IntituleAr TEXT,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);

-- Table SessionNiveau
CREATE TABLE SessionNiveau (
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

-- Table SessionJour
CREATE TABLE SessionJour (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Jour DATETIME NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);

-- Table SessionSeance
CREATE TABLE SessionSeance (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    Designation TEXT NOT NULL,
    DesignationAbregee TEXT,
    HeureDebut TEXT NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);

-- Table SessionEpreuve
CREATE TABLE SessionEpreuve (
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
    TotalNbEnveloppesDeposees INTEGER DEFAULT 0,
    FOREIGN KEY (IdSession) REFERENCES Session(Id),
    FOREIGN KEY (IdNiveau) REFERENCES SessionNiveau(Id),
    FOREIGN KEY (IdSeance) REFERENCES SessionSeance(Id),
    FOREIGN KEY (IdJour) REFERENCES SessionJour(Id)
);

-- Table SessionEpreuveCentre
CREATE TABLE SessionEpreuveCentre (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER,
    IdCentre INTEGER,
    IdNiveau INTEGER,
    IdEpreuve INTEGER NOT NULL,
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
    NbCandidatCredits INTEGER
);

-- Table SessionCredits
CREATE TABLE SessionCredits (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdSessionEpreuve INTEGER NOT NULL,
    IdCentre INTEGER NOT NULL,
    IdNiveau INTEGER NOT NULL,
    NbCandidat INTEGER NOT NULL,
    Observation TEXT,
    FOREIGN KEY (IdSession) REFERENCES Session(Id),
    FOREIGN KEY (IdSessionEpreuve) REFERENCES SessionEpreuve(Id),
    FOREIGN KEY (IdNiveau) REFERENCES SessionNiveau(Id)
);

-- Table SessionNiveau
CREATE TABLE SessionNiveau (
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

-- Table SessionCentre
CREATE TABLE SessionCentre (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdCentre INTEGER NOT NULL,
    Intitule TEXT NOT NULL,
    IntituleAbrg TEXT NOT NULL,
    Responsable TEXT NOT NULL,
    Localisation TEXT NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id)
);

-- Table SessionNiveauCentre
CREATE TABLE SessionNiveauCentre (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    IdSession INTEGER NOT NULL,
    IdSessionNiveau INTEGER NOT NULL,
    IdSessionCentre INTEGER NOT NULL,
    FOREIGN KEY (IdSession) REFERENCES Session(Id),
    FOREIGN KEY (IdSessionNiveau) REFERENCES SessionNiveau(Id),
    FOREIGN KEY (IdSessionCentre) REFERENCES SessionCentre(Id)
);

-- Index pour optimiser les performances
CREATE INDEX idx_session_dates ON Session(DateDebut, DateFin);
CREATE INDEX idx_session_etat ON Session(Etat);
CREATE INDEX idx_sessionepreuve_session ON SessionEpreuve(IdSession);
CREATE INDEX idx_sessionepreuve_niveau ON SessionEpreuve(IdNiveau);