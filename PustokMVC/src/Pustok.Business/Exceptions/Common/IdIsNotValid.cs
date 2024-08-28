namespace Pustok.Business.Exceptions
{
    public class IdIsNotValid : Exception
    {
        public string PropertyName { get; set; }
        public IdIsNotValid()
        {
        }

        public IdIsNotValid(string? message) : base(message)
        {
        }

        public IdIsNotValid(string propertyName, string? message) : base(message)
        {
            PropertyName = propertyName;
        }

    }
}
