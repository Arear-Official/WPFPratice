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
using System.Configuration;
using Computer_Club.ModelSQL;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Computer_Club.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    
    public partial class Settings : Page
    {
        bool isclicked = false;
        bool isbreak = false;
        public bool isAdmin = bool.Parse(ConfigurationManager.AppSettings["Admin"]);
        public string id = ConfigurationManager.AppSettings["id"];
        bool ischange = false;
        public Settings()
        {
            InitializeComponent();
            if (!isAdmin)
            {
                ImageChange(slider, "/Images/SliderOn.png");
                isclicked = true;
                Player.Visibility = Visibility.Visible;
                ConnectBase connectBase = new ConnectBase();
                SqlConnection connection = connectBase.GetConnection();
                string query = $"select Computername from Computerclub where ID = '{id}'";
                SqlCommand command = new SqlCommand(query, connection);
                PCName.Text = (string)command.ExecuteScalar();
                query = $"select Computertype from Computerclub where ID = '{id}'";
                command = new SqlCommand(query, connection);
                PCType.Text = (string)command.ExecuteScalar();
                query = $"select Computerstate from Computerclub where ID = '{id}'";
                command = new SqlCommand(query, connection);
                isbreak = "Break" == (string)command.ExecuteScalar();
                if (isbreak)
                {
                    ImageChange(breakSlider, "/Images/SliderOn.png");
                }
            }

        }

        private void btnCgangeClick(object sender, RoutedEventArgs e)
        {
            if (!ischange)
            {
                ConnectBase connectBase = new ConnectBase();
                SqlConnection connection = connectBase.GetConnection();
                string query = $"select Password from LogIn where Login = '{txtUser.Text}'";
                SqlCommand command = new SqlCommand(query, connection);
                if (txtPass.Text == (string)command.ExecuteScalar())
                {
                    ischange = true;
                    btnChange.Content = "Изменить";
                }
                else
                {
                    txtError.Text = "Не верный логин или пароль";
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtUser.Text) && string.IsNullOrWhiteSpace(txtPass.Text)) { txtError.Text = "Вы не ввели логин или пароль"; }
                else
                {
                    ConnectBase connectBase = new ConnectBase();
                    string query = $"Update LogIn SET Password = @pass, Login = @user Where ID = 1";
                    SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
                    command.Parameters.AddWithValue("@pass", txtPass.Text);
                    command.Parameters.AddWithValue("@user", txtUser.Text);
                    command.ExecuteNonQuery();
                    ChangeLP.Visibility = Visibility.Hidden;
                    refresText.Visibility = Visibility.Visible;
                }
            }
            txtPass.Text = "";
            txtUser.Text = "";
        }

        private void Adding_Click(object sender, RoutedEventArgs e)
        {
            ChangeLP.Visibility = Visibility.Visible;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isclicked) 
            {
                ImageChange(sender, "/Images/SliderOn.png");

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Admin"].Value = "false";
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                ConnectBase connectBase = new ConnectBase();
                string query = $"INSERT INTO Computerclub VALUES (@type,'On',@name,'None','None') ";
                SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
                command.Parameters.AddWithValue("@name", PCName.Text);
                command.Parameters.AddWithValue("@type", PCType.Text);
                command.ExecuteNonQuery();

                query = $"SELECT MAX(Id) From Computerclub";
                command = new SqlCommand(query, connectBase.GetConnection());

                config.AppSettings.Settings["id"].Value = command.ExecuteScalar().ToString();
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                id = ConfigurationManager.AppSettings["id"];

                Services.AddSecvices(connectBase.connectionString, 0, id.ToString(), "None", 0, "0");

                refresText.Visibility = Swap(refresText.Visibility);
                Player.Visibility = Visibility.Visible;
                isclicked = true; 
            }
            else
            {
                ImageChange(sender, "/Images/SliderOff.png");

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Admin"].Value = "true";
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                refresText.Visibility = Swap(refresText.Visibility);
                Player.Visibility = Visibility.Hidden;
                isclicked = false;
            }
        }
        public void ImageChange(object sender, string path)
        {
            if (sender is Image image)
            {
                image.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(path, System.UriKind.Relative));
            }
        }
        public Visibility Swap( Visibility visibility)
        {
            if (visibility == Visibility.Visible) { return Visibility.Hidden; }
            return Visibility.Visible;
        }

        private void btnNameClick(object sender, RoutedEventArgs e)
        {            
                ConnectBase connectBase = new ConnectBase();
                string query = $"Update Computerclub SET Computername = @name, Computertype = @type Where ID = '{id}'";
                SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
                command.Parameters.AddWithValue("@name", PCName.Text);
                command.Parameters.AddWithValue("@type", PCType.Text);
                command.ExecuteNonQuery();            
        }

        private void BreakMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isAdmin)
            {
                MessageBox.Show("Пока это не пользовательский компьютер, изменение на возможно");
                return;
            }
            if (isbreak)
            {
                ImageChange(breakSlider, "/Images/SliderOff.png");
                isbreak = false;
                ConnectBase connectBase = new ConnectBase();
                string query = $"Update Computerclub SET Computerstate = 'On' Where ID = '{id}'";
                SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
                command.ExecuteNonQuery();
            }
            else
            {
                ImageChange(breakSlider, "/Images/SliderOn.png");
                isbreak = true;
                ConnectBase connectBase = new ConnectBase();
                string query = $"Update Computerclub SET Computerstate = 'Break' Where ID = '{id}'";
                SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
                command.ExecuteNonQuery();
            }
        }
    }
}
