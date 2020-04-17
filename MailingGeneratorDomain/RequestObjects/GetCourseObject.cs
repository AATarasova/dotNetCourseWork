using System;

namespace MailingGeneratorDomain.RequestObjects
{
    public class GetMailingObject
    {
        public string Name { get; set; }
        public int?   Id   { get; set; }

        public enum TypeOfGetParameter
        {
            Name,
            Id, 
            NoParameter
        }

        public TypeOfGetParameter TypeOfRequest()
        {
            if (Name == null && !Id.HasValue)
            {
                return TypeOfGetParameter.NoParameter;
            }

            if (Name != null)
            {
                return TypeOfGetParameter.Name;
            }

            return TypeOfGetParameter.Id;
        }
    }
}