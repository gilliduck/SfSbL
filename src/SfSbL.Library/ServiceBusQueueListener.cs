namespace SfSbL.Library
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using JetBrains.Annotations;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;

    public class ServiceBusQueueListener : ICommunicationListener
    {
        private readonly ServiceBusProcessor _serviceBusProcessor = null!;

        internal ServiceBusQueueListener(ServiceBusProcessor serviceBusProcessor)
        {
            _serviceBusProcessor = serviceBusProcessor;
        }

        [Obsolete("Private/Obsolete to prevent usage", true)]
        [UsedImplicitly]
        private ServiceBusQueueListener()
        {
        }

        public void Abort()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            CloseAsync(cts.Token).GetAwaiter().GetResult();
        }

        public async Task CloseAsync(CancellationToken cancellationToken)
        {
            await _serviceBusProcessor.StopProcessingAsync(cancellationToken);
            await _serviceBusProcessor.CloseAsync(cancellationToken);
        }

        public async Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            await _serviceBusProcessor.StartProcessingAsync(cancellationToken);
            return _serviceBusProcessor.Identifier;
        }
    }
}