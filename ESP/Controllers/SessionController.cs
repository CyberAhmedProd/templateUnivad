using ESP.BLL;
using ESP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ESP.Controllers
{
    public class SessionController : Controller
    {
        private readonly SessionBLL _bll;
        public SessionController(SessionBLL bll) => _bll = bll;

        public IActionResult Index() => View(_bll.GetAll());

        public IActionResult Details(int id)
        {
            var session = _bll.GetById(id);
            if (session == null) return NotFound();
            return View(session);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var s = _bll.GetById(id);
            if (s == null) return NotFound();
            return Json(s);
        }

        [HttpPost]
        public IActionResult Save([FromBody] Session s)
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
