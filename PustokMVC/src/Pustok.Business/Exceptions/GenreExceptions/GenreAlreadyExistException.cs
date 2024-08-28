namespace Pustok.Business.Exceptions
{
    public class GenreAlreadyExistException : Exception
    {
        public string PropertyName { get; set; }
        public GenreAlreadyExistException()
        {
        }

        public GenreAlreadyExistException(string? message) : base(message)
        {
        }

        public GenreAlreadyExistException(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }

    }
}
