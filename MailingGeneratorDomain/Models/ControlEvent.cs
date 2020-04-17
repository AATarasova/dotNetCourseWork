using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MailingsGeneratorDomain.Models;

namespace MailingGeneratorDomain.Models
{
    public class ControlEvent
    {
        public int    ControlEventId { get; set; }
        [Range(0, 10, ErrorMessage = "Балл должен быть в промежутке от 0 до 10")]
        [Required(ErrorMessage="Укажите максимальный балл")]
        public int    MaxMark { get; set; }
        [Required(ErrorMessage="Укажите дату проведения")]
        public string Date { get; set; }
        [Required(ErrorMessage="Укажите название работы")]
        public string Name { get; set; }
        [NotMapped]
        public int    MailId { get; set; }
        public        Mailing Mail { get; set; }
    }
}