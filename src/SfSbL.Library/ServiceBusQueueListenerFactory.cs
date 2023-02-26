namespace SfSbL.Library
{
    using System;
    using System.Threading.Tasks;
    using Azure.Messaging.ServiceBus;
    using JetBrains.Annotations;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;

    public class ServiceBusQueueListenerFactory
    {
        private readonly ServiceBusClient _serviceBusClient;

        public ServiceBusQueueListenerFactory(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="errorHandler"></param>
        /// <param name="queueName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [UsedImplicitly]
        public ICommunicationListener Make(
            Func<ProcessMessageEventArgs, Task> handler,
            Func<ProcessErrorEventArgs, Task> errorHandler,
            string queueName,
            ServiceBusProcessorOptions? options = null)
        {
            var processor = _serviceBusClient.CreateProcessor(
                queueName,
                options);

            processor.ProcessMessageAsync += handler;
            processor.ProcessErrorAsync += errorHandler;

            return new ServiceBusQueueListener(processor);
        }
    }
}