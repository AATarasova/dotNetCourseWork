using System;
using MailingGeneratorDomain.RequestObjects;
using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Services;
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
        public IActionResult AddWork(int courseId, int workId, bool isFinishWork)
        {
            try
            {
                _service.AddWork(courseId, workId, isFinishWork);
            }
            catch (ErrorTypes.NotExist er)
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
        public IActionResult GetCourse (GetMailingObject get)
        {
            try
            {
                return Ok(GetCourse(get));
            }
            catch (ErrorTypes.NotExist er)
            {
                return NotFound(er.StackTrace);
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpPost]
        public IActionResult CreateMailing (MailingsGeneratorDomain.Models.Mailing mail)
        {
            try
            {
                return Created("https://localhost:5001/mailing", _service.CreateMailing(mail));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }   
        }

        [HttpDelete]
        public IActionResult DeleteMailing(int id)
        {
            try
            {
                _service.DeleteMailing(id);
                return NoContent();
            }
            catch (ErrorTypes.MailingNotExist er)
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