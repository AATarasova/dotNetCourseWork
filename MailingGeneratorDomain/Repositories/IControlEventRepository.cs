﻿using System.Threading.Tasks;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorDomain.Models;

 namespace MailingGeneratorDomain.Repositories
{
    public interface IControlEventRepository
    {
        Task<ControlEvent> CreateControlEventAsync(ControlEvent controlEvent);
        Task<ControlEvent> GetControlEventAsync(int id);

        Task               UpdateAsync(UpdateControlEventModel updateModel);

        Task               DeleteAsync(int id);
        
        Task<bool>         ExistAsync(int id);
    }
}