using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionNiveauDAL
    {
        private readonly AppDbContext _db;
        public SessionNiveauDAL(AppDbContext db) => _db = db;

        public List<SessionNiveau> GetBySession(int idSession)
        {
            var list = new List<SessionNiveau>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionNiveau WHERE IdSession=$id";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionNiveau GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionNiveau WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionNiveau n)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionNiveau (IdSession,IdSessionFiliere,Code,IntituleFr,IntituleAbrg,IntituleAr)
                VALUES ($is,$isf,$c,$ifr,$ia,$iar); SELECT last_insert_rowid();";
            BindParams(cmd, n);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionNiveau n)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionNiveau SET IdSessionFiliere=$isf,Code=$c,IntituleFr=$ifr,IntituleAbrg=$ia,IntituleAr=$iar WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", n.Id);
            BindParams(cmd, n);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionNiveau WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionNiveau n)
        {
            cmd.Parameters.AddWithValue("$is", n.IdSession);
            cmd.Parameters.AddWithValue("$isf", n.IdSessionFiliere);
            cmd.Parameters.AddWithValue("$c", n.Code);
            cmd.Parameters.AddWithValue("$ifr", n.IntituleFr);
            cmd.Parameters.AddWithValue("$ia", n.IntituleAbrg);
            cmd.Parameters.AddWithValue("$iar", (object)n.IntituleAr ?? DBNull.Value);
        }

        private SessionNiveau Map(SqliteDataReader r) => new SessionNiveau
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            IdSessionFiliere = Convert.ToInt32(r["IdSessionFiliere"]),
            Code = r["Code"].ToString(),
            IntituleFr = r["IntituleFr"].ToString(),
            IntituleAbrg = r["IntituleAbrg"].ToString(),
            IntituleAr = r["IntituleAr"] == DBNull.Value ? null : r["IntituleAr"].ToString()
        };
    }
}
