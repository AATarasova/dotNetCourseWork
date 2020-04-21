using System.Linq;
using System.Threading.Tasks;
using Mailing.MailingDal;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using Microsoft.EntityFrameworkCore;

namespace MailingGeneratorDal.Repository
{
    public class MailingRepository : IMailingRepository
    {
        private MailingDbContext _dbContext;
        
        // Anna
        public async Task<bool> HasWorkByIdAsync(int mailingId, int workId)
        {
            var mailing = await GetCourseAsync(mailingId);
            return mailing.Works.Any(work => work.ControlEventId == workId);
        }    
        
        // Anna
        public async Task<bool> HasTextByIdAsync(int mailingId, int textId)
        {
            var mailing = await GetCourseAsync(mailingId);
            return mailing.MailText.Any(text => text.TextId == textId);
        }

        // Anna
        public async Task<int> GetCommonNumberOfControlEventsAsync(int mailingId)
        {
            var mailing = await GetCourseAsync(mailingId);
            return (mailing.FinishWork == null) ? mailing.Works.Count : mailing.Works.Count + 1;
        }


        // Создание рассылки
        public async Task<MailingsGeneratorDomain.Models.Mailing> CreateMailingAsync(
            MailingsGeneratorDomain.Models.Mailing mail)
        {
            _dbContext.Mailings.Add(mail); // на запись в базу
            await _dbContext.SaveChangesAsync();

            return mail;
        }


        // Получение названия курса по id:
        public async Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(GetMailingModel getModel)
        {
            if (getModel.Id.HasValue)
            {
                return await _dbContext.Mailings.FirstOrDefaultAsync(m => m.MailingId == getModel.Id.Value);
            }
            return await _dbContext.Mailings.FirstOrDefaultAsync(m => m.CourseName.Equals(getModel.Name));
        }        
        
        // Получение названия курса по id:
        public async Task<MailingsGeneratorDomain.Models.Mailing> GetCourseAsync(int id)
        {
            return await _dbContext.Mailings.FirstOrDefaultAsync(m => m.MailingId == id);
        }

        public async Task UpdateAsync(UpdateMailingModel updateModel)
        {
            var mailing = await _dbContext.Mailings.FirstOrDefaultAsync(m => m.MailingId == updateModel.MailingId);
            if (updateModel.StartDate != null)
            {
                mailing.StartDate = updateModel.StartDate;
            }            
            
            if (updateModel.HelloText != null)
            {
                mailing.HelloText = updateModel.HelloText;
            }
            
            if (updateModel.FinishWork != null)
            {
                mailing.FinishWork = updateModel.FinishWork;
            }

            if (updateModel.Works != null)
            {
                foreach (var work in updateModel.Works)
                {
                    mailing.Works.Add(work);
                }
            }
            
            if (updateModel.MailText != null)
            {
                foreach (var mText in updateModel.MailText)
                {
                    mailing.MailText.Add(mText);
                }
            }
            
            
            _dbContext.Mailings.Update(mailing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMailingAsync(int id)
        {
            var mailing = await _dbContext.Mailings.FirstOrDefaultAsync(m => m.MailingId == id);
            _dbContext.Mailings.Remove(mailing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            var mailing = await _dbContext.Mailings.FirstOrDefaultAsync(m => m.MailingId == id);
            return mailing != null;
        }
        
        public MailingRepository(MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }

    public class GetCourseModel
    {
    }
}