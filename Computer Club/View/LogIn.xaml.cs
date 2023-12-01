using System.Windows;
using System.Windows.Input;
using Computer_Club.ModelSQL;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Win32;
using System.Reflection;
using System.Windows.Forms;
using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Interop;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Window = System.Windows.Window;
using System.Windows.Media;

namespace Computer_Club.View
{
    /// <summary>
    /// Логика взаимодействия для LogIn.xaml
    /// </summary>
    public partial class LogIn : System.Windows.Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, IntPtr text, int count);

        private System.Timers.Timer timer;



        public bool isAdmin = bool.Parse(ConfigurationManager.AppSettings["Admin"]);
        private NotifyIcon notifyIcon;
        public string id = ConfigurationManager.AppSettings["id"];
        public DateTime dateTime;
        public DialogResult result = System.Windows.Forms.DialogResult.None;
        private object lockObject = new object();
        private bool isDialogOpen = false;

        public LogIn()
        {
            InitializeComponent();            
        }
        private void Timer_Tick()
        {
            lock(lockObject)
            {
                if (!isDialogOpen && DateTime.Now > dateTime)
                {
                    this.Topmost = true;
                    isDialogOpen = true;
                    System.Windows.Forms.MessageBox.Show("Время вышло!", "Предупреждение");
                    isDialogOpen = false;
                    this.Topmost = false;
                }
            }
        }

        private void ImPlayer()
        {
            string appPath = Assembly.GetExecutingAssembly().Location;
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey runKey = currentUserKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            runKey.SetValue("MyApp", appPath);
            runKey.Close();
            notifyIcon = new NotifyIcon();
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico");
            notifyIcon.Icon = new System.Drawing.Icon(iconPath);
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            notifyIcon.Text = "Computer club";
            notifyIcon.Visible = true;
            notifyIcon.Click += (sender, e) =>
            {
                ShowMainWindow();
            };
            Closing += (sender, e) =>
            {
                notifyIcon.Dispose();
            };

        }
        private void ShowMainWindow()
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
                Activate();
            }
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }
        }
        private void MainForm_FormClosing(object sender, CancelEventArgs e)
        {
            if (!isAdmin)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (isAdmin)
            {
                System.Windows.Application.Current.Shutdown();
            }
            WindowState = WindowState.Minimized;
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            ConnectBase connectBase = new ConnectBase();
            SqlConnection connection = connectBase.GetConnection();
            string query = $"select Password from LogIn where Login = '{txtUser.Text}'";
            SqlCommand command = new SqlCommand(query, connection);
            if (txtPass.Password == (string)command.ExecuteScalar())
            {
                MainWindow winMain = new MainWindow();
                winMain.Show();
                this.Hide();
            }
            else
            {
                txtError.Text = "Wrong login or password";
            }
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            ConnectBase connectBase = new ConnectBase();
            string query = $"Update Computerclub SET Computerstate = 'Off' Where ID = '{id}'";
            SqlCommand command = new SqlCommand(query, connectBase.GetConnection());
            command.ExecuteNonQuery();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isAdmin)
            {
                ImPlayer();

                string query = $"SELECT Time FROM Services WHERE ComputerId = {id}";
                SqlCommand command = new SqlCommand(query, new ConnectBase().GetConnection());
                int time = 0;
                if (command.ExecuteScalar() != null)
                    time = (int)command.ExecuteScalar();
                dateTime = DateTime.Now.AddHours(time);
                Thread updateThread = new Thread(() =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        while (true)
                        {
                            if (!isDialogOpen && DateTime.Now > dateTime)
                            {
                                var customMessageBox = new CustomMessageBox("Время вышло");
                                customMessageBox.ShowDialog();
                            }
                        }
                    });
                });

                updateThread.Start();
            }
        }
    }
    public class CustomMessageBox : Window
    {
        public CustomMessageBox(string message)
        {
            object obj = new object();
            Title = "Предупреждение";
            Width = 300;
            Height = 100;
            double left = 0;
            double top = 0;
            double width = Width;
            double height = Height;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Tick += Timer_Tick;

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;

            var textBlock = new TextBlock
            {
                Text = message,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20
            };

            var okButton = new System.Windows.Controls.Button
            {
                Content = "OK",
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 10, 0, 0),
                Width = 100
            };
            okButton.Click += (sender, e) => Close();

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(okButton);
            Closed += (sender, e) =>
            {
                timer.Stop();
            };
            Content = stackPanel;
            Loaded += (sender, e) =>
            {                
                timer.Start();
            };
            
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            // Изменяем положение курсора
            if (!IsMouseOver)
            {
                System.Drawing.Point cursorPosition = System.Windows.Forms.Cursor.Position;
                if (cursorPosition.X < Left || cursorPosition.X >= Left + Width ||
                cursorPosition.Y < Top || cursorPosition.Y >= Top + Height)
                {
                    System.Windows.Point centerPosition = new System.Windows.Point(Width / 2, Height / 2);

                    Cursor = System.Windows.Input.Cursors.None; // Скрываем курсор
                    System.Windows.Point targetPosition = PointToScreen(centerPosition);
                    NativeMethods.SetCursorPos((int)targetPosition.X, (int)targetPosition.Y);
                    Cursor = System.Windows.Input.Cursors.Arrow;
                }
            }
        }
        private static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool SetCursorPos(int x, int y);
        }
    }
}


