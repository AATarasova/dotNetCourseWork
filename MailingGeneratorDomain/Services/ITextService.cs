using System.Threading.Tasks;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.RequestObjects;

namespace MailingGeneratorDomain.Services
{
    public interface ITextService
    {
        Task<Text> CreateTextAsync(Text text);

        Task<Text> GetTextAsync(int id);
        
        
        Task       DeleteTextAsync(int id);

        Task       UpdateAsync(UpdateTextModel updateModel);
    }
}