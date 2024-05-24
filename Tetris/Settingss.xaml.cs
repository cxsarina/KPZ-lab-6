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
            MainWindow main = new MainWindow();
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
            InitializeComboBoxes();
            SetLanguageOptions();
            SetDifficultyOptions();
            SetGhostBlockOption();
        }

        private void InitializeComboBoxes()
        {
            ComboBox comboBox1 = (ComboBox)FindName("ComboBox1");
            ComboBox1.Items.Clear();
            ComboBox comboBox2 = (ComboBox)FindName("ComboBox2");
            ComboBox2.Items.Clear();
        }

        private void SetLanguageOptions()
        {
            switch (options.Language)
            {
                case 0:
                    SetLanguageToEnglish();
                    break;
                case 1:
                    SetLanguageToUkrainian();
                    break;
            }
        }

        private void SetLanguageToEnglish()
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

            ComboBox1.ItemsSource = new List<string> { "English", "Ukrainian" };
            ComboBox2.ItemsSource = new List<string> { "Easy", "Normal", "Hard" };
        }

        private void SetLanguageToUkrainian()
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

            ComboBox1.ItemsSource = new List<string> { "Англійська", "Українська" };
            ComboBox2.ItemsSource = new List<string> { "Легко", "Нормально", "Важко" };
        }

        private void SetDifficultyOptions()
        {
            ComboBox2.SelectedIndex = options.Difficult switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                _ => 0
            };
        }

        private void SetGhostBlockOption()
        {
            CheckBox1.IsChecked = options.Ghost_Block == 0;
        }
        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox1.SelectedIndex == 0) 
            {
                options.Language = 0;
            }
            else if(ComboBox1.SelectedIndex == 1)
            {
                options.Language = 1;
            }
        }
        private void ComboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox2.SelectedIndex == 0)
            {
                options.Difficult = 0;
            }
            else if(ComboBox2.SelectedIndex == 1)
            {
                options.Difficult = 1;
            }
            else if (ComboBox2.SelectedIndex == 2)
            {
                options.Difficult = 2;
            }
        }

        private void CheckBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            options.Ghost_Block = 1;
        }
    }
}
