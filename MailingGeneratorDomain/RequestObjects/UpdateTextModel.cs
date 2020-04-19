using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDomain.RequestObjects
{
    public class UpdateTextModel
    {
        public int     Id             { get; set; }
        public string  InfomationPart { get; set; }
        public Mailing Mail           { get; set; }
        public int?    MailingId      { get; set; }

        public bool IsEmpty()
        {
            return InfomationPart == null && !MailingId.HasValue;
        }
    }
}