using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using BLL.Setting;
using Newtonsoft.Json;


namespace BLL.Network
{
    public class NetworkWorker
    {
        private readonly IPAddress _remoteIpAddress;
        private readonly int _remotePort;
        private readonly int _localPort;
        private ObservableCollection<Message> chat;
        private bool isListening;

        public NetworkWorker(ConnectionSetting setting, ObservableCollection<Message> chat)
        {
            _remoteIpAddress = setting.ip;
            _remotePort = setting.remotePort;
            _localPort = setting.localPort;
            this.chat = chat;

            Thread listener = new Thread(new ThreadStart(Receiver));
            listener.Start();
        }

        public void StopListener()
        {
            isListening = false;
        }

        public void StartListening()
        {
            isListening = true;
        }

        public void Send(Message message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(_remoteIpAddress, _remotePort);

            try
            {
                var JsonMessage = JsonConvert.SerializeObject(message);
                byte[] rowData = Encoding.UTF8.GetBytes(JsonMessage);
                client.Send(rowData, rowData.Length, ipEndPoint);
            }
            catch (Exception e)
            {
                chat.Add(new Message
                {
                    UserLogin = "system",
                    SendTime = DateTime.Now.ToString(),
                    MessageText = e.ToString()
                });
            }
            finally
            {
                client.Close();
            }
        }
        


        private void Receiver()
        {
            var udp = new UdpClient(_localPort);

            try
            {

                while (isListening)
                {
                    IPEndPoint ipEndPoint = null;
                    byte[] inputRowByte = udp.Receive(ref ipEndPoint);
                    var MessageToString = Encoding.Default.GetString(inputRowByte);
                    Message resultMessage = JsonConvert.DeserializeObject<Message>(MessageToString);
                    chat.Add(resultMessage);
                }
            }
            catch (Exception e)
            {
                var errorMessage = new Message
                {
                    SendTime = DateTime.Now.ToString(),
                    UserLogin = "system",
                    MessageText = e.ToString()
                };

                chat.Add(errorMessage);
            }
            finally
            {
                udp.Close();
            }
        }
    }
}