namespace Vinstag.InstagramAPI.Exception;

public class NoSessionException : System.Exception
{
    public NoSessionException()
    {
    }

    public NoSessionException(string message) : base(message)
    {
    }
}