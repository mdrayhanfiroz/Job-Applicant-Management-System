using masterdetailsCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace masterdetailsCore.Controllers
{
    public class SkillsController : Controller
    {
        private readonly JobDbContext _context;
        public SkillsController(JobDbContext _context)
        {
            this._context = _context;
        }
        public IActionResult Index()
        {
            var skils = _context.Skills.ToList();

            return View(skils);
        }

        public IActionResult Create()
        {



            return View();
        }

        [HttpPost]
        public IActionResult Create(Skill skill)
        {
            if (skill == null)
            {

                return NotFound();
            }
            if (ModelState.IsValid)
            {

                _context.Skills.Add(skill);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(skill);
        }


        public IActionResult Edit(int? id)
        {
            var place = _context.Skills.Find(id);

            return View(place);
        }

        [HttpPost]
        public IActionResult Edit(Skill skill)
        {
            if (skill == null)
            {

                return NotFound();
            }
            if (ModelState.IsValid)
            {

                _context.Skills.Update(skill);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(skill);
        }


        public IActionResult Delete(int? id)
        {
            var place = _context.Skills.Find(id);

            return View(place);
        }

        [HttpPost]
        public IActionResult Delete(Skill skill)
        {
            if (skill == null)
            {

                return NotFound();
            }
            if (ModelState.IsValid)
            {

                _context.Skills.Remove(skill);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(skill);
        }











    }
}
