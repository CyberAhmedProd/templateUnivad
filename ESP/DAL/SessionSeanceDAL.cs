using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionSeanceDAL
    {
        private readonly AppDbContext _db;
        public SessionSeanceDAL(AppDbContext db) => _db = db;

        public List<SessionSeance> GetBySession(int idSession)
        {
            var list = new List<SessionSeance>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionSeance WHERE IdSession=$id ORDER BY HeureDebut";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionSeance GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionSeance WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionSeance s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionSeance (IdSession,Designation,DesignationAbregee,HeureDebut)
                VALUES ($is,$d,$da,$h); SELECT last_insert_rowid();";
            BindParams(cmd, s);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionSeance s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionSeance SET Designation=$d,DesignationAbregee=$da,HeureDebut=$h WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", s.Id);
            BindParams(cmd, s);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionSeance WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionSeance s)
        {
            cmd.Parameters.AddWithValue("$is", s.IdSession);
            cmd.Parameters.AddWithValue("$d", s.Designation);
            cmd.Parameters.AddWithValue("$da", (object)s.DesignationAbregee ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$h", s.HeureDebut);
        }

        private SessionSeance Map(SqliteDataReader r) => new SessionSeance
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            Designation = r["Designation"].ToString(),
            DesignationAbregee = r["DesignationAbregee"] == DBNull.Value ? null : r["DesignationAbregee"].ToString(),
            HeureDebut = r["HeureDebut"].ToString()
        };
    }
}
