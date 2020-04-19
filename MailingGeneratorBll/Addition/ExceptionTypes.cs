using System;

namespace MailingGeneratorBll.Addition
{
    public class ExceptionTypes
    {
        public class NotExistException : Exception
        {
            protected NotExistException(string stackTrace) : base(stackTrace) { }
        }
        public class MailingNotExistException : NotExistException
        {
            public MailingNotExistException() : base("Не существует рассылки с указанным id курса!") { }
        }
        public class ControlEventNotExistException : NotExistException 
        {
            public ControlEventNotExistException() : base("Не существует контрольного мероприятия с указанным id курса!") { }
        }
        
        public class TextNotExistException : NotExistException 
        {
            public TextNotExistException() : base("Не существует текста с указанным id курса!") { }
        }

        public class AlreadyContainsException : Exception 
        {
            public AlreadyContainsException() : base("Итоговая контрольная уже есть в курсе или тестовая уже добавлена в него.") { }
        }

        public class IncorrectIdException : Exception         
        {
            public IncorrectIdException() : base("id должен быть не меньше нуля!") { }
        }
        public class IdNotFoundException : Exception
        {
            public IdNotFoundException() : base("По такому идентификатору ничего не найдено.") { }
        }
        public class IncorrectNameException : Exception
        {
            public IncorrectNameException() : base("Имя не должно быть пустым!") { }
        }
        
        public class IncorrectMarkException : Exception 
        {
            public IncorrectMarkException() : base("Оценка не должна быть отрицательной!") { }
        }
        public class NullValueException : Exception
        {
            public NullValueException() : base("Пустой запрос!") { }
        }

        public class IncorrectDateException : Exception
        {
            public IncorrectDateException() : base("Ожидается получить дату в формате dd.mm") { }
        }
    }
}