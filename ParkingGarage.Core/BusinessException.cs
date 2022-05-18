namespace ParkingGarage.Core;

public class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
        
    }
}