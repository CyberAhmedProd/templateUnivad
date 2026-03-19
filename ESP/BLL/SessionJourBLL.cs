using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionJourBLL
    {
        private readonly SessionJourDAL _dal;
        public SessionJourBLL(SessionJourDAL dal) => _dal = dal;

        public List<SessionJour> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionJour GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionJour j)
        {
            if (j.Jour == default) return (false, "La date est obligatoire.");
            if (j.Id == 0) _dal.Insert(j);
            else _dal.Update(j);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
