using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestionBackend.Services
{
    public class KafkaProducerService
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducerService()
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "kafka:9092"
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        // Método genérico para cualquier tipo de evento
        public async Task PublishAsync<T>(string topic, T message)
        {
            try
            {
                var json = JsonSerializer.Serialize(message);
                var result = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = json });
                Console.WriteLine($"✅ Mensaje publicado en {topic}: offset {result.Offset}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error enviando mensaje a Kafka: {ex.Message}");
            }
        }
    }
}
