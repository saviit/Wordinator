using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;

namespace SettingsDialog
{

    public partial class SettingsDialog : Window
    {
        // Oletusvaikeusaste on helppo
        // 0 == helppo, 1 == keski, 2 == vaikea
        private int Vaikeusaste = 0;
        //private List<VariComboboxItem> varit = new List<VariComboboxItem>();
        //oletusvärit
        private static readonly SolidColorBrush DefaultActiveTausta = new SolidColorBrush(Colors.LightBlue);
        private static readonly SolidColorBrush DefaultActiveReunus = new SolidColorBrush(Colors.SteelBlue);
        private static readonly SolidColorBrush DefaultInactiveTausta = new SolidColorBrush(Colors.SteelBlue);
        private static readonly SolidColorBrush DefaultInactiveReunus = new SolidColorBrush(Colors.Goldenrod);


        /// <summary>Muodostaja</summary>
        public SettingsDialog()
        {
            InitializeComponent();
            LueVaritComboBoxeihin();
        }

        /// <summary>
        /// Käsittelijä valintojen hyväksymiselle. Asetetaan valinnat voimaan ja piilotetaan
        /// asetusikkuna.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        /// <returns>true</returns>
        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rBtnVaikeusHelppo.IsChecked) Vaikeusaste = 0;
            if ((bool)rBtnvaikeusKeski.IsChecked) Vaikeusaste = 1;
            if ((bool)rBtnVaikeusVaikea.IsChecked) Vaikeusaste = 2;
            
            this.DialogResult = true;
            this.Hide();
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä valintojen peruuttamiselle. Piilotetaan asetusikkuna eikä aseteta
        /// valintoja voimaan
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        /// <returns>false</returns>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            //SetAsetukset();
            this.DialogResult = false;
            // Piilotetaan asetusikkuna
            this.Hide();
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä 'Palauta oletukset' -napin painnallukselle (oletusvärien palauttamiselle)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonPalautaOletusVarit_Click(object sender, RoutedEventArgs e)
        {
            List<SolidColorBrush> oletusvarit = new List<SolidColorBrush>();
            oletusvarit.Add(DefaultActiveTausta);
            oletusvarit.Add(DefaultActiveReunus);
            oletusvarit.Add(DefaultInactiveTausta);
            oletusvarit.Add(DefaultInactiveReunus);
            SetAsetukset(oletusvarit, Vaikeusaste);
        }


        /// <summary>
        /// Palauttaa valitun vaikeusasteen
        /// </summary>
        /// <returns>kokonaisluvun joka ilmaisee valitun vaikeusasteeen</returns>
        public int GetVaikeusaste()
        {
            return Vaikeusaste;
        }

        /// <summary>
        /// Palauttaa valitun värin aktiivisen Kirjainlaatan reunusväriksi
        /// </summary>
        /// <returns>SolidColorBrush objektin joka ilmaiseen Kirjainlaattojen reunusvärin</returns>
        public SolidColorBrush GetActiveLaatanReunusVari()
        {
            if (cboxActiveLaattaReunuksenVari.SelectedItem == null) return DefaultActiveReunus;
            return ((VariComboboxItem)cboxActiveLaattaReunuksenVari.SelectedItem).Vari;
        }

        /// <summary>
        /// Palauttaa valitun värin aktiivisen Kirjainlaatan taustaväriksi
        /// </summary>
        /// <returns>SolidColorBrush objektin joka ilmaisee Kirjainlaattojen taustavärin</returns>
        public SolidColorBrush GetActiveLaatanTaustaVari()
        {
            if (cboxActiveLaattaTaustaVari.SelectedItem == null) return DefaultActiveTausta;
            return ((VariComboboxItem)cboxActiveLaattaTaustaVari.SelectedItem).Vari;
        }

        /// <summary>
        /// Palauttaa valitun värin epäaktiivisen Kirjainlaatan reunusväriksi
        /// </summary>
        /// <returns>SolidColorBrush objektin joka ilmaiseen Kirjainlaattojen reunusvärin</returns>
        public SolidColorBrush GetInactiveLaatanReunusVari()
        {
            if (cboxInactiveLaattaReunuksenVari.SelectedItem == null) return DefaultInactiveReunus;
            return ((VariComboboxItem)cboxInactiveLaattaReunuksenVari.SelectedItem).Vari;
        }

        /// <summary>
        /// Palauttaa valitun värin epäaktiivisen Kirjainlaatan taustaväriksi
        /// </summary>
        /// <returns>SolidColorBrush objektin joka ilmaisee Kirjainlaattojen taustavärin</returns>
        public SolidColorBrush GetInactiveLaatanTaustaVari()
        {
            if (cboxInactiveLaattaTaustaVari.SelectedItem == null) return DefaultInactiveTausta;
            return ((VariComboboxItem)cboxInactiveLaattaTaustaVari.SelectedItem).Vari;
        }

