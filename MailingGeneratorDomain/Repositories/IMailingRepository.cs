﻿ namespace MailingGeneratorDomain.Repositories
{
    public interface IMailingRepository
    {
        MailingsGeneratorDomain.Models.Mailing CreateMailing( MailingsGeneratorDomain.Models.Mailing mail);
        
        // Получение id по названию курса:
        MailingsGeneratorDomain.Models.Mailing GetCourse(int id);
        
        // Получение названия курса по id:
        MailingsGeneratorDomain.Models.Mailing GetCourse(string name);
        
        // Добавление работы в курс:
        void Update(MailingsGeneratorDomain.Models.Mailing mailing);
        
        // Удаление рассылки
        void DeleteMailing(MailingsGeneratorDomain.Models.Mailing id);
    }
    
}