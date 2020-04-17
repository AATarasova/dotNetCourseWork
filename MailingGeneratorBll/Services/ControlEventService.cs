﻿using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.Repositories;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Models;
 using MailingsGeneratorDomain.Services;

namespace MailingsGeneratorBll.Services
{
    public class ControlEventService : IControlEventService
    {
        private IControlEventRepository _repository;
        public ControlEvent CreateControlEvent(ControlEvent controlEvent)
        {
            if (controlEvent == null)
            {
                throw new ErrorTypes.NullValue();
            }

            if (controlEvent.Name == null)
            {
                throw new ErrorTypes.IncorrectName();
            }
            return _repository.CreateControlEvent(controlEvent);
        }

        // Получение контрольного метроприятия по его id:
        public ControlEvent GetControlEvent(int id)
        {
            Helpful.CheckId(id);
            var controlEvent = _repository.GetControlEvent(id);
            if (controlEvent == null)
            {
                throw new ErrorTypes.ControlEventNotExist();
            }
            return controlEvent;
        }

        public void UpdateControlEvent(UpdateControlEventObject update)
        {
            Helpful.CheckId(update.Id);
            
            if (update.IsEmpty())
            {
                throw new ErrorTypes.NullValue();
            }
            
            var controlEvent = _repository.GetControlEvent(update.Id);
            if (controlEvent == null)
            {
                throw new ErrorTypes.IdNotFound();
            }

            if (update.MaxMark > 0)
            {
                controlEvent.MaxMark = update.MaxMark.Value;
            }

            if (update.Date != null)
            {
                controlEvent.Date = update.Date;
            }
            
            _repository.Update(controlEvent);
        }

        public void Delete(int id)
        {
            Helpful.CheckId(id);
            
            var controlEvent = _repository.GetControlEvent(id);
            if (controlEvent == null)
            {
                throw new ErrorTypes.ControlEventNotExist();
            }
            
            _repository.Delete(controlEvent);
        }

        public ControlEventService(IControlEventRepository repository)
        {
            _repository = repository;
        }


        // Вспомогательные
        private void UpdateDate(ControlEvent controlEvent, string date)
        {
            Helpful.CheckDate(date);
            controlEvent.Date = date;
            _repository.Update(controlEvent);
        }

        private void UpdateMaxMark(ControlEvent controlEvent, int maxMark)
        {
            if (maxMark < 0)
            {
                throw new ErrorTypes.IncorrectMark();
            } 
            
            _repository.Update(controlEvent);
        }
    }
}