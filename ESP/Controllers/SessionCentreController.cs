using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionCentreController : Controller
    {
        private readonly SessionCentreBLL _bll;
        public SessionCentreController(SessionCentreBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) => Json(_bll.GetBySession(idSession));

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var c = _bll.GetById(id);
            if (c == null) return NotFound();
            return Json(c);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SessionCentre c)
        {
            var (success, error) = _bll.Save(c);
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
