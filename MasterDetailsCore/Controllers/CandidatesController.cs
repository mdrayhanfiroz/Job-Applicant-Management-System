using masterdetailsCore.Models;
using masterdetailsCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace masterdetailsCore.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly JobDbContext _context;
        private readonly IWebHostEnvironment _he;
        public CandidatesController(JobDbContext _context, IWebHostEnvironment _he)
        {
            this._context = _context;
            this._he = _he;
        }
        public IActionResult Index()
        {


            var clients = _context.Candidates
                .Include(x => x.CandidateSkills)
                .ThenInclude(b => b.Skill)
                .OrderByDescending(x => x.CandidateId)
                .ToList();

            return View(clients);





        }

        public IActionResult AddNewSkill(int? id)
        {
            ViewBag.skill = new SelectList(_context.Skills, "SkillId", "SkillName", id.ToString() ?? "");

            return PartialView("_addNewSkills");
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public IActionResult Create(CandidateVM candidateVM, int[] skillId)
        {
            string msg = "";

            if (ModelState.IsValid)
            {
                Candidate c = new Candidate();

                c.CandidateId = candidateVM.CandidateId;
                c.CandidateName = candidateVM.CandidateName;
                c.DateofBirth = candidateVM.DateofBirth;
                c.Phone = candidateVM.Phone;
                c.Fresher = candidateVM.Fresher;


                //for image
                string webroot = _he.WebRootPath;
                string folder = "Candidate_Images";
                string imgFileName = Guid.NewGuid().ToString() + Path.GetFileName(candidateVM.Image.FileName);
                string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    candidateVM.Image.CopyTo(ms);
                    c.Image = ms.ToArray();
                }

                foreach (var item in skillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        Candidate = c,
                        CandidateId = c.CandidateId,
                        SkillId = item

                    };
                    _context.CandidateSkills.Add(candidateSkill);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            return View();
        }


        public IActionResult Edit(int? id)
        {

            Candidate candidate = _context.Candidates.First(x => x.CandidateId == id);

            var candidateskillset = _context.CandidateSkills.Where(x => x.CandidateId == id).ToList();

            CandidateVM candidateVM = new CandidateVM()
            {
                CandidateId = candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                DateofBirth = candidate.DateofBirth,
                Phone = candidate.Phone,
                Fresher = candidate.Fresher,

            };

            if (candidateskillset.Count() > 0)
            {
                foreach (var item in candidateskillset)
                {
                    candidateVM.SkillList.Add(item.SkillId);
                }
            }

            return View(candidateVM);

        }

        [HttpPost]
        public IActionResult Edit(CandidateVM candidateVM, int[] skillId)
        {

            if (ModelState.IsValid)
            {
                Candidate existsClient = _context.Candidates.Find(candidateVM.CandidateId);
                if (existsClient == null)
                {
                    return NotFound();
                }

                existsClient.CandidateName = candidateVM.CandidateName;
                existsClient.DateofBirth = candidateVM.DateofBirth;
                existsClient.Phone = candidateVM.Phone;
                existsClient.Fresher = candidateVM.Fresher;



                //for image
                string webroot = _he.WebRootPath;
                string folder = "Candidate_Images";
                string imgFileName = Guid.NewGuid().ToString() + Path.GetFileName(candidateVM.Image.FileName);
                string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                using (MemoryStream ms = new MemoryStream())
                {
                    candidateVM.Image.CopyTo(ms);
                    existsClient.Image = ms.ToArray();
                }

                var existskillentry = _context.CandidateSkills.Where(x => x.CandidateId == existsClient.CandidateId).ToList();

                foreach (var item in existskillentry)
                {
                    _context.CandidateSkills.Remove(item);
                }


                foreach (var item in skillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        CandidateId = existsClient.CandidateId,
                        SkillId = item

                    };
                    _context.CandidateSkills.Add(candidateSkill);
                }

                _context.Entry(existsClient).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");

            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            Candidate candidate = _context.Candidates.First(x=>x.CandidateId==id);

            var candidateskills = _context.CandidateSkills.Where(x=>x.CandidateId==id).ToList();

            CandidateVM candidateVM = new CandidateVM()
            {
                CandidateId =candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                DateofBirth =candidate.DateofBirth,
                Phone = candidate.Phone,
                Fresher = candidate.Fresher,

            };
            if (candidateskills.Count()>0)
            {
                foreach (var item in candidateskills)
                {
                    candidateVM.SkillList.Add(item.SkillId);
                }
            }

            return View(candidateVM);
        }


        [HttpPost]
        [ActionName("Delete")]

        public IActionResult Delete(int id)
        {
            Candidate candidate = _context.Candidates.Find(id);

            if (candidate==null)
            {
                return NotFound();
            }
            _context.Entry(candidate).State = EntityState.Deleted;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }





    }
}
