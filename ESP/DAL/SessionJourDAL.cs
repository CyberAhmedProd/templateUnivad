using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionJourDAL
    {
        private readonly AppDbContext _db;
        public SessionJourDAL(AppDbContext db) => _db = db;

        public List<SessionJour> GetBySession(int idSession)
        {
            var list = new List<SessionJour>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionJour WHERE IdSession=$id ORDER BY Jour";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionJour GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionJour WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionJour j)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO SessionJour (IdSession,Jour) VALUES ($is,$j); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$is", j.IdSession);
            cmd.Parameters.AddWithValue("$j", j.Jour.ToString("yyyy-MM-dd"));
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionJour j)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionJour SET Jour=$j WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", j.Id);
            cmd.Parameters.AddWithValue("$j", j.Jour.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionJour WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private SessionJour Map(SqliteDataReader r) => new SessionJour
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            Jour = DateTime.Parse(r["Jour"].ToString())
        };
    }
}
