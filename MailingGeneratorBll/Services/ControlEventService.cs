using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;
using MailingsGeneratorBll.Services;

namespace MailingGeneratorBll.Services
{
    public class ControlEventService : IControlEventService
    {
        private IControlEventRepository _repository;
        public async Task<ControlEvent> CreateControlEventAsync(ControlEvent controlEvent)
        {
            if (controlEvent == null)
            {
                throw new ExceptionTypes.NullValueException();
            }

            if (controlEvent.Name == null)
            {
                throw new ExceptionTypes.IncorrectNameException();
            }
            return await _repository.CreateControlEventAsync(controlEvent);
        }

        // Получение контрольного метроприятия по его id:
        public async Task<ControlEvent> GetControlEventAsync(int id)
        {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            var controlEvent = await _repository.GetControlEventAsync(id);
            if (controlEvent == null)
            {
                throw new ExceptionTypes.ControlEventNotExistException();
            }
            return controlEvent;
        }

        public async Task UpdateControlEventAsync(UpdateControlEventModel updateModel)
        {
            if (updateModel.Id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            
            if (updateModel.IsEmpty())
            {
                throw new ExceptionTypes.NullValueException();
            }
            
            if (!await _repository.ExistAsync(updateModel.Id))
            {
                throw new ExceptionTypes.IdNotFoundException();
            }

            if (updateModel.MaxMark > 10)
            {
                throw new ExceptionTypes.IncorrectMarkException();
            }

            MailingService.CheckDate(updateModel.Date);
            
            
            await _repository.UpdateAsync(updateModel);
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            var controlEvent = _repository.GetControlEventAsync(id);
            if (controlEvent == null)
            {
                throw new ExceptionTypes.ControlEventNotExistException();
            }
            
            await _repository.DeleteAsync(id);
        }

        public ControlEventService(IControlEventRepository repository)
        {
            _repository = repository;
        }
    }
}