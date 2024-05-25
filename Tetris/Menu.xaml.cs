using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TetrisLibrary;
using System.IO;

namespace Tetris
{
    public partial class Menu : Window
    {
        public Options settings = new Options();
        public User user = new User();
        public Menu()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnterTheUser.Visibility = Visibility.Hidden;
            User_Text.Visibility = Visibility.Hidden;
            User.Visibility = Visibility.Hidden;
            if(settings.Language == 0)
            {
                Play.Content = "Play";
                Settings.Content = "Settings";
                Exit.Content = "Exit";
                EnterTheUser.Content = "Enter the User:";
            }
            if(settings.Language == 1)
            {
                Play.Content = "Грати♥";
                Settings.Content = "Налаштування";
                Exit.Content = "Вихід";
                EnterTheUser.Content = "Введіть користувача:";
            }
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            Play.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Hidden;
            Exit.Visibility = Visibility.Hidden;
            EnterTheUser.Visibility= Visibility.Visible;
            User_Text.Visibility= Visibility.Visible;
            User.Visibility = Visibility.Visible;
        }
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settingss setings = new Settingss();
            setings.options = settings;
            setings.Show();
            this.Close();
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if(settings.Language == 0)
            {
                MessageBoxResult result = MessageBox.Show("Close the program?", "EXIT", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
            if (settings.Language == 1)
            {
                MessageBoxResult result = MessageBox.Show("Завершити програму?", "ВИХІД", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Close();
                }
            }
        }
        private void User_Click(object sender, RoutedEventArgs e)
        {
            user.Name = User_Text.Text;
            MainWindow main = MainWindow.Instance;
            main.option = settings;
            main.users = user;
            main.Show();
            this.Close();
        }
    }
}
