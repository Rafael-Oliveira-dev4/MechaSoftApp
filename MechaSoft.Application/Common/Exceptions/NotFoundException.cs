namespace MechaSoft.Application.Common.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string msg) : base(msg)
    {
    }
}
