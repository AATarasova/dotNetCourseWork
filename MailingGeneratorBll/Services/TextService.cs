using System.Threading.Tasks;
using MailingGeneratorBll.Addition;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingGeneratorDomain.Services;




namespace MailingGeneratorBll.Services
{
    public class TextService : ITextService
    {
        private ITextRepository    _repository;
        private IMailingRepository _repositoryMailing;
        
        public async Task<Text> CreateTextAsync(Text text)
        {
            if (text == null)
            {
                throw new ExceptionTypes.NullValueException();
            }

            if (text.InfomationPart == null)
            {
                throw  new ExceptionTypes.IncorrectNameException();
            }

            if (text.MailId > 1)
            {
                var mail = await _repositoryMailing.GetCourseAsync(text.MailId);
                if (mail == null)
                {
                    throw new ExceptionTypes.MailingNotExistException();
                }

                text.Mail = mail;
            }
            
            return await _repository.CreateTextAsync(text);
        }

        public async Task<Text> GetTextAsync(int id)
        {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            var text = await _repository.GetTextAsync(id);
            
            if (text == null)
            {
                throw new ExceptionTypes.TextNotExistException();
            }

            return text;

        }

        public async Task DeleteTextAsync(int id)
        {
            if (id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }
            
            if (!await _repository.ExistAsync(id))
            {
                throw new ExceptionTypes.TextNotExistException();
            }

            await _repository.DeleteTextAsync(id);
        }

        public async Task UpdateAsync(UpdateTextModel updateModel)
        {
            if (updateModel == null || updateModel.IsEmpty())
            {
                throw new ExceptionTypes.NullValueException();
            }
            if (updateModel.Id < 1)
            {
                throw new ExceptionTypes.IncorrectIdException();
            }

            if (!await _repository.ExistAsync(updateModel.Id))
            {
                throw new ExceptionTypes.TextNotExistException();
            }

            if (updateModel.MailingId.HasValue && !await _repositoryMailing.ExistAsync(updateModel.MailingId.Value))
            {
                throw new ExceptionTypes.MailingNotExistException();
            }
            else if (updateModel.MailingId.HasValue)
            {
                updateModel.Mail = await _repositoryMailing.GetCourseAsync(updateModel.MailingId.Value);
            }

            await _repository.UpdateAsync(updateModel);
        }

        public TextService(ITextRepository repository, IMailingRepository mailingRepository)
        {
            _repository = repository;
            _repositoryMailing = mailingRepository;
        }
    }
}