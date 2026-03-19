using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionNiveauCentreBLL
    {
        private readonly SessionNiveauCentreDAL _dal;
        public SessionNiveauCentreBLL(SessionNiveauCentreDAL dal) => _dal = dal;

        public List<SessionNiveauCentre> GetBySession(int idSession) => _dal.GetBySession(idSession);

        public (bool success, string error) Save(SessionNiveauCentre nc)
        {
            if (nc.IdSessionNiveau == 0) return (false, "Le niveau est obligatoire.");
            if (nc.IdSessionCentre == 0) return (false, "Le centre est obligatoire.");
            _dal.Insert(nc);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
