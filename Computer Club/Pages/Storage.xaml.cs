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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Computer_Club.Pages
{
    /// <summary>
    /// Логика взаимодействия для Storage.xaml
    /// </summary>
    public partial class Storage : Page
    {
        public bool search = false;
        public Storage()
        {
            InitializeComponent();
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
            if (search)
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
                animation.From = new Thickness(0, 0, -35, 290);
                animation.To = new Thickness(35, 0, 0, 290);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                grid.BeginAnimation(Grid.MarginProperty, animation);
            }
        }

        private void Image_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!search)
            {
                ThicknessAnimation animation = new ThicknessAnimation();
                animation.From = new Thickness(35, 0, 0, 290);
                animation.To = new Thickness(0, 0, -35, 290);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                grid.BeginAnimation(Grid.MarginProperty, animation);
            }
        }
        private void SeachText_TextChanged(object sender, TextChangedEventArgs e)
        {
            Data.Items.Clear();
            foreach (Items items in Items.SetItems(new ConnectBase().connectionString, SearchCombo.Text, SeachText.Text))
            {
                Data.Items.Add(items);
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
        private void UpdateRow(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Viol.png");
            Adding.Visibility = Visibility.Visible;
            Header.Text = "Изменение комплектующих";
            btnLogin.Content = "Изменить";
            Items selectedItem = (Items)Data.SelectedItem;
            if (selectedItem != null)
            {
                itemName.Text = selectedItem.productName;
                itemType.Text = selectedItem.productType;
                itenQuantity.Text = selectedItem.quantity;
            }
        }

        private void Data_Loaded(object sender, RoutedEventArgs e)
        {
            Fill();
        }

        public void Fill()
        {
            Data.Items.Clear();
            foreach (Items items in Items.SetItems(new ConnectBase().connectionString))
            {
                Data.Items.Add(items);
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Adding.Visibility = Visibility.Visible;
            Header.Text = "Добавление комплектующих";
            btnLogin.Content = "Добавить";
            itemName.Text = "";
            itemType.Text = "";
            itenQuantity.Text = "";
        }
        private void DeleteRow(object sender, MouseButtonEventArgs e)
        {
            ImageChange(sender, "/Images/Red.png");
            string id = "";
            Items selectedItem = (Items)Data.SelectedItem;
            if (selectedItem != null)
            {
                id = selectedItem.Id;
            }
            MessageBoxResult result = System.Windows.MessageBox.Show($"Вы уверены, что хотите удалить предмет №{id}?", "Подтверждение удаления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Items.DeleteItem(new ConnectBase().connectionString, id);
                Fill();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Adding.Visibility = Visibility.Hidden;
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(itemName.Text))
            {
                Error.Text = "Вы не назвали предмет";
                return;
            }
            if (itemType.Text == "")
            {
                Error.Text = "Вы не выбрали тип предмета";
                return;
            }
            if (itenQuantity.Text == "")  {
                Error.Text = "Вы не выбрали тип предмета";
                return;
            }
            if (Header.Text == "Добавление комплектующих")
            {                
                Items.AddItem(new ConnectBase().connectionString, itemName.Text, itenQuantity.Text, itemType.Text);
                Fill();
                Adding.Visibility = Visibility.Hidden;
                return;
            }
            string id = "";
            Items selectedItem = (Items)Data.SelectedItem;
            if (selectedItem != null)
            {
                id = selectedItem.Id;
            }
            Items.UpdateItem(new ConnectBase().connectionString, itemName.Text, itenQuantity.Text, itemType.Text, id);
            Fill();
            Adding.Visibility = Visibility.Hidden;
        }
    }
}
