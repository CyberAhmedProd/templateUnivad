using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionCreditsBLL
    {
        private readonly SessionCreditsDAL _dal;
        public SessionCreditsBLL(SessionCreditsDAL dal) => _dal = dal;

        public List<SessionCredits> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionCredits GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionCredits c)
        {
            if (c.IdSessionEpreuve == 0) return (false, "L'épreuve est obligatoire.");
            if (c.IdNiveau == 0) return (false, "Le niveau est obligatoire.");
            if (c.NbCandidat < 0) return (false, "Le nombre de candidats est invalide.");
            if (c.Id == 0) _dal.Insert(c);
            else _dal.Update(c);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
