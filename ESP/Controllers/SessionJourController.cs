using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionJourController : Controller
    {
        private readonly SessionJourBLL _bll;
        public SessionJourController(SessionJourBLL bll) => _bll = bll;

        [HttpGet]
        public IActionResult GetBySession(int idSession) => Json(_bll.GetBySession(idSession));

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var j = _bll.GetById(id);
            if (j == null) return NotFound();
            return Json(j);
        }

        [HttpPost]
        public IActionResult Save([FromBody] SessionJour j)
        {
            var (success, error) = _bll.Save(j);
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
