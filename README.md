# ESP — Exam Scheduling Platform

> Template d'architecture MVC .NET 8 avec SQLite, Bootstrap 3, jQuery 3 et DataTables.
> Ce projet est conçu comme un **modèle de codage réutilisable** pour toute application de gestion métier structurée en couches Controller / BLL / DAL / Models.

---

## Architecture

```
ESP/
├── Controllers/          # Couche présentation — reçoit les requêtes HTTP, délègue à la BLL
│   ├── SessionController.cs
│   ├── SessionSeanceController.cs
│   ├── SessionFiliereController.cs
│   ├── SessionNiveauController.cs
│   ├── SessionJourController.cs
│   ├── SessionCentreController.cs
│   ├── SessionNiveauCentreController.cs
│   ├── SessionEpreuveController.cs
│   └── SessionCreditsController.cs
│
├── BLL/                  # Business Logic Layer — validation métier, orchestration
│   ├── SessionBLL.cs
│   ├── SessionSeanceBLL.cs
│   ├── SessionFiliereBLL.cs
│   ├── SessionNiveauBLL.cs
│   ├── SessionJourBLL.cs
│   ├── SessionCentreBLL.cs
│   ├── SessionNiveauCentreBLL.cs
│   ├── SessionEpreuveBLL.cs
│   └── SessionCreditsBLL.cs
│
├── DAL/                  # Data Access Layer — accès SQLite direct via Microsoft.Data.Sqlite
│   ├── DbContext.cs          # Fournit la connexion SQLite
│   ├── DbInitializer.cs      # Crée les tables au démarrage (CREATE TABLE IF NOT EXISTS)
│   ├── SessionDAL.cs
│   ├── SessionSeanceDAL.cs
│   ├── SessionFiliereDAL.cs
│   ├── SessionNiveauDAL.cs
│   ├── SessionJourDAL.cs
│   ├── SessionCentreDAL.cs
│   ├── SessionNiveauCentreDAL.cs
│   ├── SessionEpreuveDAL.cs
│   └── SessionCreditsDAL.cs
│
├── Models/               # POCOs — représentation des entités de la base
│   ├── Session.cs
│   ├── SessionSeance.cs
│   ├── SessionFiliere.cs
│   ├── SessionNiveau.cs
│   ├── SessionJour.cs
│   ├── SessionCentre.cs
│   ├── SessionNiveauCentre.cs
│   ├── SessionEpreuve.cs
│   └── SessionCredits.cs
│
├── Views/
│   ├── Session/
│   │   ├── Index.cshtml      # Liste des sessions avec DataTable + modals CRUD
│   │   └── Details.cshtml    # Fiche session avec onglets (Séances, Filières, Niveaux,
│   │                         #   Journées, Centres, Niveaux-Centres, Épreuves, Crédits)
│   └── Shared/
│       └── _Layout.cshtml    # Layout principal (sidebar, topbar, CDN Bootstrap/jQuery)
│
├── DAL/DbInitializer.cs  # Initialisation automatique de la base SQLite au démarrage
├── Program.cs            # Point d'entrée, injection de dépendances, pipeline HTTP
├── appsettings.json      # Chaîne de connexion SQLite
└── esp.db                # Base SQLite générée automatiquement au premier lancement
```

---

## Flux de données

```
Navigateur (jQuery AJAX)
        ↓ HTTP JSON
    Controller          ← reçoit et retourne JSON / View
        ↓
      BLL               ← valide les règles métier
        ↓
      DAL               ← exécute les requêtes SQL paramétrées
        ↓
    SQLite (esp.db)
```

---

## Stack technique

| Composant | Version | Rôle |
|---|---|---|
| ASP.NET Core MVC | .NET 8 | Framework web |
| Microsoft.Data.Sqlite | 10.x | Accès SQLite sans ORM |
| SQLite | — | Base de données locale (fichier `esp.db`) |
| Bootstrap | 3.4.1 | UI responsive |
| jQuery | 3.7.1 | AJAX, manipulation DOM |
| DataTables | 1.13.6 | Tables dynamiques avec tri, pagination, recherche |

> Pas d'Entity Framework. Toutes les requêtes sont écrites en SQL pur avec paramètres nommés (`$param`) pour éviter les injections SQL.

---

## Pattern de codage — comment ajouter une nouvelle entité

Pour chaque nouvelle table, reproduire ce pattern en 4 étapes :

**1. Model** — `Models/MonEntite.cs`
```csharp
public class MonEntite {
    public int Id { get; set; }
    public int IdSession { get; set; }
    // ... propriétés
}
```

**2. DAL** — `DAL/MonEntiteDAL.cs`
```csharp
public class MonEntiteDAL {
    private readonly AppDbContext _db;
    public MonEntiteDAL(AppDbContext db) => _db = db;
    public List<MonEntite> GetBySession(int idSession) { ... }
    public int Insert(MonEntite e) { ... }
    public void Update(MonEntite e) { ... }
    public void Delete(int id) { ... }
}
```

**3. BLL** — `BLL/MonEntiteBLL.cs`
```csharp
public class MonEntiteBLL {
    private readonly MonEntiteDAL _dal;
    public (bool success, string error) Save(MonEntite e) {
        // validation métier ici
        if (e.Id == 0) _dal.Insert(e); else _dal.Update(e);
        return (true, null);
    }
}
```

**4. Controller** — `Controllers/MonEntiteController.cs`
```csharp
public class MonEntiteController : Controller {
    [HttpGet] public IActionResult GetBySession(int idSession) => Json(_bll.GetBySession(idSession));
    [HttpPost] public IActionResult Save([FromBody] MonEntite e) { ... }
    [HttpPost] public IActionResult Delete(int id) { ... }
}
```

**5. Enregistrer dans `Program.cs`**
```csharp
builder.Services.AddScoped<MonEntiteDAL>();
builder.Services.AddScoped<MonEntiteBLL>();
```

---

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- Aucune installation de base de données requise (SQLite est embarqué)

---

## Lancer l'application

```bash
# Cloner ou copier le projet
cd ESP

# Restaurer les dépendances
dotnet restore

# Lancer en mode développement
dotnet run
```

L'application sera disponible sur `http://localhost:5032`.

La base de données `esp.db` est créée automatiquement au premier démarrage dans le dossier du projet. Aucune migration ni commande SQL manuelle n'est nécessaire.

---

## Build de production

```bash
dotnet publish -c Release -o ./publish
```

Le dossier `publish/` contient l'application autonome. Copier également le fichier `esp.db` si la base contient des données.

---

## Notes

- La chaîne de connexion est dans `appsettings.json` → `ConnectionStrings.DefaultConnection`
- Les clés Data Protection sont stockées dans `DataProtection-Keys/` (dossier local, pas de dépendance système)
- Bootstrap 3, jQuery 3 et DataTables sont chargés via CDN — une connexion internet est requise en développement
- Pour un déploiement hors-ligne, télécharger les librairies et les placer dans `wwwroot/lib/`
