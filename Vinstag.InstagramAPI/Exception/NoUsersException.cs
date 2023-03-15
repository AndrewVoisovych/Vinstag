namespace Vinstag.InstagramAPI.Exception;

public class NoUsersException : System.Exception
{
    public NoUsersException()
    {
    }

    public NoUsersException(string message) : base(message)
    {
    }
}