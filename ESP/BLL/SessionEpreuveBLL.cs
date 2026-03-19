using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionEpreuveBLL
    {
        private readonly SessionEpreuveDAL _dal;
        public SessionEpreuveBLL(SessionEpreuveDAL dal) => _dal = dal;

        public List<SessionEpreuve> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionEpreuve GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionEpreuve e)
        {
            if (e.IdNiveau == 0) return (false, "Le niveau est obligatoire.");
            if (string.IsNullOrWhiteSpace(e.Intitule)) return (false, "L'intitulé est obligatoire.");
            if (e.Id == 0) _dal.Insert(e);
            else _dal.Update(e);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
