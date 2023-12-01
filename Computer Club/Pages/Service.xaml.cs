using Computer_Club.ModelSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataGrid = System.Windows.Controls.DataGrid;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace Computer_Club.Pages
{
    /// <summary>
    /// Логика взаимодействия для Service.xaml
    /// </summary>
    public partial class Service : Page
    {
        public bool search = false;
        public Service()
        {
            InitializeComponent();
            OffImage.Visibility = Visibility.Hidden;
        }
        public void FillIf(DataGrid dataGrid, string condition, string condition2,string condition3, string condition4 = "")
        {
            dataGrid.Items.Clear();
            if (condition == "Имя сервиса")
            {
                foreach (Services services in Services.SetServices(new ConnectBase().connectionString))
                {
                
                    if (services.computerState != "Break" && services.serviceName.Contains(condition2)&&(services.serviceState == condition || services.serviceState == condition2))
                    {
                        dataGrid.Items.Add(services);
                    }
                }
            }
            if (condition == "Имя компьютера")
            {
                foreach (Services services in Services.SetServices(new ConnectBase().connectionString))
                {

                    if (services.computerState != "Break" && services.computerName.Contains(condition2)&&(services.serviceState == condition || services.serviceState == condition2))
                    {
                        dataGrid.Items.Add(services);
                    }
                }
            }
        }
        public void Fill(DataGrid dataGrid,string condition, string condition2 = "")
        {
            string cond = "";
            if (condition == "None")
                cond = "Break"; 
            dataGrid.Items.Clear();
            foreach (Services services in Services.SetServices( new ConnectBase().connectionString))
            {
                if (services.computerState != cond && (services.serviceState == condition || services.serviceState == condition2))
                {
                    dataGrid.Items.Add(services);
                }
            }
        }

        private void DataOn_Loaded(object sender, RoutedEventArgs e)
        {
            Fill(DataOn, "Work", "Wait");
        }

        private void ImageChangeOn(object sender, MouseButtonEventArgs e)
        {
            PCOn.Visibility = Visibility.Hidden;
            PCOff.Visibility = Visibility.Visible;
            OffImage.Visibility = Visibility.Visible;
            search = false;
            Fill(DataOff, "None");
        }

        private void Image_MouseEnterOn(object sender, MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(0, 0, -35, 305);
                animation.To = new Thickness(35, 0, 0, 305);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                gridOn.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(35, 0, 0, 305);
                animation.To = new Thickness(0, 0, -35, 305);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                gridOn.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void SearchClickOn(object sender, MouseButtonEventArgs e)
        {
            if (!search)
            {
                SearchComboOn.Visibility = Visibility.Visible;
                SeachTextOn.Visibility = Visibility.Visible;
                search = true;
                return;
            }
            if (search)
            {
                SearchComboOn.Visibility = Visibility.Hidden;
                SeachTextOn.Visibility = Visibility.Hidden;
                search = false;
                Fill(DataOn, "Work", "Wait");
            }
        }

        private void SeachTextOn_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillIf(DataOn, SearchComboOn.Text, SeachTextOn.Text, "Work", "Wait");
        }

        private void ImageChangeOff(object sender, MouseButtonEventArgs e)
        {
            PCOn.Visibility = Visibility.Visible;
            PCOff.Visibility = Visibility.Hidden;
            OffImage.Visibility = Visibility.Hidden;
            search = false;
            Fill(DataOff, "None");
        }

        private void Image_MouseEnterOff(object sender, MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(0, 0, -35, 305);
                animation.To = new Thickness(35, 0, 0, 305);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                gridOff.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void Image_MouseLeaveOff(object sender, MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(35, 0, 0, 305);
                animation.To = new Thickness(0, 0, -35, 305);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                gridOff.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void SearchClickOff(object sender, MouseButtonEventArgs e)
        {
            if (!search)
            {
                SearchComboOff.Visibility = Visibility.Visible;
                SeachTextOff.Visibility = Visibility.Visible;
                search = true;
                return;
            }
            if (search)
            {
                SearchComboOff.Visibility = Visibility.Hidden;
                SeachTextOff.Visibility = Visibility.Hidden;
                search = false;
                Fill(DataOff, "None");
            }
        }

        private void SeachTextOff_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillIf(DataOff, SearchComboOff.Text, SeachTextOff.Text, "None");
        }

        private void MouseLeftButtonUpOff(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Green.png");
            Adding.Visibility = Visibility.Visible;
            comboClients.Items.Clear();
            foreach (ClientsSQL services in ClientsSQL.SetUselessClients(new ConnectBase().connectionString))
            {                
                comboClients.Items.Add(services);                
            }
        }

        private void MouseLeftButtonDownOff(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/GreenPush.png");
        }
        public void ImageChange(object sender, string path)
        {
            if (sender is Image image)
            {
                image.Source = new System.Windows.Media.Imaging.BitmapImage(new System.Uri(path, System.UriKind.Relative));
            }
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            int time = 0;
            int cost = 0;
            if (!int.TryParse(serviceTime.Text,out time))
            {
                Error.Text = "Введите число в поле со временем";
                return;
            }
            if (!int.TryParse(serviceCost.Text, out cost))
            {
                Error.Text = "Введите число в поле со стоимостью";
                return;
            }
            if (serviceName.Text == "")
            {
                Error.Text = "Вы не выбрали тип услуги";
                return;
            }
            if (comboClients.Text == "")
            {
                Error.Text = "Вы не выбрали клиента";
                return;
            }
            cost *= time;
            ClientsSQL clientsSQL = (ClientsSQL)comboClients.SelectedValue;
            Services services = (Services)DataOff.SelectedItem;
            Services.AddSecvices(new ConnectBase().connectionString, int.Parse(clientsSQL.ID),services.computerId, serviceName.Text,
                time,cost.ToString());
            Fill(DataOff, "None");
            Fill(DataOn, "Work", "Wait");
            comboClients.Text = "";
            serviceName.Text = "";
            serviceTime.Text = "";
            serviceCost.Text = "";
            Adding.Visibility = Visibility.Hidden;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            comboClients.Text = "";
            serviceName.Text = "";
            serviceTime.Text = "";
            serviceCost.Text = "";
            Adding.Visibility = Visibility.Hidden;
        }

        private void DeleteService(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Red.png");
            MessageBoxResult result = System.Windows.MessageBox.Show($"Вы уверены, что хотите удалить услугу?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                return;
            }
            Services services =  (Services)DataOn.SelectedItem;
            Services.DeleteServices(new ConnectBase().connectionString, services.computerId);
            Fill(DataOff, "None");
            Fill(DataOn, "Work", "Wait");
        }

        private void DeletServicePush(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/RedPush.png");
        }
    }
}