        /// <summary>
        /// Asettaa ohjelman asetusten mukaiset valinnat valituiksi
        /// </summary>
        /// <param name="colors">Kirjainlaattojen värit</param>
        /// <param name="vaikeusaste">Pelin vaikeustaso</param>
        public void SetAsetukset(List<SolidColorBrush> colors, int vaikeusaste)
        {
            // Asetetaan configin mukaiset värit valituiksi
            // värit järjestyksessä colors[0] == aktiivinen tausta
            //                      colors[1] == aktiivinen reunus
            //                      colors[2] == epäaktiivinen tausta
            //                      colors[3] == epäaktiivinen reunus
            foreach (VariComboboxItem item in cboxActiveLaattaTaustaVari.Items)
            {
                if (item.Vari.Color.Equals(colors[0].Color))
                {
                    cboxActiveLaattaTaustaVari.SelectedIndex = cboxActiveLaattaTaustaVari.Items.IndexOf(item);
                }
            }
            foreach (VariComboboxItem item in cboxActiveLaattaReunuksenVari.Items)
            {
                if (item.Vari.Color.Equals(colors[1].Color))
                {
                    cboxActiveLaattaReunuksenVari.SelectedIndex = cboxActiveLaattaReunuksenVari.Items.IndexOf(item);
                }
            }
            foreach (VariComboboxItem item in cboxInactiveLaattaTaustaVari.Items)
            {
                if (item.Vari.Color.Equals(colors[2].Color))
                {
                    cboxInactiveLaattaTaustaVari.SelectedIndex = cboxInactiveLaattaTaustaVari.Items.IndexOf(item);
                }
            }
            foreach (VariComboboxItem item in cboxInactiveLaattaReunuksenVari.Items)
            {
                if (item.Vari.Color.Equals(colors[3].Color))
                {
                    cboxInactiveLaattaReunuksenVari.SelectedIndex = cboxInactiveLaattaReunuksenVari.Items.IndexOf(item);
                    
                }
            }

            // Asetetaan configin mukainen vaikeusaste
            switch (vaikeusaste)
            {
                case 0:
                    rBtnVaikeusHelppo.IsChecked = true;
                    rBtnvaikeusKeski.IsChecked = false;
                    rBtnVaikeusVaikea.IsChecked = false;
                    break;
                case 1:
                    rBtnVaikeusHelppo.IsChecked = false;
                    rBtnvaikeusKeski.IsChecked = true;
                    rBtnVaikeusVaikea.IsChecked = false;
                    break;
                case 2:
                    rBtnVaikeusHelppo.IsChecked = false;
                    rBtnvaikeusKeski.IsChecked = false;
                    rBtnVaikeusVaikea.IsChecked = true;
                    break;
                default:
                    rBtnVaikeusHelppo.IsChecked = true;
                    rBtnvaikeusKeski.IsChecked = false;
                    rBtnVaikeusVaikea.IsChecked = false;
                    break;
            }
        }

        /// <summary>
        /// Täyttää comboboxit valittavilla väreillä
        /// </summary>
        private void LueVaritComboBoxeihin()
        {
            List<VariComboboxItem> ActiveTaustaVarit = new List<VariComboboxItem>();
            List<VariComboboxItem> ActiveReunusVarit = new List<VariComboboxItem>();
            List<VariComboboxItem> InactiveTaustaVarit = new List<VariComboboxItem>();
            List<VariComboboxItem> InactiveReunusVarit = new List<VariComboboxItem>();
            // Käydään läpi kaikki nimetyt värit System.Windows.Media.Colors rakenteesta
            System.Reflection.PropertyInfo[] colors = (typeof(Colors).GetProperties());
            foreach (PropertyInfo property in colors)
            {
                VariComboboxItem vari = new VariComboboxItem(new SolidColorBrush((Color)ColorConverter.ConvertFromString(property.Name)), property.Name);
                // pitää tehdä kopiot, muuten comboboxit ei toimi oikein
                VariComboboxItem vari2 = new VariComboboxItem(vari.Vari, vari.Teksti);
                VariComboboxItem vari3 = new VariComboboxItem(vari.Vari, vari.Teksti);
                VariComboboxItem vari4 = new VariComboboxItem(vari.Vari, vari.Teksti);
                ActiveTaustaVarit.Add(vari);
                ActiveReunusVarit.Add(vari2);
                InactiveTaustaVarit.Add(vari3);
                InactiveReunusVarit.Add(vari4);
            }
            cboxActiveLaattaReunuksenVari.ItemsSource = ActiveReunusVarit;
            cboxActiveLaattaTaustaVari.ItemsSource = ActiveTaustaVarit;
            cboxInactiveLaattaReunuksenVari.ItemsSource = InactiveReunusVarit;
            cboxInactiveLaattaTaustaVari.ItemsSource = InactiveTaustaVarit;
        }

        /// <summary>
        /// Käsittelijä aktiivisen Kirjainlaatan reunusvärin vaihdolle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxActiveLaattaReunuksenVari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboxActiveLaattaReunuksenVari.SelectedItem = e.Source;
            Esimerkkilaatta_Active.BorderColor = GetActiveLaatanReunusVari();
        }

        /// <summary>
        /// Käsittelijä aktiivisen Kirjainlaatan taustavärin vaihdolle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxActiveLaattaTaustaVari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboxActiveLaattaTaustaVari.SelectedItem = e.Source;
            Esimerkkilaatta_Active.FillColor = GetActiveLaatanTaustaVari();
        }

        /// <summary>
        /// Käsitttelijä epäaktiivisen Kirjainlaatan reunusvärin vaihdolle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxInactiveLaattaReunuksenVari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboxInactiveLaattaReunuksenVari.SelectedItem = e.Source;
            Esimerkkilaatta_Inactive.BorderColor = GetInactiveLaatanReunusVari();
        }

        /// <summary>
        /// Käsitttelijä epäaktiivisen Kirjainlaatan taustavärin vaihdolle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxInactiveLaattaTaustaVari_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cboxInactiveLaattaTaustaVari.SelectedItem = e.Source;
            Esimerkkilaatta_Inactive.FillColor = GetInactiveLaatanTaustaVari();
        }

        
    }
    
}
