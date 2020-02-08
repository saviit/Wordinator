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

namespace AboutDialog
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        /// <summary>
        /// Oletusmuodostaja
        /// </summary>
        public AboutDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja. Alustaa AboutDialogin tekstikentät annetuilla arvoilla.
        /// </summary>
        /// <param name="title">Ohjelman nimi</param>
        /// <param name="version">Ohjelman versio</param>
        /// <param name="vdate">Ohjelman versiopäiväys</param>
        /// <param name="author">Ohjelman tekijä</param>
        public AboutDialog(String title, String version, String vdate, String author)
        {
            InitializeComponent();
            this.Title = "Tietoja " + title + "ista";
            labelOtsikko.Content = "Tietoja " + title + "rista";
            ohjelmanNimi.Content = title;
            tekijanNimi.Content = author;
            versioNro.Content = version;
            paivays.Content = vdate;
        }

        /// <summary>
        /// Käsittelijä 'Sulje'-napin painallukselle. Piilottaa ikkunan näkyvistä
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSulje_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        
    }
}
