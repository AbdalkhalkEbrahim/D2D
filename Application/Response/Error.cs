namespace Application.Response
{
    public record Error(string StatusCode, string Message)
    {
        public static readonly Error None = new("None", string.Empty);

       
    }
}
