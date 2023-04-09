using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestRequestProcessor.Core.Dtos;

namespace TestRequestProcessor.Core.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Test1()
        {
            Mock<IAvailabilityChecker> availabilityCheckerMock = new Mock<IAvailabilityChecker>();
            availabilityCheckerMock
                .Setup(a => a.CheckAvailability(It.IsAny<CheckAvailabilityParameters>()))
                .Returns(AvailabilityStatus.Available);

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<CheckAvailabilityConsumer>();
                cfg.AddSagaStateMachine<TestRequestStateMachine, TestRequestState>();
            });
            serviceCollection.AddScoped<IAvailabilityChecker>(ac => availabilityCheckerMock.Object);

            await using var provider = serviceCollection.BuildServiceProvider(true);

            var harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();
            var sagaId = Guid.NewGuid();
            await harness.Bus.Publish(new TestRequestDto
            {
                TestRequestId = sagaId
            });
            var sagaHarness = harness.GetSagaStateMachineHarness<TestRequestStateMachine, TestRequestState>();
            Assert.That(await sagaHarness.Consumed.Any<TestRequestDto>());
            Assert.That(await harness.Consumed.Any<CheckAvailabilityRequestDto>());
            Assert.That(await harness.Consumed.Any<CheckAvailabilityResponseDto>());

            Assert.That(await sagaHarness.Created.Any(x => x.CorrelationId == sagaId));
            var instance = sagaHarness.Created.ContainsInState(sagaId, sagaHarness.StateMachine, sagaHarness.StateMachine.Processed);
            Assert.IsNotNull(instance, "Saga instance not found");

            availabilityCheckerMock.Verify(ac => ac.CheckAvailability(It.IsAny<CheckAvailabilityParameters>()), Times.Once);
            Assert.That(instance.IsAvailable, Is.True);
        }
    }
}