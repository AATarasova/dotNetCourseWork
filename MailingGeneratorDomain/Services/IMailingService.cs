using System.Threading.Tasks;
using MailingGeneratorDomain.RequestObjects;
using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDomain.Services
{
    public interface IMailingService
    {

        Task<bool> HasWorkByIdAsync(int mailingId, int workId);

        Task<bool> HasTextByIdAsync(int mailingId, int textId);

        Task<int> GetCommonNumberOfControlEventsAsync(int mailingId);
        
        Task<Mailing> CreateMailingAsync(Mailing mail);
        
 
        // Получение id по названию курса:
        Task<Mailing> GetCourseAsync(int id);

        // Добавление работы в курс:
        Task          UpdateAsync(UpdateMailingModel updateModel);
        
        // Удаление рассылки
        Task          DeleteMailingAsync(int id);

        Task<Mailing> GetCourseAsync(GetMailingModel getModel);
    }
}