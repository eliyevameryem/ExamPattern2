using Exam.DAL;
using Exam.Helpers;
using Exam.Models;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeamController(AppDbContext context,IWebHostEnvironment env)
        {
            this._context = context;
            this._env = env;
        }
        public IActionResult Index()
        {
            List<TeamSection> teamSections = _context.TeamSections.ToList();
            return View(teamSections);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeamSection teamSection)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            if(teamSection.ImageFile == null) 
            {
                ModelState.AddModelError("ImageFile", "Sekil daxil edin");
                return View();
            }

            if(!teamSection.ImageFile.CheckType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Sekil daxil edin");
                return View();
            }
            if (teamSection.ImageFile.CheckLength(200))
            {
                ModelState.AddModelError("ImageFile", "Sekilin olcusu 200 mb cox ola bilmez");
                return View();
            }

            teamSection.Image = teamSection.ImageFile.CreateFile(_env.WebRootPath, "upload/team");
                await _context.TeamSections.AddAsync(teamSection);


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            if (id ==null) return BadRequest();
            TeamSection section= _context.TeamSections.FirstOrDefault(s => s.Id == id);
            if (section == null) return NotFound();
            
                         
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id,TeamSection teamSection)
        {
            if (id <= 0) return BadRequest();
            TeamSection section = _context.TeamSections.FirstOrDefault(s => s.Id == id);
            if (section == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (teamSection.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Sekil daxil edin");
                if (teamSection.ImageFile.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Sekil daxil edin");
                    return View();
                }
                if (teamSection.ImageFile.CheckLength(200))
                {
                    ModelState.AddModelError("ImageFile", "Sekilin olcusu 200 mb cox ola bilmez");
                    return View();
                }


                teamSection.Image = teamSection.ImageFile.CreateFile(_env.WebRootPath, "upload/team");
                teamSection.Image.DeleteFile(_env.WebRootPath, "upload/team");

            }

            section.Title = teamSection.Title;
            section.Description = teamSection.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0) return BadRequest();
            TeamSection teamSection = _context.TeamSections.FirstOrDefault(s => s.Id == id);
            if (teamSection == null) return NotFound();
            teamSection.Image.DeleteFile(_env.WebRootPath, "upload/team");
            _context.TeamSections.Remove(teamSection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
