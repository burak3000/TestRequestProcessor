using MassTransit;
using TestRequestProcessor.Core.Dtos;

namespace TestRequestProcessor.Core
{
    public class CheckAvailabilityConsumer : IConsumer<CheckAvailabilityRequestDto>
    {
        Random _random;
        public CheckAvailabilityConsumer()
        {
            _random = new Random();

        }
        public async Task Consume(ConsumeContext<CheckAvailabilityRequestDto> context)
        {
            bool isAvailable = _random.Next(0, 2) == 1;
            await context.RespondAsync(new CheckAvailabilityResponseDto { IsAvailable = isAvailable });
        }
    }
}
