using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionNiveauBLL
    {
        private readonly SessionNiveauDAL _dal;
        public SessionNiveauBLL(SessionNiveauDAL dal) => _dal = dal;

        public List<SessionNiveau> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionNiveau GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionNiveau n)
        {
            if (string.IsNullOrWhiteSpace(n.Code)) return (false, "Le code est obligatoire.");
            if (string.IsNullOrWhiteSpace(n.IntituleFr)) return (false, "L'intitulé est obligatoire.");

            if (n.Id == 0) _dal.Insert(n);
            else _dal.Update(n);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
