using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionDAL
    {
        private readonly AppDbContext _db;
        public SessionDAL(AppDbContext db) => _db = db;

        public List<Session> GetAll()
        {
            var list = new List<Session>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Session ORDER BY Id DESC";
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public Session GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Session WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(Session s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Session (Designation,Annee,DateDebut,DateFin,Etat,Type,Periode,UseGroupe,Observations,Unite,IdOrganisation,IntervalleMinOccupationSalle,IntervalleMinSurveillancesSucc)
                VALUES ($d,$a,$dd,$df,$e,$t,$p,$ug,$obs,$u,$io,$im1,$im2);
                SELECT last_insert_rowid();";
            BindParams(cmd, s);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(Session s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Session SET Designation=$d,Annee=$a,DateDebut=$dd,DateFin=$df,Etat=$e,Type=$t,Periode=$p,UseGroupe=$ug,Observations=$obs,Unite=$u,IdOrganisation=$io,IntervalleMinOccupationSalle=$im1,IntervalleMinSurveillancesSucc=$im2
                WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", s.Id);
            BindParams(cmd, s);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Session WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, Session s)
        {
            cmd.Parameters.AddWithValue("$d", s.Designation);
            cmd.Parameters.AddWithValue("$a", s.Annee);
            cmd.Parameters.AddWithValue("$dd", s.DateDebut.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$df", s.DateFin.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$e", s.Etat);
            cmd.Parameters.AddWithValue("$t", s.Type);
            cmd.Parameters.AddWithValue("$p", s.Periode);
            cmd.Parameters.AddWithValue("$ug", s.UseGroupe);
            cmd.Parameters.AddWithValue("$obs", (object)s.Observations ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$u", (object)s.Unite ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$io", s.IdOrganisation);
            cmd.Parameters.AddWithValue("$im1", s.IntervalleMinOccupationSalle);
            cmd.Parameters.AddWithValue("$im2", s.IntervalleMinSurveillancesSucc);
        }

        private Session Map(SqliteDataReader r) => new Session
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Designation = r["Designation"].ToString(),
            Annee = r["Annee"].ToString(),
            DateDebut = DateTime.Parse(r["DateDebut"].ToString()),
            DateFin = DateTime.Parse(r["DateFin"].ToString()),
            Etat = r["Etat"].ToString(),
            Type = r["Type"].ToString(),
            Periode = r["Periode"].ToString(),
            UseGroupe = Convert.ToInt32(r["UseGroupe"]),
            Observations = r["Observations"] == DBNull.Value ? null : r["Observations"].ToString(),
            Unite = r["Unite"] == DBNull.Value ? null : r["Unite"].ToString(),
            IdOrganisation = Convert.ToInt32(r["IdOrganisation"]),
            IntervalleMinOccupationSalle = Convert.ToInt32(r["IntervalleMinOccupationSalle"]),
            IntervalleMinSurveillancesSucc = Convert.ToInt32(r["IntervalleMinSurveillancesSucc"])
        };
    }
}
