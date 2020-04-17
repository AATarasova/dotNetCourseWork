using System.Linq;
using Mailing.MailingDal;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDal.Repository
{
    public class TextRepository : ITextRepository
    {
        private MailingDbContext _dbContext;
        
        public Text CreateText (Text text)
        {
            _dbContext.Add(text); // на запись в базу
            _dbContext.SaveChanges();

            return text;
        }

        public Text GetText(int id)
        {
            return _dbContext.Texts.AsEnumerable().FirstOrDefault(t => t.TextId == id);
        }

        public TextRepository (MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteText(Text text)
        {
            _dbContext.Remove(text);
        }

        public void Update(Text text)
        {
            _dbContext.Update(text);
            _dbContext.SaveChanges();
        }
    }
}