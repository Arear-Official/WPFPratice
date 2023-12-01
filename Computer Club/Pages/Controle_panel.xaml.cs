using Computer_Club.ModelSQL;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Computer_Club.Pages
{
    /// <summary>
    /// Логика взаимодействия для Controle_panel.xaml
    /// </summary>
    public partial class Controle_panel : Page
    {
        public Controle_panel()
        {
            InitializeComponent();
        }

        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            Fill();
            Keyboard.Visibility = Visibility.Hidden;
        }

        private void Button_Click()
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Fill();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Keyboard.Visibility == Visibility.Visible)
            {
                Keyboard.Visibility = Visibility.Hidden;
                return;
            }
            Keyboard.Visibility = Visibility.Visible;
        }

        private void SearchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data.Items.Clear();
            foreach (Computers computers in Computers.SetComputers(new ConnectBase().connectionString, SearchBox.Text, SearchText.Text))
            {
                Data.Items.Add(computers);
            }
        }

        private void SearchBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchText.Clear();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
        }
        public void Fill()
        {
            Data.Items.Clear();
            foreach (Computers clients in Computers.SetComputers(new ConnectBase().connectionString))
            {
                Data.Items.Add(clients);
            }
        }
    }
}
