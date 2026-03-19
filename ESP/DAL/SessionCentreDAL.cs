using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionCentreDAL
    {
        private readonly AppDbContext _db;
        public SessionCentreDAL(AppDbContext db) => _db = db;

        public List<SessionCentre> GetBySession(int idSession)
        {
            var list = new List<SessionCentre>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionCentre WHERE IdSession=$id ORDER BY Intitule";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionCentre GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionCentre WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionCentre c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionCentre (IdSession,IdCentre,Intitule,IntituleAbrg,Responsable,Localisation)
                VALUES ($is,$ic,$i,$ia,$r,$l); SELECT last_insert_rowid();";
            BindParams(cmd, c);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionCentre c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionCentre SET Intitule=$i,IntituleAbrg=$ia,Responsable=$r,Localisation=$l WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", c.Id);
            BindParams(cmd, c);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionCentre WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionCentre c)
        {
            cmd.Parameters.AddWithValue("$is", c.IdSession);
            cmd.Parameters.AddWithValue("$ic", c.IdCentre);
            cmd.Parameters.AddWithValue("$i", c.Intitule);
            cmd.Parameters.AddWithValue("$ia", c.IntituleAbrg);
            cmd.Parameters.AddWithValue("$r", c.Responsable);
            cmd.Parameters.AddWithValue("$l", c.Localisation);
        }

        private SessionCentre Map(SqliteDataReader r) => new SessionCentre
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            IdCentre = Convert.ToInt32(r["IdCentre"]),
            Intitule = r["Intitule"].ToString(),
            IntituleAbrg = r["IntituleAbrg"].ToString(),
            Responsable = r["Responsable"].ToString(),
            Localisation = r["Localisation"].ToString()
        };
    }
}
