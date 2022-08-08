using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DevTrackR.API.Controllers
{
    [ApiController]
    [Route("/api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly DevTrackRContext _context;
        public PackagesController(DevTrackRContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var packages = _context.Packages;
            return Ok(packages);
        }
        // GET api/packages/guid(identificador)
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code)
        {
            var package = _context.Packages.SingleOrDefault(p => p.Code == code);
            if(package == null) {
                return NotFound();
            }
            return Ok(package);

        }
        //POST api/packages
        [HttpPost]
        public IActionResult Post(AddPackageInputModel model)
        {
            if (model.Title.Length < 10) {
                return BadRequest("Title lenght must be at least 10 characters");
            }
            var package = new Package(model.Title, model.Weight);
            _context.Packages.Add(package);
            return CreatedAtAction("GetByCode", new { code = package.Code }, package);

        }
        //POST api/package/guid(identificador)/updates
        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model)
        {
            var package = _context.Packages.SingleOrDefault(p => p.Code == code);
            if(package == null) {
                return NotFound();
            }
            package.AddUpdate(model.Status, model.Delivered);
            return NoContent();
        }
    }
}