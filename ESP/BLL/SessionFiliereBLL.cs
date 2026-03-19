using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionFiliereBLL
    {
        private readonly SessionFiliereDAL _dal;
        public SessionFiliereBLL(SessionFiliereDAL dal) => _dal = dal;

        public List<SessionFiliere> GetBySession(int idSession) => _dal.GetBySession(idSession);
        public SessionFiliere GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(SessionFiliere f)
        {
            if (string.IsNullOrWhiteSpace(f.Code)) return (false, "Le code est obligatoire.");
            if (string.IsNullOrWhiteSpace(f.IntituleFr)) return (false, "L'intitulé est obligatoire.");

            if (f.Id == 0) _dal.Insert(f);
            else _dal.Update(f);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
