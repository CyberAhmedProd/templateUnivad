using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionFiliereController : Controller
    {
        private readonly SessionFiliereBLL _bll;
        public SessionFiliereController(SessionFiliereBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) =>
            Json(_bll.GetBySession(idSession));

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var f = _bll.GetById(id);
            if (f == null) return NotFound();
            return Json(f);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SessionFiliere f)
        {
            var (success, error) = _bll.Save(f);
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
