using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class HighScoreTablee : Window
    {
        public Options optionss;
        public User userss;
        public List <User> userlist;
        public HighScoreTablee()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(optionss.Language == 0)
            {
                label1.Content = "Score Table";
                label2.Content = "User                                                        Score Easy                                               Score Normal                                                 Score Hard";
                ReturnnToMenu.Content = "Return To Menu";
            }
            if(optionss.Language == 1)
            {
                label1.Content = "Таблиця Рекордів";
                label2.Content = "Користувач                                             Рахунок (Легкий)                                    Рахунок (Нормальний)                            Рахунок (Важкий)";
                ReturnnToMenu.Content = "Повернення до меню";

            }
            foreach(User user in userlist)
            {
                listBox1.Items.Add(user);
            }
        }
        private void ReturnnToMenu_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.settings = optionss;
            menu.Show();
            this.Close();
        }
    }
}
