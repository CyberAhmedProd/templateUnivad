using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionNiveauController : Controller
    {
        private readonly SessionNiveauBLL _bll;
        public SessionNiveauController(SessionNiveauBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) =>
            Json(_bll.GetBySession(idSession));

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var n = _bll.GetById(id);
            if (n == null) return NotFound();
            return Json(n);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SessionNiveau n)
        {
            var (success, error) = _bll.Save(n);
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
