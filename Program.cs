using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

using System.Text.Json;


namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        conMysql conn = new conMysql();
      
          static MqttClient ConnectMQTT(string broker, int port, string clientId)
        {
            MqttClient client = new MqttClient(broker, port, false, MqttSslProtocols.None, null, null);
            // client.Connect(clientId, username, password);
            client.Connect(clientId);
            if (client.IsConnected)
            {
                Console.WriteLine("Connected to MQTT Broker");
            }
            else
            {
                Console.WriteLine("Failed to connect");
            }
            return client;
        }

        // static void Publish(MqttClient client, string topic)
        // {
        //     int msg_count = 0;
        //     while (true)
        //     {
        //         System.Threading.Thread.Sleep(1*1000);
        //         string msg = "messages: " + msg_count.ToString();
        //         client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));
        //         Console.WriteLine("Send `{0}` to topic `{1}`", msg, topic);
        //         msg_count++;
        //     }
        // }

        static void Subscribe(MqttClient client, string topic)
        {
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // Format DATETIME at MySQL
            // DateTime time = DateTime.Now;  
            // string format = "yyyy-MM-dd HH:mm:ss";
            
            
            string payload = System.Text.Encoding.Default.GetString(e.Message);
    
                using JsonDocument doc = JsonDocument.Parse(payload);
                JsonElement root = doc.RootElement;
                    
                var temperature = root.GetProperty("temperature");
                var humidity = root.GetProperty("humidity");
                var vOut = root.GetProperty("vOut");
                var vIn = root.GetProperty("vIn");
                
            conMysql conn = new conMysql();
            conn.SaveTemperature(temperature.ToString(),humidity.ToString(),vOut.ToString(),vIn.ToString());
        }

        static void Main(string[] args)
        {
            
            // MQTT Configuration Setting
            string broker = "aessa.space";
            int port = 1883;
            string topic = "esp32/sensor";

            // string topicPublish = "cs/publish";
            string clientId = Guid.NewGuid().ToString();

            // string username = "emqx";
            // string password = "public";
            MqttClient client = ConnectMQTT(broker, port, clientId);
            Subscribe(client, topic);
            
            // Publish(client, topicPublish);
                   
        }
    }
}