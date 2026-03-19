using Microsoft.Data.Sqlite;
using ESP.Models;

namespace ESP.DAL
{
    public class SessionEpreuveDAL
    {
        private readonly AppDbContext _db;
        public SessionEpreuveDAL(AppDbContext db) => _db = db;

        public List<SessionEpreuve> GetBySession(int idSession)
        {
            var list = new List<SessionEpreuve>();
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionEpreuve WHERE IdSession=$id ORDER BY Id";
            cmd.Parameters.AddWithValue("$id", idSession);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public SessionEpreuve GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM SessionEpreuve WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Insert(SessionEpreuve e)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO SessionEpreuve
                (IdSession,IdNiveau,Code,Nature,Intitule,IntituleAbrg,Duree,Type,IdSeance,IdJour,HeureDebut,HeureFin,NbCandidat,NbCandidatCredits,NbEnveloppes,EtatDepotSujet,NbEnveloppesADeposer,TotalNbEnveloppesDeposees)
                VALUES ($is,$in,$c,$na,$i,$ia,$du,$t,$ise,$ij,$hd,$hf,$nb,$nbc,$nbe,$eds,$nbad,$tnbd);
                SELECT last_insert_rowid();";
            BindParams(cmd, e);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Update(SessionEpreuve e)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE SessionEpreuve SET
                IdNiveau=$in,Code=$c,Nature=$na,Intitule=$i,IntituleAbrg=$ia,Duree=$du,Type=$t,
                IdSeance=$ise,IdJour=$ij,HeureDebut=$hd,HeureFin=$hf,NbCandidat=$nb,
                NbCandidatCredits=$nbc,NbEnveloppes=$nbe,EtatDepotSujet=$eds,
                NbEnveloppesADeposer=$nbad,TotalNbEnveloppesDeposees=$tnbd
                WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", e.Id);
            BindParams(cmd, e);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM SessionEpreuve WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }

        private void BindParams(SqliteCommand cmd, SessionEpreuve e)
        {
            cmd.Parameters.AddWithValue("$is", e.IdSession);
            cmd.Parameters.AddWithValue("$in", e.IdNiveau);
            cmd.Parameters.AddWithValue("$c",   (object)e.Code ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$na",  (object)e.Nature ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$i",   (object)e.Intitule ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$ia",  (object)e.IntituleAbrg ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$du",  (object)e.Duree ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$t",   (object)e.Type ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$ise", (object)e.IdSeance ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$ij",  (object)e.IdJour ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$hd",  (object)e.HeureDebut ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$hf",  (object)e.HeureFin ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$nb",  (object)e.NbCandidat ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$nbc", (object)e.NbCandidatCredits ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$nbe", (object)e.NbEnveloppes ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$eds", (object)e.EtatDepotSujet ?? DBNull.Value);
            cmd.Parameters.AddWithValue("$nbad", e.NbEnveloppesADeposer);
            cmd.Parameters.AddWithValue("$tnbd", e.TotalNbEnveloppesDeposees);
        }

        private SessionEpreuve Map(SqliteDataReader r) => new SessionEpreuve
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            IdSession = Convert.ToInt32(r["IdSession"]),
            IdNiveau = Convert.ToInt32(r["IdNiveau"]),
            Code = r["Code"] == DBNull.Value ? null : r["Code"].ToString(),
            Nature = r["Nature"] == DBNull.Value ? null : r["Nature"].ToString(),
            Intitule = r["Intitule"] == DBNull.Value ? null : r["Intitule"].ToString(),
            IntituleAbrg = r["IntituleAbrg"] == DBNull.Value ? null : r["IntituleAbrg"].ToString(),
            Duree = r["Duree"] == DBNull.Value ? null : r["Duree"].ToString(),
            Type = r["Type"] == DBNull.Value ? null : r["Type"].ToString(),
            IdSeance = r["IdSeance"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["IdSeance"]),
            IdJour = r["IdJour"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["IdJour"]),
            HeureDebut = r["HeureDebut"] == DBNull.Value ? null : r["HeureDebut"].ToString(),
            HeureFin = r["HeureFin"] == DBNull.Value ? null : r["HeureFin"].ToString(),
            NbCandidat = r["NbCandidat"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["NbCandidat"]),
            NbCandidatCredits = r["NbCandidatCredits"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["NbCandidatCredits"]),
            NbEnveloppes = r["NbEnveloppes"] == DBNull.Value ? null : (int?)Convert.ToInt32(r["NbEnveloppes"]),
            EtatDepotSujet = r["EtatDepotSujet"] == DBNull.Value ? null : r["EtatDepotSujet"].ToString(),
            NbEnveloppesADeposer = Convert.ToInt32(r["NbEnveloppesADeposer"]),
            TotalNbEnveloppesDeposees = Convert.ToInt32(r["TotalNbEnveloppesDeposees"])
        };
    }
}
