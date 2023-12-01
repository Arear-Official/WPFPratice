using Computer_Club.ModelSQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Computer_Club.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsSQL.xaml
    /// </summary>
    
    public partial class Clients : Page
    {
        bool search = false;
        public Clients()
        {
            InitializeComponent();
            Adding.Visibility = Visibility.Hidden;
        }

        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            Fill();
        }

        public void Fill()
        {
            Data.Items.Clear();
            foreach (ClientsSQL clients in ClientsSQL.SetClients(new ConnectBase().connectionString))
            {
                Data.Items.Add(clients);
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Adding.Visibility = Visibility.Visible;
            Header.Text = "Добавление клиента";
            btnLogin.Content = "Добавить";
            txtNumber.Text = "";
            txtUser.Text = "";
            txtEMail.Text = "";
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Adding.Visibility = Visibility.Hidden;
        }

        private void Adding_Click(object sender, RoutedEventArgs e)
        {
            
            string message = ""; 
            
            if (!IsPhone(txtNumber.Text, out message))
            {
                txtError.Text = "Не правильный формат телефона";
                return;
            }
            txtNumber.Text = message;
            if (!IsEmail(txtEMail.Text))
            {
                txtError.Text = "Не правильный формат Почты";
                return;
            }
            
            txtError.Text = "";            
            if (Header.Text == "Изменение клиента")
            {
                string id = "";
                ClientsSQL selectedItem = (ClientsSQL)Data.SelectedItem;
                if (selectedItem != null)
                {
                    id = selectedItem.ID;
                }
                ClientsSQL.UpdateClient(new ConnectBase().connectionString, txtUser.Text, txtNumber.Text, txtEMail.Text, id);
                Fill();
                Adding.Visibility = Visibility.Hidden;
                return;                
            }
            if (ClientsSQL.IsRepeat(new ConnectBase().connectionString, txtUser.Text))
            {
                txtError.Text = "Такой пользователь уже есть";
                return;
            }
            ClientsSQL.AddClient(new ConnectBase().connectionString, txtUser.Text, txtNumber.Text, txtEMail.Text);
            Fill();
            Adding.Visibility = Visibility.Hidden;
        }

        public static bool IsEmail(string email)
        {
            // Паттерн для проверки email
            string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            // Проверка совпадения паттерна
            Match match = Regex.Match(email, pattern);
            if (match.Success)
            {
                return true;
            }
                return false;
        }
        public static bool IsPhone(string Number, out string OutNumber)
        {            
            // Удаление всех символов, кроме цифр
            string cleanedNumber = Regex.Replace(Number, @"\D", "");
            OutNumber = cleanedNumber;
            OutNumber = OutNumber.Insert(0, "+");
            OutNumber = OutNumber.Insert(OutNumber.Length - 9, "-");
            OutNumber = OutNumber.Insert(OutNumber.Length - 7, "-");
            OutNumber = OutNumber.Insert(OutNumber.Length - 4, "-");
            OutNumber = OutNumber.Insert(OutNumber.Length - 2, "-");
            // Проверка формата номера
            if (!Regex.IsMatch(cleanedNumber, @"^\d{10,12}$"))
            {
                OutNumber = Number;
                return false;
            }

            return true;
        }

        private void DeleteRow(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Red.png");
            string id = "";          
            ClientsSQL selectedItem = (ClientsSQL)Data.SelectedItem;
            if (selectedItem != null)
            {
                id = selectedItem.ID;
            }
            MessageBoxResult result = System.Windows.MessageBox.Show($"Вы уверены, что хотите удалить клиента №{id}?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ClientsSQL.DeleteClient(new ConnectBase().connectionString, id);
                Fill();
            }                     
        }

        private void UpdateRow(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Viol.png");
            Adding.Visibility = Visibility.Visible;
            Header.Text = "Изменение клиента";
            btnLogin.Content = "Изменить";
            ClientsSQL selectedItem = (ClientsSQL)Data.SelectedItem;
            if (selectedItem != null)
            {
                txtUser.Text = selectedItem.Iinitials;
                txtNumber.Text = selectedItem.phoneNumber;
                txtEMail.Text = selectedItem.eMail;
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/ViolPush.png");
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/RedPush.png");
        }
        public void ImageChange(object sender, string path)
        {
            if (sender is Image image)
            {
                image.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(path, System.UriKind.Relative));
            }
        }

        private void SearchClick(object sender, MouseButtonEventArgs e)
        {
            if (!search)
            {
                SearchCombo.Visibility = Visibility.Visible;
                SeachText.Visibility = Visibility.Visible;
                search = true;
                return;
            }
            if(search)
            {
                SearchCombo.Visibility = Visibility.Hidden;
                SeachText.Visibility = Visibility.Hidden;
                search = false;
                Fill();
            }
        }

        private void Image_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(39, 161, -35, 444);
                animation.To = new Thickness(74, 161, 0, 444);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                grid.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(74, 161, 0, 444);
                animation.To = new Thickness(39, 161, -35, 444);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                grid.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void SeachText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data.Items.Clear();
            foreach (ClientsSQL clients in ClientsSQL.SetClients(new ConnectBase().connectionString, SearchCombo.Text, SeachText.Text))
            {
                Data.Items.Add(clients);
            }
        }
    }   
}
