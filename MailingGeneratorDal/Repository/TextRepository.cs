using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mailing.MailingDal;
using MailingGeneratorDomain.Models;
using MailingGeneratorDomain.Repositories;
using MailingGeneratorDomain.RequestObjects;
using MailingsGeneratorDomain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MailingGeneratorDal.Repository
{
    public class TextRepository : ITextRepository
    {
        private MailingDbContext _dbContext;
        public TextRepository (MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Text> CreateTextAsync (Text text)
        {
            _dbContext.Add(text);
            await _dbContext.SaveChangesAsync();

            return text;
        }

        public async Task<Text> GetTextAsync(int id)
        {
            return await _dbContext.Texts.FirstOrDefaultAsync(t => t.TextId == id);
        }


        public async Task DeleteTextAsync(int id)
        {
            _dbContext.Remove(_dbContext.Texts.FirstOrDefaultAsync(t => t.TextId == id));
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTextModel updateModel)
        {
            var text = await _dbContext.Texts.FirstOrDefaultAsync(t => t.TextId == updateModel.Id);
            if (updateModel.InfomationPart != null)
            {
                text.InfomationPart = updateModel.InfomationPart;
            }
            
            if (updateModel.Mail != null)
            {
                text.Mail = updateModel.Mail;
            }
            
            _dbContext.Update(text);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            var text = await _dbContext.Texts.FirstOrDefaultAsync(t => t.TextId == id);
            return text != null;
        }

        public async Task<List<Text>> GetTextsAsyncById(List<int> idList)
        {
            return await  _dbContext.Texts.Where(t => idList.Contains(t.TextId)).ToListAsync();
        }
    }
}