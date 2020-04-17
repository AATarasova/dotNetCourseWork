﻿using MailingGeneratorDomain.Models;
 using MailingsGeneratorDomain.Models;

 namespace MailingGeneratorDomain.Repositories
{
    public interface IControlEventRepository
    {
        ControlEvent CreateControlEvent(ControlEvent controlEvent);
        ControlEvent GetControlEvent (int id);

        void Update(ControlEvent controlEvent);

        void Delete(ControlEvent controlEvent);
    }
}