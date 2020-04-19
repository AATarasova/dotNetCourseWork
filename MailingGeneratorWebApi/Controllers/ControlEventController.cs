using System;
using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;
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
        public async Task<IActionResult> GetControlEventAsync(int id)
        {
            try
            {
                return Ok(await _service.GetControlEventAsync(id));
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
        public async Task<IActionResult> CreateControlEvent (ControlEvent controlEvent)
        {
            try
            {
                return Ok(await _service.CreateControlEventAsync(controlEvent));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateControlEvent(UpdateControlEventModel update)
        {
            try
            {
                await _service.UpdateControlEventAsync(update);
                return Ok("Обновлено.");
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
        
        [HttpDelete]
        public async Task<IActionResult> DeleteMailing(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
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
    }
}