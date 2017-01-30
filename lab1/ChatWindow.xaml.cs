using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using BLL.Network;
using BLL.Setting;
using Newtonsoft.Json;

namespace lab1
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly string _login;
        private bool _isFirstMessage = true;
        private readonly ConnectionSetting _setting;

        public ObservableCollection<Message> Messages { get; set; }

        public ChatWindow(ConnectionSetting inputConnectionSetting, string login)
        {
            DataContext = this;
            _login = login;
            _setting = inputConnectionSetting;

            var helloMessage = new Message
            {
                UserLogin = _login,
                SendTime = DateTime.Now.ToString(),
                MessageText = _login + " присоединился(лась) к чату!"
            };

            Messages = new ObservableCollection<Message> {helloMessage};
            Send(helloMessage);


            Thread thread = new Thread(new ThreadStart(Receiver));
            Dispatcher.Invoke(new Action(() =>
            {
                thread.Start();
            }));

            InitializeComponent();
        }


        #region NetworkMethods

        private void Scroll()
        {
            var lastIndex = ChatListBox.Items.Count - 1;
            ChatListBox.ScrollIntoView(ChatListBox.Items[lastIndex]);
        }

        public void Send(Message message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ipEndPoint = new IPEndPoint(_setting.ip, _setting.remotePort);

            try
            {
                var JsonMessage = JsonConvert.SerializeObject(message);
                byte[] rowData = Encoding.UTF8.GetBytes(JsonMessage);
                client.Send(rowData, rowData.Length, ipEndPoint);
            }
            catch (Exception e)
            {
                Messages.Add(new Message
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
            var client = new UdpClient(_setting.localPort);
            IPEndPoint RemoteIpEndPoint = null; 
            try
            {

                while (true)
                {
                    Message resultMessage = new Message();
                    byte[] inputRowByte = client.Receive(ref RemoteIpEndPoint);
                    var MessageToString = Encoding.UTF8.GetString(inputRowByte);
                    try
                    {
                        resultMessage = JsonConvert.DeserializeObject<Message>(MessageToString);
                    }
                    catch (JsonSerializationException)
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
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => Messages.Add(resultMessage)));
                        //Messages.Add(resultMessage);
                        Scroll();

                    }
                }
            }

            catch (Exception e)
            {
                var errorMessage = new Message
                {
                    SendTime = DateTime.Now.ToString(),
                    UserLogin = "system",
                    MessageText = e.Message
                };
                Application.Current.Dispatcher.BeginInvoke(new Action(() => Messages.Add(errorMessage)));
                //Messages.Add(errorMessage);
            }
            finally
            {
                client.Close();
            }

        }

        #endregion



            #region WPF methods

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var messageText = MessageInputTextBox.Text;

            if (string.IsNullOrEmpty(messageText) || string.IsNullOrWhiteSpace(messageText))
            {
                MessageBox.Show("Пустое сообщение");
                return;
            }

            if (messageText.Length > 255)
            {
                MessageBox.Show("Слишком длинное сообщение!");
                return;
            }

            var message = new Message
            {
                UserLogin=_login,
                SendTime = DateTime.Now.ToString(),
                MessageText = messageText
            };
            Messages.Add(message);
            Send(message);

        }


        private void MessageInputTextBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (_isFirstMessage)
            {
                MessageInputTextBox.Text = string.Empty;
                _isFirstMessage = false;
            }
         
        }


        private void ClearMessages_OnClick(object sender, RoutedEventArgs e)
        {
            Messages.Clear();
        }
        #endregion

        private void ChatWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }
    }


}
