namespace MailingGeneratorDomain.RequestObjects
{
    public class UpdateControlEventObject
    {
        public int    Id      { get; set; }
        public int?   MaxMark { get; set; }
        public string Date    { get; set; }

        public bool IsEmpty()
        {
            return !MaxMark.HasValue && Date == null;
        }
    }
}