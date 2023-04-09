namespace TestRequestProcessor.Core
{
    public class TestAvailabilityChecker : IAvailabilityChecker
    {
        Random _random;

        public TestAvailabilityChecker()
        {
            _random = new Random();
        }
        public AvailabilityStatus CheckAvailability(CheckAvailabilityParameters availabilityParameters)
        {

            return _random.Next(0, 2) == 1 ? AvailabilityStatus.Available : AvailabilityStatus.NotAvailable;
        }
    }

    public enum AvailabilityStatus
    {
        NotDetermined,
        Available,
        NotAvailable
    }
}
