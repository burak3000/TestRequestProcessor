namespace TestRequestProcessor.Core
{
    public interface IAvailabilityChecker
    {
        AvailabilityStatus CheckAvailability(CheckAvailabilityParameters availabilityParameters);
    }
}
