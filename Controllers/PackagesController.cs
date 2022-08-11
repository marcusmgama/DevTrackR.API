using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistence;
using DevTrackR.API.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevTrackR.API.Controllers
{
    [ApiController]
    [Route("/api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        public PackagesController(IPackageRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var packages = _repository.GetAll();
            return Ok(packages);
        }
        // GET api/packages/guid(identificador)
        [HttpGet("{code}")]
        public IActionResult GetByCode(string code)
        {
            var package = _repository.GetByCode(code);

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
            _repository.Add(package);
            return CreatedAtAction("GetByCode", new { code = package.Code }, package);

        }
        //POST api/package/guid(identificador)/updates
        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model)
        {
            var package = _repository.GetByCode(code);
            if(package == null) {
                return NotFound();
            }
            package.AddUpdate(model.Status, model.Delivered);
            _repository.Update(package);

            return NoContent();
        }
    }
}