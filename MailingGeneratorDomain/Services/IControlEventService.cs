using System.Threading.Tasks;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.RequestObjects;

namespace MailingGeneratorDomain.Services
{
    public interface IControlEventService
    {
        Task<ControlEvent> CreateControlEventAsync(ControlEvent service);
        Task<ControlEvent> GetControlEventAsync (int id);

        Task               UpdateControlEventAsync(UpdateControlEventModel update);

        Task               DeleteAsync(int id);
    }
}