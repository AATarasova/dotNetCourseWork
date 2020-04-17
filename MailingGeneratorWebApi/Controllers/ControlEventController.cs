using System;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.RequestObjects;
using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Models;
using MailingsGeneratorDomain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailingGeneratorWebApi.Controllers
{   
    
    [ApiController]
    [Route("[controller]")]
    public class ControlEventController : ControllerBase
    {
        private readonly ILogger<ControlEventController> _controller;
        private IControlEventService _service;
        
        
        public ControlEventController(IControlEventService service, ILogger<ControlEventController> controller)
        {
            _controller = controller;
            _service = service;
        }

        [HttpGet]
        public IActionResult GetControlEvent(int id)
        {
            try
            {
                return Ok(_service.GetControlEvent(id));
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
        public IActionResult CreateControlEvent (ControlEvent controlEvent)
        {
            try
            {
                return Ok(_service.CreateControlEvent(controlEvent));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpPut]
        public IActionResult UpdateControlEvent(UpdateControlEventObject update)
        {
            try
            {
                _service.UpdateControlEvent(update);
                return Ok("Обновлено.");
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
        
        [HttpDelete]
        public IActionResult DeleteMailing(int id)
        {
            try
            {
                _service.Delete(id);
                return NoContent();
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
    }
}