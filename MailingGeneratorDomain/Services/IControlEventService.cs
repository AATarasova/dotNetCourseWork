﻿using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorDomain.Models;

namespace MailingsGeneratorDomain.Services
{
    public interface IControlEventService
    {
        ControlEvent CreateControlEvent(ControlEvent service);
        ControlEvent GetControlEvent (int id);

        void UpdateControlEvent(UpdateControlEventObject update);

        void Delete(int id);
    }
}