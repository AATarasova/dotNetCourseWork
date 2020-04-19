using System.Collections.Generic;
using MailingGeneratorDomain.Models;
using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDomain.RequestObjects
{
    public class UpdateMailingModel : Mailing
    {
        public int? FinishId { get; set; }
        public bool IsEmpty()
        {
            return HelloText == null && StartDate == null && TextId == null && !FinishId.HasValue && WorksId == null;
        }
    }
}