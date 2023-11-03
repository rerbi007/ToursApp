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
using Toolkit =  Xceed.Wpf.Toolkit;

namespace ToursApp
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Hotel _currentHotel = new Hotel();

        public AddEditPage(Hotel selectedHotel)
        {
            InitializeComponent();

            if(selectedHotel != null) 
                _currentHotel= selectedHotel;

            DataContext = _currentHotel;
            ComboCountries.ItemsSource = ToursBaseEntities.GetContext().Countries.ToList();
            ComboCountries.SelectedValue = 1;

            List<int> stars = new List<int>() { 1, 2, 3, 4, 5 };
            ComboStars.ItemsSource = stars;
            ComboStars.SelectedIndex = 2;

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentHotel.Name))
                errors.AppendLine("Укажите название отеля");
            if (_currentHotel.CountOfStars < 1 || _currentHotel.CountOfStars > 5)
                errors.AppendLine("Количество звезд - число от 1 до 5");
            if (_currentHotel.Country == null)
                errors.AppendLine("Выберите страну");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentHotel.Id == 0)
                ToursBaseEntities.GetContext().Hotels.Add(_currentHotel);
            try
            {
                ToursBaseEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Toolkit.MaskedTextBox box)
            {
                if (string.IsNullOrEmpty(box.Text))
                    box.Background = (ImageBrush)FindResource("watermark");
                else
                    box.Background = null;
            }
        }
    }
}
