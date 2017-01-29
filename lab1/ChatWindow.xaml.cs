using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        private NetworkWorker network;
        private readonly string _login;
        private bool _isFirstMessage = true;
        public ObservableCollection<Message> Messages { get; set; }

        public ChatWindow(ConnectionSetting inputConnectionSetting, string login)
        {
            DataContext = this;

            _login = login;
            network = new NetworkWorker(inputConnectionSetting,Messages);
            Messages = new ObservableCollection<Message> {new Message
            {
                UserLogin = _login,
                SendTime = DateTime.Now.ToString(),
                MessageText = _login + " присоединился(лась) к чату!"
            }};

            InitializeComponent();
        }

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
            network.Send(message);

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
    }

    
}
