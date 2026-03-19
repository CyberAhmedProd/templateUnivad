using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionNiveauCentreDAL
    {
        private readonly AppDbContext _db;
        public SessionNiveauCentreDAL(AppDbContext db) => _db = db;

        public List<SessionNiveauCentre> GetBySession(int idSession)
        {
            var list = new List<SessionNiveauCentre>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT nc.*, n.IntituleFr AS NiveauIntitule, c.Intitule AS CentreIntitule
                FROM SessionNiveauCentre nc
                LEFT JOIN SessionNiveau n ON n.Id = nc.IdSessionNiveau
                LEFT JOIN SessionCentre c ON c.Id = nc.IdSessionCentre
                WHERE nc.IdSession=$id";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public int Insert(SessionNiveauCentre nc)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionNiveauCentre (IdSession,IdSessionNiveau,IdSessionCentre)
                VALUES ($is,$in,$ic); SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("$is", nc.IdSession);
            cmd.Parameters.AddWithValue("$in", nc.IdSessionNiveau);
            cmd.Parameters.AddWithValue("$ic", nc.IdSessionCentre);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionNiveauCentre WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private SessionNiveauCentre Map(SqliteDataReader r) => new SessionNiveauCentre
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            IdSessionNiveau = Convert.ToInt32(r["IdSessionNiveau"]),
            IdSessionCentre = Convert.ToInt32(r["IdSessionCentre"]),
            NiveauIntitule = r["NiveauIntitule"] == DBNull.Value ? "" : r["NiveauIntitule"].ToString(),
            CentreIntitule = r["CentreIntitule"] == DBNull.Value ? "" : r["CentreIntitule"].ToString()
        };
    }
}
