using System;
using System.Collections.Generic;
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
        public List<Message> InputMessages { get; set; } = new List<Message>();

        private readonly IPAddress _remoteIpAddress;
        private readonly int _remotePort;
        private readonly int _localPort;

        public NetworkWorker(ConnectionSetting setting)
        {
            _remoteIpAddress = setting.ip;
            _remotePort = setting.remotePort;
            _localPort = setting.localPort;

            Thread listener = new Thread(new ThreadStart(Receiver));
            listener.Start();
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
                InputMessages.Add(new Message
                {
                    UserLogin = "system",
                    SendTime = DateTime.Now.ToString(),
                    MessageText = e.Message
                });
            }
            finally
            {
                client.Close();
            }
        }
        


        private void Receiver()
        {
            var client = new UdpClient(_localPort);
            IPEndPoint RemoteIpEndPoint = null;

            try
            {

                while (true)
                {
                    Message resultMessage = new Message();
                    byte[] inputRowByte = client.Receive(ref RemoteIpEndPoint);
                    var MessageToString = Encoding.Default.GetString(inputRowByte);
                    try
                    {
                        resultMessage = JsonConvert.DeserializeObject<Message>(MessageToString);
                    }
                    catch (JsonSerializationException e)
                    {
                        resultMessage = new Message
                        {
                            MessageText = MessageToString,
                            SendTime = DateTime.MinValue.ToString(),
                            UserLogin = "Unknow client user"
                        };
                    }
                    finally
                    {
                        InputMessages.Add(resultMessage);
                    }
                }
            }
            
            catch (Exception e)
            {
                var errorMessage = new Message
                {
                    SendTime = DateTime.Now.ToString(),
                    UserLogin = "system",
                    MessageText = ""
                };

                InputMessages.Add(errorMessage);
            }
            finally
            {
                client.Close();
            }
        }
    }
}