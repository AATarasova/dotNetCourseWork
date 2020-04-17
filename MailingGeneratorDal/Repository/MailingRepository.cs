using System.Linq;
using Mailing.MailingDal;
using MailingGeneratorDomain.Repositories;

namespace MailingGeneratorDal.Repository
{
    public class MailingRepository : IMailingRepository
    {
        private MailingDbContext _dbContext;
        
        // Создание рассылки
        public MailingsGeneratorDomain.Models.Mailing CreateMailing(MailingsGeneratorDomain.Models.Mailing mail)
        {
            _dbContext.Mailings.Add(mail); // на запись в базу
            _dbContext.SaveChanges();

            return mail;
        }

        // Получение id по названию курса:
        public MailingsGeneratorDomain.Models.Mailing GetCourse(int id)
        {
            return _dbContext.Mailings.AsEnumerable().FirstOrDefault(m => m.MailingId == id);
        }
        
        // Получение названия курса по id:
        public MailingsGeneratorDomain.Models.Mailing GetCourse(string name)
        {
            return _dbContext.Mailings.AsEnumerable().FirstOrDefault(m => m.CourseName.Equals(name));
        }

        // Обновление рассылки в базе данных:
        public void Update (MailingsGeneratorDomain.Models.Mailing mailing)
        {
            _dbContext.Mailings.Update(mailing);
            _dbContext.SaveChanges();
        }


        public void DeleteMailing(MailingsGeneratorDomain.Models.Mailing mailing)
        {
            _dbContext.Mailings.Remove(mailing);
        }


        public MailingRepository(MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}