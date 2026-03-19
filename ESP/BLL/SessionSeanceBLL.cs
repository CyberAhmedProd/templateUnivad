using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionSeanceBLL
    {
        private readonly SessionSeanceDAL _dal;
        public SessionSeanceBLL(SessionSeanceDAL dal) => _dal = dal;

        public List<SessionSeance> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionSeance GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionSeance s)
        {
            if (string.IsNullOrWhiteSpace(s.Designation)) return (false, "La désignation est obligatoire.");
            if (string.IsNullOrWhiteSpace(s.HeureDebut)) return (false, "L'heure de début est obligatoire.");

            if (s.Id == 0) _dal.Insert(s);
            else _dal.Update(s);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
