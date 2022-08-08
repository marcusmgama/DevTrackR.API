using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTrackR.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DevTrackR.API.Controllers
{
    [ApiController]
    [Route("/api/packages")]
    public class PackagesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() {
            var packages = new List<Package>{
                new Package("Pacote 1", 1.3m),
                new Package("Pacote 2", 0.2m),
            };
            return Ok(packages);
        }
    [HttpGet("{code}")]
        public IActionResult GetByCode(string code) {
            var package = new Package ("Pacote 2", 0.2m);
            return Ok(package);

        }
        //POST api/packages
        [HttpPost]
        public IActionResult Post(Package package) {
            return Ok(package);
            
        }
        //POST api/package/guid(identificador)/updates
        [HttpPost("{code}")]
        public IActionResult PostUpdates(string code) {
            return Ok();
        }
     }
}