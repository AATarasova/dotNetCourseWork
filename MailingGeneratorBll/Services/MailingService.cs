using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;

namespace MailingGeneratorBll.Services
{
    public class MailingService : IMailingService
    {
        private IMailingRepository _repository;
        private IControlEventRepository _eventRepository;
        private ITextRepository _textRepository;

        // Olga, Add:
        public async Task<bool> HasWorkByIdAsync(int mailingId, int workId)
        {
            if (mailingId < 1 || workId < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            if (!await _repository.ExistAsync(mailingId))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }

            if (!await _eventRepository.ExistAsync(workId))
            {
                throw new ExceptionTypes.ControlEventNotExistException();
            }

            return await _repository.HasWorkByIdAsync(mailingId, workId);
        }        
        
        // Olga, Add:
        public async Task<bool> HasTextByIdAsync(int mailingId, int textId)
        {
            if (mailingId < 1 || textId < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            if (!await _repository.ExistAsync(mailingId))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }

            if (!await _textRepository.ExistAsync(textId))
            {
                throw new ExceptionTypes.TextNotExistException();
            }

            return await _repository.HasTextByIdAsync(mailingId, textId);
        }
        
        // Olga, Add:

        public async Task<int> GetCommonNumberOfControlEventsAsync(int mailingId)
        {
            if (mailingId < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            if (!await _repository.ExistAsync(mailingId))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }
            
            return await _repository.GetCommonNumberOfControlEventsAsync(mailingId);
        }
        // Olga, Change:
        public async Task<MailingsGeneratorDomain.Models.Mailing> CreateMailingAsync(
            MailingsGeneratorDomain.Models.Mailing mail)
        {
            if (mail == null)
            {
                throw new ExceptionTypes.NullValueException();
            }
            if (mail.CourseName == null)
            {
                throw new ExceptionTypes.IncorrectNameException();
            }

            CheckDate(mail.StartDate);

            if (mail.TextId != null)
            {
                GetElements<Text> getTexts = _textRepository.GetTextsAsyncById;
                await AddElement(mail.MailText, mail.TextId, getTexts, new ExceptionTypes.TextNotExistException());
            }
            
            if (mail.WorksId != null)
            {
                GetElements<ControlEvent> getElements = _eventRepository.GetControlEventsAsyncById;
                await AddElement(mail.Works, mail.WorksId, getElements, new ExceptionTypes.ControlEventNotExistException());
                foreach (var work in mail.Works)
                {
                    var text = await _textRepository.CreateTextAsync(new Text()
                    {
                        RememberAboutDeadline = "Контрольное мероприятие " + work.ControlEventId,
                        InfomationPart =  "Дедлайн:  " + work.Date
                    });
                    mail.MailText.Add(text);
                }
            }

            if (mail.FinishWorkId != 0)
            {
                var finishWork = await _eventRepository.GetControlEventAsync(mail.FinishWorkId);
                if (finishWork == null)
                {
                    throw new ExceptionTypes.ControlEventNotExistException();
                }

                mail.FinishWork = finishWork;
            }
            
            return await _repository.CreateMailingAsync(mail);
        }
        
        public async Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(int id)
                 {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            var mailing = await  _repository.GetCourseAsync(id);
            if (mailing == null)
            {
                throw new ExceptionTypes.MailingNotExistException();
            }
            return mailing;
        }
        

        // Olga, change
        public async Task UpdateAsync(UpdateMailingModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ExceptionTypes.NullValueException();
            }
            if ((updateModel.FinishId.HasValue && updateModel.FinishId.Value < 1) 
                || updateModel.MailingId < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            
            if (!await _repository.ExistAsync(updateModel.MailingId))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }

            CheckDate(updateModel.StartDate);

            if (updateModel.TextId != null)
            {
                GetElements<Text> getTexts = _textRepository.GetTextsAsyncById;
                await AddElement(updateModel.MailText, updateModel.TextId, getTexts, new ExceptionTypes.TextNotExistException());
            }
            
            if (updateModel.WorksId != null)
            {
                GetElements<ControlEvent> getElements = _eventRepository.GetControlEventsAsyncById;
                await AddElement(updateModel.Works, updateModel.WorksId, getElements, new ExceptionTypes.ControlEventNotExistException());
                foreach (var work in updateModel.Works)
                {
                    var text = await _textRepository.CreateTextAsync(new Text()
                    {
                        RememberAboutDeadline = "Контрольное мероприятие " + work.ControlEventId,
                        InfomationPart =  "Дедлайн:  " + work.Date
                    });
                    updateModel.MailText.Add(text);
                }
            }

            if (updateModel.FinishId.HasValue)
            {
                var control = await _eventRepository.GetControlEventAsync(updateModel.FinishId.Value);
                if (control == null)
                {
                    throw new ExceptionTypes.ControlEventNotExistException();
                }

                updateModel.FinishWork = control;
            }
            
            
            await _repository.UpdateAsync(updateModel);
        }

        public async Task DeleteMailingAsync(int id)
        {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            
          
            if (!await _repository.ExistAsync(id))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }
            await _repository.DeleteMailingAsync(id);
        }

        public async Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(GetMailingModel getMailingModel)
        {
            if (getMailingModel == null || getMailingModel.IsEmpty())
            {
                throw new ExceptionTypes.NullValueException();
            }

            if (getMailingModel.Id.HasValue && getMailingModel.Id.Value < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
          
            return await _repository.GetCourseAsync(getMailingModel);

        }


        public MailingService(IMailingRepository repository, IControlEventRepository eventRepository,
            ITextRepository textRepository)
        {
            _repository      = repository;
            _eventRepository = eventRepository;
            _textRepository  = textRepository;
        }

        
        private async Task AddElement<T>(List<T> list, 
            List<int> idList, GetElements<T> getElements, Exception exception)
        {
            
            if (idList != null)
            {
                var listOfData = await getElements(idList);
                
                if (listOfData.Count != idList.Count)
                {
                    throw exception;
                }

                foreach (var element in listOfData)
                {
                    list.Add(element);
                }
            }
        }

        private delegate Task<List<T>> GetElements<T> (List<int> idList);

        public static void CheckDate(string date)
        {
            bool isCorrect;
            if (date == null)
            {
                isCorrect = true;
            }
            else
            {
                var parts = date.Split('.');
                int day;
                int month;
                isCorrect = parts.Length == 2 && int.TryParse(parts[0], out day) && int.TryParse(parts[1], out month)
                            && day <= 31 && day >= 1 && month <= 12 && month >= 1;
            }
            if (!isCorrect)
            {
                throw new ExceptionTypes.IncorrectDateException();
            }
        }
    }
}