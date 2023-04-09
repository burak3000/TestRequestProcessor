using MassTransit;

namespace TestRequestProcessor.Core
{
    public class TestRequestState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }
        public Guid? AvailabilityRequestId { get; set; }
        public bool IsAvailable { get; set; }
    }
}