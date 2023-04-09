using MassTransit;
using TestRequestProcessor.Core.Dtos;

namespace TestRequestProcessor.Core
{
    public class TestRequestStateMachine : MassTransitStateMachine<TestRequestState>
    {
        public Event<TestRequestDto> TestRequestedEvent { get; private set; }
        public Request<TestRequestState, CheckAvailabilityRequestDto, CheckAvailabilityResponseDto> CheckAvailabilityRequest { get; private set; }
        public TestRequestStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => TestRequestedEvent, x => x.CorrelateById(ctx => ctx.Message.TestRequestId));

            Request(
                () => CheckAvailabilityRequest,
                x => x.AvailabilityRequestId);

            Initially(
                When(TestRequestedEvent)
                .Request(CheckAvailabilityRequest, x =>
                x.Init<CheckAvailabilityRequestDto>(new CheckAvailabilityRequestDto { CheckAvailabilityRequestId = x.Saga.CorrelationId }))
                .TransitionTo(Processing));

            During(Processing,
                When(CheckAvailabilityRequest.Completed)
                .Then(ctx =>
                Console.WriteLine("Availability status is: " + ctx.Message.IsAvailable))
                .TransitionTo(Processed));
        }

        public State Processing { get; private set; }

        public State Processed { get; private set; }
    }
}
