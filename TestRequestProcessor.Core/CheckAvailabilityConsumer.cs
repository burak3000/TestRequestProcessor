using MassTransit;
using TestRequestProcessor.Core.Dtos;

namespace TestRequestProcessor.Core
{
    public class CheckAvailabilityConsumer : IConsumer<CheckAvailabilityRequestDto>
    {
        private readonly IAvailabilityChecker _availabilityChecker;
        Random _random;
        public CheckAvailabilityConsumer(IAvailabilityChecker availabilityChecker)
        {
            _random = new Random();
            _availabilityChecker = availabilityChecker;
        }
        public async Task Consume(ConsumeContext<CheckAvailabilityRequestDto> context)
        {
            AvailabilityStatus availabilityStatus = _availabilityChecker.CheckAvailability(new CheckAvailabilityParameters());
            await context.RespondAsync(new CheckAvailabilityResponseDto { IsAvailable = availabilityStatus == AvailabilityStatus.Available });
        }
    }
}
