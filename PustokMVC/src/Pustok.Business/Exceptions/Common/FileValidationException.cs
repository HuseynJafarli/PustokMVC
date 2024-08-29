namespace Pustok.Business.Exceptions
{
    public class FileValidationException: Exception
    {
        public string PropertyName { get; set; }
        public FileValidationException()
        {
        }

        public FileValidationException(string? message) : base(message)
        {
        }

        public FileValidationException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
