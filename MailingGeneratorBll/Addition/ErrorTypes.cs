﻿using System;
 using Microsoft.CodeAnalysis.CSharp.Syntax;

 namespace MailingsGeneratorBll.Addition
{
    public class ErrorTypes
    {
        public class NotExist : Exception
        {
            protected NotExist(string stackTrace) : base(stackTrace) { }
        }
        public class MailingNotExist : NotExist
        {
            public MailingNotExist() : base("Не существует рассылки с указанным id курса!") { }
        }
        public class ControlEventNotExist : NotExist 
        {
            public ControlEventNotExist() : base("Не существует контрольного мероприятия с указанным id курса!") { }
        }
        
        public class TextNotExist : NotExist 
        {
            public TextNotExist() : base("Не существует текста с указанным id курса!") { }
        }

        public class AlreadyContains : Exception 
        {
            public AlreadyContains() : base("Итоговая контрольная уже есть в курсе или тестовая уже добавлена в него.") { }
        }

        public class IncorrectId : Exception         
        {
            public IncorrectId() : base("id должен быть не меньше нуля!") { }
        }
        public class IdNotFound : Exception
        {
            public IdNotFound() : base("По такому идентификатору ничего не найдено.") { }
        }
        public class IncorrectName : Exception
        {
            public IncorrectName() : base("Имя не должно быть пустым!") { }
        }
        
        public class IncorrectMark : Exception 
        {
            public IncorrectMark() : base("Оценка не должна быть отрицательной!") { }
        }
        public class NullValue : Exception
        {
            public NullValue() : base("Пустой запрос!") { }
        }

        public class IncorrectDate : Exception
        {
            public IncorrectDate() : base("Ожидается получить дату в формате dd.mm") { }
        }
    }
}