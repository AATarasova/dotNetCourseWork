﻿using System.Collections.Generic;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.Repositories;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Models;
 using MailingsGeneratorDomain.Services;

namespace MailingsGeneratorBll.Services
{
    public class MailingService : IMailingService
    {
        private IMailingRepository _repository;
        private IControlEventRepository _eventRepository;
        private ITextRepository _textRepository;

        // Создание рассылки
        public MailingsGeneratorDomain.Models.Mailing CreateMailing(MailingsGeneratorDomain.Models.Mailing mail)
        {
            if (mail == null)
            {
                throw new ErrorTypes.NullValue();
            }
            if (mail.CourseName == null)
            {
                throw new ErrorTypes.IncorrectName();
            }

            Helpful.CheckDate(mail.StartDate);

            if (mail.WorksId != null)
            {
                mail.Works = new List<ControlEvent>();
                foreach (var workId in mail.WorksId)
                {
                    var work = _eventRepository.GetControlEvent(workId);
                    if (work == null)
                    {
                        throw new ErrorTypes.ControlEventNotExist();
                    }
                    mail.Works.Add(work);
                }
            }

            if (mail.FinishWorkId != 0)
            {
                var finishWork = _eventRepository.GetControlEvent(mail.FinishWorkId);
                if (finishWork == null)
                {
                    throw new ErrorTypes.ControlEventNotExist();
                }
                mail.FinishWork = finishWork;
            }

            if (mail.TextId != null)
            {
                foreach (var textId in mail.TextId)
                {
                    var text = _textRepository.GetText(textId);
                    if (text == null)
                    {
                        throw new ErrorTypes.TextNotExist();
                    }
                    mail.MailText.Add(text);
                }
            }
            return _repository.CreateMailing(mail);
        }
        
        // Имя курса по id его рассылки:
        public string GetCourseName(int id)
        {
            Helpful.CheckId(id);
            var mailing = _repository.GetCourse(id);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }

            return mailing.CourseName;
        }

        // id рассылки по имени ее курса:
        public int GetCourseId(string name)
        {
            if (name == null)
            {
                throw new ErrorTypes.IncorrectName();
            }
            var mailing = _repository.GetCourse(name);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }

            return mailing.MailingId;
        }

        public MailingsGeneratorDomain.Models.Mailing GetCourse(int id)
        {
            Helpful.CheckId(id);
            var mailing = _repository.GetCourse(id);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }
            return mailing;
        }
        
        public MailingsGeneratorDomain.Models.Mailing GetCourse(string name) 
        {                      
            if (name == null)
            {
                throw new ErrorTypes.IncorrectName();
            }  
            var mailing = _repository.GetCourse(name);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }
            return mailing;
        }

        public void AddWork(int courseId, int workId, bool isFinish)
        {
            Helpful.CheckId(courseId);
            Helpful.CheckId(workId);
            
            var mailing = _repository.GetCourse(courseId);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }

            if (mailing.Works == null)
            {
                mailing.Works = new List<ControlEvent>();
            }

            var controlEvent = _eventRepository.GetControlEvent(workId);
            if (controlEvent == null)
            {
                throw new ErrorTypes.ControlEventNotExist();
            }

            
            if ((isFinish && mailing.FinishWork != null)  || (!isFinish && mailing.Works.Contains(controlEvent)))
            {
                
                throw new ErrorTypes.AlreadyContains();
            }

            if (isFinish)
            {
                mailing.FinishWork = controlEvent;
            }
            else
            {
                mailing.Works.Add(controlEvent);
            }
            _repository.Update(mailing);
        }

        public List<ControlEvent> GetControlEvents(int id)
        {
            Helpful.CheckId(id);
            
            var mailing = _repository.GetCourse(id);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }

            return mailing.Works;
        }

        public ControlEvent GetFinishEvent(int id)
        {
            Helpful.CheckId(id);
            
            var mailing = _repository.GetCourse(id);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }

            return mailing.FinishWork;
        }

        public void DeleteMailing(int id)
        {
            Helpful.CheckId(id);
            
            var mailing = _repository.GetCourse(id);
            if (mailing == null)
            {
                throw new ErrorTypes.MailingNotExist();
            }
            _repository.DeleteMailing(mailing);
        }

        public MailingsGeneratorDomain.Models.Mailing GetCourse(GetMailingObject getMailingObject)
        {
            GetMailingObject.TypeOfGetParameter type;
            if (getMailingObject == null || 
                (type = getMailingObject.TypeOfRequest()) == GetMailingObject.TypeOfGetParameter.NoParameter)
            {
                throw new ErrorTypes.NullValue();
            }

            return type == GetMailingObject.TypeOfGetParameter.Id ?
                GetCourse(getMailingObject.Id.Value) : GetCourse(getMailingObject.Name);

        }


        public MailingService(IMailingRepository repository, IControlEventRepository eventRepository,
            ITextRepository textRepository)
        {
            _repository      = repository;
            _eventRepository = eventRepository;
            _textRepository  = textRepository;
        }
        
    }
}