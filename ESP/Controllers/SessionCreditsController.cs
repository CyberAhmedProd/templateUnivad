using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionCreditsController : Controller
    {
        private readonly SessionCreditsBLL _bll;
        public SessionCreditsController(SessionCreditsBLL bll) => _bll = bll;

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
        public IActionResult Save([FromBody] SessionCredits c)
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
