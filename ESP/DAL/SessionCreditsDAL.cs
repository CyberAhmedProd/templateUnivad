using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionCreditsDAL
    {
        private readonly AppDbContext _db;
        public SessionCreditsDAL(AppDbContext db) => _db = db;

        public List<SessionCredits> GetBySession(int idSession)
        {
            var list = new List<SessionCredits>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT sc.*, e.Intitule AS EpreuveIntitule, n.IntituleFr AS NiveauIntitule
                FROM SessionCredits sc
                LEFT JOIN SessionEpreuve e ON e.Id = sc.IdSessionEpreuve
                LEFT JOIN SessionNiveau n ON n.Id = sc.IdNiveau
                WHERE sc.IdSession=$id ORDER BY sc.Id";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionCredits GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT sc.*, e.Intitule AS EpreuveIntitule, n.IntituleFr AS NiveauIntitule FROM SessionCredits sc LEFT JOIN SessionEpreuve e ON e.Id=sc.IdSessionEpreuve LEFT JOIN SessionNiveau n ON n.Id=sc.IdNiveau WHERE sc.Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionCredits c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionCredits (IdSession,IdSessionEpreuve,IdCentre,IdNiveau,NbCandidat,Observation)
                VALUES ($is,$ie,$ic,$in,$nb,$obs); SELECT last_insert_rowid();";
            BindParams(cmd, c);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionCredits c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionCredits SET IdSessionEpreuve=$ie,IdCentre=$ic,IdNiveau=$in,NbCandidat=$nb,Observation=$obs WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", c.Id);
            BindParams(cmd, c);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionCredits WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionCredits c)
        {
            cmd.Parameters.AddWithValue("$is", c.IdSession);
            cmd.Parameters.AddWithValue("$ie", c.IdSessionEpreuve);
            cmd.Parameters.AddWithValue("$ic", c.IdCentre);
            cmd.Parameters.AddWithValue("$in", c.IdNiveau);
            cmd.Parameters.AddWithValue("$nb", c.NbCandidat);
            cmd.Parameters.AddWithValue("$obs", (object)c.Observation ?? DBNull.Value);
        }

        private SessionCredits Map(SqliteDataReader r) => new SessionCredits
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            IdSessionEpreuve = Convert.ToInt32(r["IdSessionEpreuve"]),
            IdCentre = Convert.ToInt32(r["IdCentre"]),
            IdNiveau = Convert.ToInt32(r["IdNiveau"]),
            NbCandidat = Convert.ToInt32(r["NbCandidat"]),
            Observation = r["Observation"] == DBNull.Value ? null : r["Observation"].ToString(),
            EpreuveIntitule = r["EpreuveIntitule"] == DBNull.Value ? "" : r["EpreuveIntitule"].ToString(),
            NiveauIntitule = r["NiveauIntitule"] == DBNull.Value ? "" : r["NiveauIntitule"].ToString()
        };
    }
}
