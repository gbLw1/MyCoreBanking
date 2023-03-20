namespace MyCoreBanking;

public class NotFoundException : Exception
{
    public NotFoundException(string message, string paramName) : base(message)
    {
        ParamName = paramName;
    }

    public NotFoundException(string paramName) : base($"'{paramName}' não encontrado.")
    {
        ParamName = paramName;
    }

    public string ParamName { get; }
}
