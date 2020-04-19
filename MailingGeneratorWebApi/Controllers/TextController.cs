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
        public async Task<IActionResult> CreateTextAsync(Text text)
        {
            try
            {
                return Ok(await _service.CreateTextAsync(text));
            }
            catch (Exception er)
            {
                return BadRequest(er.StackTrace);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTextAsync(int id)
        {
            try
            {
                var text = await _service.GetTextAsync(id);
                return Ok(text);
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

        [HttpPut]
        public async Task<IActionResult> UpdateTextAsync(UpdateTextModel updateModel)
        {
            try
            {
                await _service.UpdateAsync(updateModel);
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
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                await _service.DeleteTextAsync(id);
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