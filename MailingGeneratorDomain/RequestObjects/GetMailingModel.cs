using System;

namespace MailingGeneratorDomain.RequestObjects
{
    public class GetMailingModel
    {
        public string Name { get; set; }
        public int?   Id   { get; set; }

        public bool IsEmpty()
        {
            return Name == null && !Id.HasValue;
        }
    }
}