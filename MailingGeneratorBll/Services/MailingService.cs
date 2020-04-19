﻿using System;
 using System.Collections.Generic;
 using System.Threading.Tasks;
 using MailingGeneratorBll.Addition;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.Repositories;
 using MailingGeneratorDomain.RequestObjects;
 using MailingGeneratorDomain.Services;


 namespace MailingsGeneratorBll.Services
{
    public class MailingService : IMailingService
    {
        private IMailingRepository _repository;
        private IControlEventRepository _eventRepository;
        private ITextRepository _textRepository;

        // Создание рассылки
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

            if (mail.WorksId != null)
            {
                mail.Works = new List<ControlEvent>();
                foreach (var workId in mail.WorksId)
                {
                    var work = await _eventRepository.GetControlEventAsync(workId);
                    if (work == null)
                    {
                        throw new ExceptionTypes.ControlEventNotExistException();
                    }
                    mail.Works.Add(work);
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

            if (mail.TextId != null)
            {
                foreach (var textId in mail.TextId)
                {
                    var text = await _textRepository.GetTextAsync(textId);
                    if (text == null)
                    {
                        throw new ExceptionTypes.TextNotExistException();
                    }
                    mail.MailText.Add(text);
                }
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


            if (updateModel.TextId != null)
            {
                GetElement<Text> getText = _textRepository.GetTextAsync;
                await AddElement(updateModel.MailText, updateModel.TextId, getText);
            }
            
            if (updateModel.WorksId != null)
            {
                GetElement<ControlEvent> getText = _eventRepository.GetControlEventAsync;
                await AddElement(updateModel.Works, updateModel.WorksId, getText);
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
          
            return await _repository.GetCourseAsync(getMailingModel);

        }


        public MailingService(IMailingRepository repository, IControlEventRepository eventRepository,
            ITextRepository textRepository)
        {
            _repository      = repository;
            _eventRepository = eventRepository;
            _textRepository  = textRepository;
        }

        
        private async Task AddElement<T>(ICollection<T> list, 
            IReadOnlyCollection<int> idList, GetElement<T> getControlEvent)
        {
            if (idList != null)
            {
                foreach (var id in idList)
                {
                    var controlEvent = await getControlEvent(id);
                    if (controlEvent == null)
                    {
                        throw new ExceptionTypes.TextNotExistException();
                    }
                    list.Add(controlEvent);
                }
            }
        }

        private delegate Task<T> GetElement<T> (int id);

        public static void CheckDate(string date)
        {
            bool isCorrect;
            if (date == null)
            {
                isCorrect = false;
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