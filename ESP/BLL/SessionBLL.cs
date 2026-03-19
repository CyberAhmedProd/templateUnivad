using ESP.DAL;
using ESP.Models;

namespace ESP.BLL
{
    public class SessionBLL
    {
        private readonly SessionDAL _dal;
        public SessionBLL(SessionDAL dal) => _dal = dal;

        public List<Session> GetAll() => _dal.GetAll();
        public Session GetById(int id) => _dal.GetById(id);

        public (bool success, string error) Save(Session s)
        {
            if (string.IsNullOrWhiteSpace(s.Designation)) return (false, "La désignation est obligatoire.");
            if (string.IsNullOrWhiteSpace(s.Annee)) return (false, "L'année est obligatoire.");
            if (s.DateFin < s.DateDebut) return (false, "La date de fin doit être après la date de début.");

            if (s.Id == 0) _dal.Insert(s);
            else _dal.Update(s);
            return (true, null);
        }

        public void Delete(int id) => _dal.Delete(id);
    }
}
