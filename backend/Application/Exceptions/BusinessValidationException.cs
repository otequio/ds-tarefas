namespace Application.Exceptions
{
    public class BusinessValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public BusinessValidationException(IDictionary<string, string[]> errors)
            : base("Ocorreram erros de validação.")
        {
            Errors = errors;
        }

        public BusinessValidationException(string identificacao, string message)
            : base(message)
        {
            if (Errors == null)
                Errors = new Dictionary<string, string[]>();

            Errors.Add(identificacao, [message]);
        }
    }
}
