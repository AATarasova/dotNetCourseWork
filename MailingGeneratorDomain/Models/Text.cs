using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDomain.Models
{
    public class Text
    {
        public string RememberAboutDeadline { get; set; }
        [Required(ErrorMessage="В тексте рассылки должно быть описание модуля!")]
        public string InfomationPart { get; set; }
        public int    TextId { get; set; }
        [NotMapped]
        public int    MailId { get; set; }
        public Mailing Mail { get; set; }
    }
}