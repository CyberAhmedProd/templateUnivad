using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionCentreBLL
    {
        private readonly SessionCentreDAL _dal;
        public SessionCentreBLL(SessionCentreDAL dal) => _dal = dal;

        public List<SessionCentre> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionCentre GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionCentre c)
        {
            if (string.IsNullOrWhiteSpace(c.Intitule)) return (false, "L'intitulé est obligatoire.");
            if (c.Id == 0) _dal.Insert(c);
            else _dal.Update(c);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
