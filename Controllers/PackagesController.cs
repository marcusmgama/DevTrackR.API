using DevTrackR.API.Entities;
using DevTrackR.API.Models;
using DevTrackR.API.Persistence;
using DevTrackR.API.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DevTrackR.API.Controllers
{
    [ApiController]
    [Route("/api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageRepository _repository;
        private readonly ISendGridClient _client;
        public PackagesController(IPackageRepository repository, ISendGridClient client)
        {
            _repository = repository;
            _client = client;
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
        /// <summary>
        /// Cadastro de um pacote.
        /// </summary>
        /// <remarks>
        /// {
        /// "title": "Pacote Cartas Colecionáveis"
        /// "weight": 1.8
        /// "senderName": "FirstSender"
        /// "senderEmail": "marcusmachado17@gmail.com"
        /// }
        /// </remarks>
        /// <param name="model">Dados do pacote</param>
        /// <returns>Objeto recém criado</returns>
        /// <response code ="201">Cadastro realizado com sucesso.</response>
        /// <response code ="400">Dados estão inválidos.</response>
        [HttpPost]
        public async Task<IActionResult> Post(AddPackageInputModel model)
        {
            if (model.Title.Length < 10) {
                return BadRequest("Title lenght must be at least 10 characters");
            }
            var package = new Package(model.Title, model.Weight);

            _repository.Add(package);

            var message = new SendGridMessage{
                From = new EmailAddress("marcusmachado17@gmail.com", "FirstSender"),
                Subject = "Your package was dispatched.",
                PlainTextContent = $"Your package with code {package.Code} was dispatched."
            };

            message.AddTo(model.SenderEmail, model.SenderName);

            await _client.SendEmailAsync(message);

            return CreatedAtAction("GetByCode", new { code = package.Code }, package);

        }
        //POST api/package/guid(identificador)/updates
        [HttpPost("{code}/updates")]
        public async Task<IActionResult> PostUpdate(string code, AddPackageUpdateInputModel model)
        {
            var package = _repository.GetByCode(code);
            if(package == null) {
                return NotFound();
            }
            package.AddUpdate(model.Status, model.Delivered);
            _repository.Update(package);

            var message = new SendGridMessage{
                From = new EmailAddress("marcusmachado17@gmail.com", "FirstSender"),
                Subject = $"Your package has received a new status: {model.Status}",
                PlainTextContent = $"Hello,{model.SenderName} your package received a new status: {model.Status}"
            };
            message.AddTo(model.SenderEmail, model.SenderName);

            await _client.SendEmailAsync(message);

            return NoContent();
        }
    }
}