namespace Pustok.Business.Exceptions
{
    public class AuthorFullNameException : Exception
    {
        public string PropertyName { get; set; }
        public AuthorFullNameException()
        {
        }

        public AuthorFullNameException(string? message) : base(message)
        {
        }

        public AuthorFullNameException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
