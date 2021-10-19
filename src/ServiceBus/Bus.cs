using Abstractions;
using NetMQ;
using NetMQ.Sockets;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class Bus : IBus
    {
        private static readonly BlockingCollection<Message> sendQueue = new BlockingCollection<Message>();

        private static readonly Dictionary<int, PushSocket> pushShockets = new Dictionary<int, PushSocket>();

        public Bus()
        {
            Task.Factory.StartNew(SendMessage, TaskCreationOptions.LongRunning);
        }

        public void Publish<T>(T @event) where T : IEvent
        {
            string content = JsonSerializer.Serialize(@event);

            Message message = new Message { Content = content, Port = @event.Port };

            sendQueue.Add(message);
        }

        private static void SendMessage()
        {
            foreach (Message message in sendQueue.GetConsumingEnumerable())
            {
                PushSocket pushSocket = pushShockets.ContainsKey(message.Port) ? pushShockets[message.Port] : Create(message.Port);

                string jsonMessage = JsonSerializer.Serialize(message);

                pushSocket.SendFrame(jsonMessage);
            }
        }

        private static PushSocket Create(int port)
        {
            PushSocket pushSocket = new($"@tcp://*:{port}");

            pushShockets.Add(port, pushSocket);

            return pushSocket;
        }
    }
}
