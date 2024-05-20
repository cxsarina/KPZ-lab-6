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

namespace Tetris
{
    public partial class Settingss : Window
    {
        public Options options = new Options();
        public Settingss()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            MainWindow main = MainWindow.Instance;
            HighScoreTablee highScoreTablee = new HighScoreTablee();
            main.option = options;
            menu.settings = options;
            highScoreTablee.optionss = options;
            menu.Show();
            this.Close();
        }
        private void Default_Click(object sender, RoutedEventArgs e)
        {
            options.Theme = 0;
        }
        private void Flowers_Click(object sender, RoutedEventArgs e)
        {
            options.Theme = 1;
        }

        private void CheckBox1_Checked(object sender, RoutedEventArgs e)
        {
            options.Ghost_Block = 0;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox1 = (ComboBox)FindName("ComboBox1");
            ComboBox1.Items.Clear();
            ComboBox comboBox2 = (ComboBox)FindName("ComboBox2");
            ComboBox2.Items.Clear();
            if (options.Language == 0)
            {
                ComboBox1.SelectedIndex = 0;
                label1.Content = "Settings:";
                label2.Content = "Program Style:";
                CheckBox1.Content = "With Ghost-Block";
                label4.Content = "Language:";
                label5.Content = "Difficulty:";
                Save.Content = "Save";
                Default.Content = "Default";
                Flowers.Content = "Flowers";
                List<string> newItems = new List<string>
                {
                    "English",
                    "Ukrainian"
                };
                comboBox1.ItemsSource = newItems;
                List<string> newItems1 = new List<string>
                {
                    "Easy",
                    "Normal",
                    "Hard"
                };
                comboBox2.ItemsSource = newItems1;
            }
            if (options.Language == 1)
            {
                ComboBox1.SelectedIndex = 1;
                label1.Content = "Налаштування:";
                label2.Content = "Тема програми:";
                CheckBox1.Content = "Блок-Підказка";
                label4.Content = "Мова:";
                label5.Content = "Складність:";
                Save.Content = "Зберегти";
                Default.Content = "Базова";
                Flowers.Content = "Квіткова";
                List<string> newItems = new List<string>
                {
                    "Англійська",
                    "Українська"
                };
                comboBox1.ItemsSource = newItems;
                List<string> newItems1 = new List<string>
                {
                    "Легко",
                    "Нормально",
                    "Важко"
                };
                comboBox2.ItemsSource = newItems1;
            }
            if(options.Difficult == 0)
            {
                ComboBox2.SelectedIndex = 0;
            }
            if(options.Difficult == 1)
            {
                ComboBox2.SelectedIndex = 1;
            }
            if(options.Difficult == 2)
            {
                ComboBox2.SelectedIndex = 2;
            }
            if(options.Ghost_Block == 0)
            {
                CheckBox1.IsChecked = true;
            }
            if (options.Ghost_Block == 1)
            {
                CheckBox1.IsChecked = false;
            }
        }
        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            options.Language = ComboBox1.SelectedIndex;
        }
        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            options.Difficult = ComboBox2.SelectedIndex;
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            options.Ghost_Block = 1;
        }
    }
}
