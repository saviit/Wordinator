using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KirjainValintaDialog
{
    /// <summary>
    /// Interaction logic for KirjainValintaDialog.xaml
    /// </summary>
    public partial class KirjainValintaDialog : Window
    {
        private String valittuKirjain = "";
        private Button edellinenValittu;

        /// <summary>Muodostaja</summary>
        public KirjainValintaDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sulkee valintaikkunan. Ei päästä käyttäjää poistumaan ennen kuin on valittu jokin kirjain
        /// tyhjälle laatalle.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (valittuKirjain == String.Empty)
            {
                MessageBox.Show("Valitse kirjain tyhjälle laatalle.", "Valitse kirjain", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.Hide();
            }
        }

        /// <summary>
        /// Palauttaa valitun kirjaimen merkkijonona sitä tarvitseville ulkoisille luokille.
        /// </summary>
        /// <returns>valitun kirjaimen</returns>
        public String GetValittuKirjain()
        {
            return valittuKirjain;
        }

        private void kirjaimet_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)e.OriginalSource;
            if (ButtonCheck.GetIsChecked(button) != true)
            {
                ButtonCheck.SetIsChecked(button, true);
                //aiemmin valittu pitää asettaa ei-valituksi
                if (edellinenValittu != null) ButtonCheck.SetIsChecked(edellinenValittu, false);
            }
            valittuKirjain = button.Content.ToString();
            edellinenValittu = button;
            
        }
    }


    /// <summary>
    /// Apuriluokka jolla saadaan attached propetyt käyttöön
    /// </summary>
    public static class ButtonCheck
    {
        /// <summary>
        /// Attached DependencyProperty to determine whether Button is active
        /// </summary>
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.RegisterAttached("IsChecked", typeof(Boolean), typeof(Button),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsCheckedChanged)));

        public static Boolean GetIsChecked(Button target)
        {
            return (Boolean)target.GetValue(IsCheckedProperty);
        }

        public static void SetIsChecked(Button target, Boolean value)
        {
            target.SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// Value change handler for attached dependency property 'IsChecked'
        /// </summary>
        /// <param name="obj">the object in which the change occurred</param>
        /// <param name="args">parameters of the change (values)</param>
        private static void OnIsCheckedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Button button = (Button)obj;
            if (args.NewValue.Equals(false))
                button.Background = new SolidColorBrush(Colors.LightBlue);
            if (args.NewValue.Equals(true))
                button.Background = new SolidColorBrush(Colors.SteelBlue);
        }
    }
}
