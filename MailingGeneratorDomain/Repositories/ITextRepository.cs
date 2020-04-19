using System.Collections.Generic;
using System.Threading.Tasks;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.RequestObjects;

namespace MailingGeneratorDomain.Repositories
{
    public interface ITextRepository
    {
        Task<Text> CreateTextAsync(Text text);

        Task<Text> GetTextAsync(int id);

        Task DeleteTextAsync(int id);

        Task UpdateAsync(UpdateTextModel updateModel);

        Task<bool> ExistAsync(int id);

        Task<List<Text>> GetTextsAsyncById(List<int> idList);

    }
}