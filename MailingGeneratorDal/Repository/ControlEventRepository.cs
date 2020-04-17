﻿using System.Linq;
using Mailing.MailingDal;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.Repositories;
 using MailingsGeneratorDomain.Models;

 namespace MailingsGeneratorDal.Repository
{
    public class ControlEventRepository : IControlEventRepository
    {
        private MailingDbContext _dbContext;

        public ControlEvent CreateControlEvent(ControlEvent controlEvent)
        {
            //_dbContext.ControlEvents.Where(r => r.MailId == 9)
            //_dbContext.ControlEvents.Select(ce => ce.MailId)
            //_dbContext.ControlEvents.ToList()
            // _dbContext.ControlEvents.Any() есть ли вообще    
            _dbContext.ControlEvents.Add(controlEvent); // на запись в базу
            _dbContext.SaveChanges();

            return controlEvent;
        }


        public ControlEvent GetControlEvent(int id)
        {
            return _dbContext.ControlEvents.AsEnumerable().FirstOrDefault(ce => ce.MailId == id);
        }

        public void Update(ControlEvent controlEvent)
        {
            _dbContext.ControlEvents.Update(controlEvent);
            _dbContext.SaveChanges();
        }

        public void Delete(ControlEvent controlEvent)
        {
            _dbContext.Remove(controlEvent);
        }


        public ControlEventRepository(MailingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }

}