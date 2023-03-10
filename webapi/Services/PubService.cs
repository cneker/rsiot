using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using webapi.Contracts.Services;

namespace webapi.Services
{
    public class PubService : IPubService
    {
        private PublisherClient _publisher;
        private readonly TopicName _topic;
        public PubService(IConfiguration configuration)
        {
            var pubSubSection = configuration.GetSection("PubSub");
            var projectId = pubSubSection.GetSection("ProjectId").Value;
            var topicId = pubSubSection.GetSection("TopicId").Value;
            _topic = new TopicName(projectId, topicId);
        }
        public async Task Publish(string data)
        {
            _publisher = PublisherClient.Create(_topic);
            var message = new PubsubMessage()
            {
                Data = ByteString.CopyFromUtf8(data)
            };

            await _publisher.PublishAsync(message);
            await _publisher.ShutdownAsync(TimeSpan.FromSeconds(15));
        }
    }
}
