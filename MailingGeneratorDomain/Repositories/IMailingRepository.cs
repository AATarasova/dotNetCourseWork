﻿ using System.Threading.Tasks;
  using MailingGeneratorDomain.RequestObjects;

  namespace MailingGeneratorDomain.Repositories
{
    public interface IMailingRepository
    {
        Task<MailingsGeneratorDomain.Models.Mailing> CreateMailingAsync( MailingsGeneratorDomain.Models.Mailing mail);
        
        // Получение id по названию курса:
        Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(GetMailingModel getModel);

        Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(int id);
        // Добавление работы в курс:
        Task UpdateAsync(UpdateMailingModel updateModel);
        
        // Удаление рассылки
        Task DeleteMailingAsync(int id);
        
        Task<bool> ExistAsync(int id);
    }
    
}