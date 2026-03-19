using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionNiveauCentreController : Controller
    {
        private readonly SessionNiveauCentreBLL _bll;
        public SessionNiveauCentreController(SessionNiveauCentreBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) => Json(_bll.GetBySession(idSession));

        [HttpPost]
        public IActionResult Save([FromBody] SessionNiveauCentre nc)
        {
            var (success, error) = _bll.Save(nc);
            if (!success) return BadRequest(new { error });
            return Ok(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _bll.Delete(id);
            return Ok(new { success = true });
        }
    }
}
