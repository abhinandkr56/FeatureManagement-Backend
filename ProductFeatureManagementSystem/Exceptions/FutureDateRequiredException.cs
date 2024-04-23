namespace ProductFeatureManagementSystem.Exceptions;

public class FutureDateRequiredException : Exception
{
    public FutureDateRequiredException()
    {
    }

    public FutureDateRequiredException(string message)
        : base(message)
    {
    }
}