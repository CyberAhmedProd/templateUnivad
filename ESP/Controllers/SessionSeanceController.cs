using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionSeanceController : Controller
    {
        private readonly SessionSeanceBLL _bll;
        public SessionSeanceController(SessionSeanceBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) =>
            Json(_bll.GetBySession(idSession));

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var s = _bll.GetById(id);
            if (s == null) return NotFound();
            return Json(s);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SessionSeance s)
        {
            var (success, error) = _bll.Save(s);
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
