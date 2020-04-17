﻿namespace MailingsGeneratorBll.Addition
{
    public class Helpful
    {
        public static void CheckDate(string date)
        {
            bool isCorrect;
            if (date == null)
            {
                isCorrect = false;
            }
            else
            {
                var parts = date.Split('.');
                int day;
                int month;
                isCorrect = parts.Length == 2 && int.TryParse(parts[0], out day) && int.TryParse(parts[1], out month)
                            && day <= 31 && day >= 1 && month <= 12 && month >= 1;
            }
            if (!isCorrect)
            {
                throw new ErrorTypes.IncorrectDate();
            }
        }

        public static void CheckId(int id)
        {
            if (id < 1)
            {
                throw new ErrorTypes.IncorrectId();
            }
        }
    }
}