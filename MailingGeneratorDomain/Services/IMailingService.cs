﻿using System;
 using System.Collections.Generic;
 using MailingGeneratorDomain.Models;
 using MailingGeneratorDomain.RequestObjects;
 using MailingsGeneratorDomain.Models;

namespace MailingsGeneratorDomain.Services
{
    public interface IMailingService
    {
        MailingsGeneratorDomain.Models.Mailing CreateMailing(MailingsGeneratorDomain.Models.Mailing mail);
        
        // Получение id по названию курса:
        string GetCourseName(int id);
        
        // Получение названия курса по id:
        int GetCourseId(string name);        
        // Получение id по названию курса:
        Mailing GetCourse(int id);
        
        // Получение названия курса по id:
        Mailing GetCourse(string name);
        
        // Добавление работы в курс:
        void AddWork(int courseId, int workId, bool isFinish);
        
        // Получения списка работ в курсе:
        List<ControlEvent> GetControlEvents(int id);
        
        // Получение итоговой работы курса:
        ControlEvent GetFinishEvent(int id);
        
        // Удаление рассылки
        void DeleteMailing(int id);

        Mailing GetCourse(GetMailingObject getMailingObject);
    }
}