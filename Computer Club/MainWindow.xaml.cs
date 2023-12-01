using Computer_Club.ModelSQL;
using Computer_Club.Pages;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Computer_Club.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isAdmin = bool.Parse(ConfigurationManager.AppSettings["Admin"]);
        public string id = ConfigurationManager.AppSettings["id"];

        Controle_panel controle_panel;
        Settings settings;
        Clients clients;
        Service service;
        Storage storage;
        public MainWindow()
        {
            InitializeComponent();
            controle_panel = new Controle_panel();
            settings = new Settings();
            clients = new Clients();
            service = new Service();
            storage = new Storage();

        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void pnlControlBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                System.Windows.Application.Current.Shutdown();
            }
            Hide();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            ConnectBase connectBase = new ConnectBase();
            string query = $"Update Computerclub SET Computerstate = 'Off' Where ID = '{id}'";
            SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
            command.ExecuteNonQuery();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            content.Content = controle_panel;
            panelBlock.Text = "Панель управления";
            panelIcon.Icon = IconChar.Home;
        }

        private void RadioButton_Click_1(object sender, RoutedEventArgs e)
        {
            content.Content = settings;
            panelBlock.Text = "Настройки";
            panelIcon.Icon = IconChar.Tools;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            content.Content = controle_panel;            
        }

        private void RadioButton_Click_2(object sender, RoutedEventArgs e)
        {
            content.Content = clients;
            panelBlock.Text = "Клиенты";
            panelIcon.Icon = IconChar.UserGroup;
        }

        private void RadioButton_Click_3(object sender, RoutedEventArgs e)
        {
            content.Content = service;
            panelBlock.Text = "Услуги";
            panelIcon.Icon = IconChar.HandHolding;
        }

        private void RadioButton_Click_4(object sender, RoutedEventArgs e)
        {
            content.Content = storage;
            panelBlock.Text = "Комплектующие";
            panelIcon.Icon = IconChar.Boxes;
        }
    }
}
