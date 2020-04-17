using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.Services;
using MailingsGeneratorBll.Addition;
using MailingsGeneratorDomain.Models;
using MailingsGeneratorDomain.Services;

namespace MailingGeneratorBll.Services
{
    public class TextService : ITextService
    {
        private ITextRepository    _repository;
        private IMailingRepository _repositoryMailing;
        
        public Text CreateText(Text text)
        {
            if (text == null)
            {
                throw new ErrorTypes.NullValue();
            }

            if (text.InfomationPart == null)
            {
                throw  new ErrorTypes.IncorrectName();
            }

            if (text.MailId > 1)
            {
                var mail = _repositoryMailing.GetCourse(text.MailId);
                if (mail == null)
                {
                    throw new ErrorTypes.MailingNotExist();
                }
                else
                {
                    text.Mail = mail;
                }
            }
            
            return _repository.CreateText(text);
        }

        public Text GetText(int id)
        {
            Helpful.CheckId(id);

            var text = _repository.GetText(id);
            
            if (text == null)
            {
                throw new ErrorTypes.TextNotExist();
            }

            return text;

        }

        public void DeleteText(int id)
        {
            Helpful.CheckId(id);
            
            var text = GetText(id);
            if (text == null)
            {
                throw new ErrorTypes.TextNotExist();
            }

            _repository.DeleteText(text);
        }

        public void Update(int id, string information)
        {
            Helpful.CheckId(id);

            if (information == null)
            {
                throw new ErrorTypes.NullValue();
            }

            var text = GetText(id);
            if (text == null)
            {
                throw new ErrorTypes.TextNotExist();
            }

            text.InfomationPart = information;
            _repository.Update(text);
        }

        public TextService(ITextRepository repository, IMailingRepository mailingRepository)
        {
            _repository = repository;
            _repositoryMailing = mailingRepository;
        }
    }
}