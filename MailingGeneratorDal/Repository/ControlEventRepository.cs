﻿using System.Linq;
 using System.Threading.Tasks;
 using Mailing.MailingDal;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.Repositories;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorDomain.Models;
 using Microsoft.EntityFrameworkCore;

 namespace MailingsGeneratorDal.Repository
{
    public class ControlEventRepository : IControlEventRepository
    {
        private MailingDbContext _dbContext;
        
        public async Task<ControlEvent> CreateControlEventAsync(ControlEvent controlEvent)
        {
            await _dbContext.ControlEvents.AddAsync(controlEvent); // на запись в базу
            await _dbContext.SaveChangesAsync();

            return controlEvent;
        }
        
        public async Task<ControlEvent> GetControlEventAsync(int id)
        {
            return await _dbContext.ControlEvents.FirstOrDefaultAsync(ce => ce.MailId == id);
        }

        public async Task UpdateAsync(UpdateControlEventModel updateModel)
        {
            var controlEvent = await _dbContext.ControlEvents.FirstOrDefaultAsync(ce 
                => ce.MailId == updateModel.Id);
            if (updateModel.Date != null)
            {
                controlEvent.Date = updateModel.Date;
            }
            if (updateModel.MaxMark.HasValue)
            {
                controlEvent.MaxMark = updateModel.MaxMark.Value;
            }
            _dbContext.ControlEvents.Update(controlEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var controlEvent = await _dbContext.ControlEvents.FirstOrDefaultAsync(ce => ce.MailId == id);
            _dbContext.Remove(controlEvent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(int id)
        {
            var controlEvent = await _dbContext.ControlEvents.FirstOrDefaultAsync(ce => ce.MailId == id);
            return controlEvent != null;
        }
        
        public ControlEventRepository(MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }

}