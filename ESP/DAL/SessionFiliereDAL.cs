using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionFiliereDAL
    {
        private readonly AppDbContext _db;
        public SessionFiliereDAL(AppDbContext db) => _db = db;

        public List<SessionFiliere> GetBySession(int idSession)
        {
            var list = new List<SessionFiliere>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionFiliere WHERE IdSession=$id";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionFiliere GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionFiliere WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionFiliere f)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionFiliere (IdSession,Code,IntituleFr,IntituleAr)
                VALUES ($is,$c,$ifr,$iar); SELECT last_insert_rowid();";
            BindParams(cmd, f);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionFiliere f)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "UPDATE SessionFiliere SET Code=$c,IntituleFr=$ifr,IntituleAr=$iar WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", f.Id);
            BindParams(cmd, f);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionFiliere WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionFiliere f)
        {
            cmd.Parameters.AddWithValue("$is", f.IdSession);
            cmd.Parameters.AddWithValue("$c", f.Code);
            cmd.Parameters.AddWithValue("$ifr", f.IntituleFr);
            cmd.Parameters.AddWithValue("$iar", (object)f.IntituleAr ?? DBNull.Value);
        }

        private SessionFiliere Map(SqliteDataReader r) => new SessionFiliere
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            Code = r["Code"].ToString(),
            IntituleFr = r["IntituleFr"].ToString(),
            IntituleAr = r["IntituleAr"] == DBNull.Value ? null : r["IntituleAr"].ToString()
        };
    }
}
