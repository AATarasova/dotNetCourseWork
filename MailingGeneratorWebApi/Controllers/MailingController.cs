using System;
using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailingGeneratorWebApi.Controllers
{   
    
    [ApiController]
    [Route("[controller]")]
    public class MailingController : ControllerBase
    {
        private readonly ILogger<MailingController> _controller;
        private IMailingService _service;
        
        
        public MailingController(IMailingService service, ILogger<MailingController> controller)
        {
            _controller = controller;
            _service = service;
        }

        [HttpPut]
        //public IActionResult AddWork([FromBody]int courseId, [FromBody]int workId, [FromBody]bool isFinishWork)
        public async Task<IActionResult> AddWorkAsync(UpdateMailingModel updateModel)
        {
            try
            {
                await _service.UpdateAsync(updateModel);
            }
            catch (ExceptionTypes.NotExistException er)
            {
                return NotFound(er.StackTrace);
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
            return new OkResult();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCourseAsync (GetMailingModel getModel)
        {
            try
            {
                return Ok(await GetCourseAsync(getModel));
            }
            catch (ExceptionTypes.NotExistException er)
            {
                return NotFound(er.StackTrace);
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMailingAsync (MailingsGeneratorDomain.Models.Mailing mail)
        {
            try
            {
                return Created("https://localhost:5001/mailing", await _service.CreateMailingAsync(mail));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }   
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMailingAsync(int id)
        {
            try
            {
                await _service.DeleteMailingAsync(id);
                return NoContent();
            }
            catch (ExceptionTypes.MailingNotExistException er)
            {
                return NotFound(er.StackTrace);
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }
        
    }
}