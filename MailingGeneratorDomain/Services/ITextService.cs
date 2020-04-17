using MailingGeneratorDomain.Models;

namespace MailingGeneratorDomain.Services
{
    public interface ITextService
    {
        Text CreateText(Text text);

        Text GetText(int id);
        
        
        void DeleteText(int id);

        void Update(int id, string information);
    }
}