using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows;

namespace lab1
{
    /// <summary>
    /// Interaction logic for AppStartWindow.xaml
    /// </summary>
    public partial class AppStartWindow
    {
        public AppStartWindow()
        {
            InitializeComponent();
        }

        private void ToSettingWindowButtonClick(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text + "  ";

            if(string.IsNullOrWhiteSpace(login) || string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Недопустимый логин!");
                return;
            }

            var settingWindow = new SettingWindow(login);
            settingWindow.Show();
            Close();
        }
    }
}
