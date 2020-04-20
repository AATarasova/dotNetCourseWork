﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 using MailingGeneratorDomain.Models;

 namespace MailingsGeneratorDomain.Models
{
    public class Mailing
    {

        [Required(ErrorMessage="Укажите дату начала курса")]
        public string StartDate { get; set; }
        [Required(ErrorMessage="Укажите название курса")]
        public string CourseName { get; set; }
        
        public List<ControlEvent> Works { get; set; } = new List<ControlEvent>();
        public ControlEvent FinishWork { get; set; }
        
        public string HelloText { get; set; }
        public int MailingId { get; set; }
        public List<Text> MailText { get; set; } = new List<Text>(); 

        
        [NotMapped]
        public List<int> WorksId { get; set; }
        
        [NotMapped]
        public List<int> TextId { get; set; }
        
        [NotMapped]
        public int FinishWorkId { get; set; }
    }
}