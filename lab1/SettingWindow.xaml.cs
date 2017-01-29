using System.Net;
using System.Windows;
using BLL.Setting;

namespace lab1
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        private string _login;

        public SettingWindow(string login)
        {
            _login = login;
            InitializeComponent();
            LoadSetting();
            Title = login + " введите настройки подключения";
        }

        private void ToChatWindowButtonClick(object sender, RoutedEventArgs e)
        {
            IPAddress ip;
            int remotePort;
            int localPort;

            var flag1 = IPAddress.TryParse(IpAdressTextBox.Text, out ip);
            var flag2 = int.TryParse(RemotePortTextBox.Text, out remotePort);
            var flag3 = int.TryParse(LocalPortIpAdress.Text, out localPort);

            if (!flag1)
            {
                MessageBox.Show("Ошибка ввода IP адреса сервера!");
                IpAdressTextBox.Text = string.Empty;
                return;
            }
            if (!flag2)
            {
                MessageBox.Show("Ошибка ввода локального порта!");
                LocalPortIpAdress.Text = string.Empty;
                return;
            }
            if (!flag3)
            {
                MessageBox.Show("Ошибка ввода внешнего порта!");
                RemotePortTextBox.Text = string.Empty;
                return;
            }

            Hide();
            var setting = new ConnectionSetting(ip,remotePort,localPort);
            var provider = new JsonSettingProvider {ConnectionSetting = setting};

            var mw = new ChatWindow(setting,_login);

            mw.Show();
            Close();
        }

        private void LoadSetting()
        {
            var provider = new JsonSettingProvider();
            var lastSetting = provider.ConnectionSetting;
            if (lastSetting == null) return;
            IpAdressTextBox.Text = lastSetting.ip.ToString();
            RemotePortTextBox.Text = lastSetting.remotePort.ToString();
            LocalPortIpAdress.Text = lastSetting.localPort.ToString();
        }
    }
}
