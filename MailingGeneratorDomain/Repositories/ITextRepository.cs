using MailingGeneratorDomain.Models;

namespace MailingGeneratorDomain.Repositories
{
    public interface ITextRepository
    {
        Text CreateText(Text text);

        Text GetText(int id);

        void DeleteText(Text text);

        void Update(Text text);

    }
}