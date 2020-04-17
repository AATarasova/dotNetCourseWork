using System;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Services;
using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Models;
using MailingsGeneratorDomain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MailingGeneratorWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TextController : ControllerBase
    {
        private readonly ILogger<TextController> _controller;
        private ITextService _service;


        public TextController(ITextService service, ILogger<TextController> controller)
        {
            _controller = controller;
            _service = service;
        }


        [HttpPost]
        public IActionResult CreateText(Text text)
        {
            try
            {
                return Ok(_service.CreateText(text));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpGet]
        public IActionResult GetText(int id)
        {
            try
            {
                var text = _service.GetText(id);
                return Ok(text);
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

        [HttpPut]
        public IActionResult UpdateText(int id, string text)
        {
            try
            {
                _service.Update(id, text);
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
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteText(id);
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