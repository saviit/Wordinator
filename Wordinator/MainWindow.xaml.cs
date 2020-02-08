using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameComponents;

namespace Wordinator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private property fields

        /// <summary>
        /// Vakiokentät pelin vaikeusasteen (aloitusajan) määrittämiseksi
        /// </summary>
        private static readonly int[] VAIKEUSASTE = new int[3] { 180, 120, 60 };

        #region Program settings value holders

        private String programTitle;
        private String programAuthor;
        private String programVersion;
        private String programVersionDate;
        private static List<SolidColorBrush> varit = new List<SolidColorBrush>();

        #endregion

        #endregion

        #region Component declarations

        /// <summary>Ikkuna ohjelman About-dialogia varten</summary>
        private Window aboutWindow; // = new AboutDialog.AboutDialog();
        /// <summary>SettingDialogin alustus</summary>
        private SettingsDialog.SettingsDialog settingsWindow;// = new SettingsDialog.SettingsDialog();
        /// <summary>KirjainValintaDialogin alustus</summary>
        private KirjainValintaDialog.KirjainValintaDialog kirjainValintaWindow = new KirjainValintaDialog.KirjainValintaDialog();
        /// <summary>Pääohjelman aloitusnnäytön alustus</summary>
        private Aloitusnaytto.Aloitusnaytto aloitusnaytto;// = new Aloitusnaytto.Aloitusnaytto();

        /// <summary>Käyttöliittymän kontrollien esittelyt</summary>
        private DockPanel window_dockpanel;
        private StackPanel peliControlsPanel;
        private StackPanel pelaajaTiedotPanel;
        private Pelilauta pelilauta1;
        //private PelilautaRuutu pelilautaruutu;
        private KirjainlaattaHolder kirjainlaattaHolder1;
        private Aikalaskuri aikalaskuri1;
        private Pistelaskuri pistelaskuri1;
        private KirjainlaattaPussi kirjainlaattaPussi1;
        private Button buttonLaskePisteet;
        private Button buttonVaihdaKirjaimet;
        private Label lblLaatatKaytossa;
        private Label lblAika;
        private Label lblPisteet;
        private Label lblLaattojaJaljella;
        private TextBlock txtVaihtojaJaljella;

        #endregion

        #region Constructor

        /// <summary>
        /// Alustaa pääikkunan
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            varit.Add(ActiveKirjainlaatanTaustavari);
            varit.Add(ActiveKirjainlaatanReunusvari);
            varit.Add(InactiveKirjainlaatanTaustavari);
            varit.Add(InactiveKirjainlaatanReunusvari);
            LueAsetukset();
            settingsWindow = new SettingsDialog.SettingsDialog();
            aloitusnaytto = new Aloitusnaytto.Aloitusnaytto(programVersion, programVersionDate);
            aboutWindow = new AboutDialog.AboutDialog(programTitle, programVersion, programVersionDate, programAuthor);      
            settingsWindow.SetAsetukset(varit, Vaikeusaste);

            window_content.Children.Add(aloitusnaytto);
        }

        #endregion

        #region Dependency properties

        #region Vaikeustaso
        /// <summary>
        /// DependencyProperty jolla määrätään uuden pelin vaikeusaste
        /// </summary>
        public static readonly DependencyProperty VaikeusasteProperty =
            DependencyProperty.Register("Vaikeusaste", typeof(int), typeof(MainWindow),
            new FrameworkPropertyMetadata(0)); //, FrameworkPropertyMetadataOptions.None,
                //new PropertyChangedCallback(OnVaikeusasteChanged)));

        public int Vaikeusaste
        {
            get { return (int)GetValue(VaikeusasteProperty); }
            set { SetValue(VaikeusasteProperty, value); }
        }

        private static void OnVaikeusasteChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            MainWindow main = (MainWindow)obj;
            if (!(args.NewValue.Equals(main.Vaikeusaste)))
            {
                MessageBox.Show("Uusi vaikeusaste astuu voimaan vasta kun aloitat uuden pelin.", "Vaikeusasteen muutos", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        #region Kirjainten vaihdot
        /// <summary>
        /// Dependency property jolla määritetään voiko pelaaja vaihtaa käytössään olevat kirjaimet uusiin.
        /// </summary>
        public static readonly DependencyProperty KirjaintenVaihtojaJaljellaProperty =
            DependencyProperty.Register("KirjaintenVaihtojaJaljella", typeof(int), typeof(MainWindow),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender)); //,
                //new PropertyChangedCallback(OnKirjaintenVaihtojaJaljellaChanged)));

        public int KirjaintenVaihtojaJaljella
        {
            get { return (int)GetValue(KirjaintenVaihtojaJaljellaProperty); }
            set { SetValue(KirjaintenVaihtojaJaljellaProperty, value); }
        }

        private static void OnKirjaintenVaihtojaJaljellaChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // ei tarvita
        }
        #endregion

        #region Väriasetukset
        /// <summary>
        /// DependencyProperty jossa säilytetään tietoa Aktiivisen Kirjainlaatan taustaväristä
        /// </summary>
        public static readonly DependencyProperty ActiveKirjainlaatanTaustavariProperty =
            DependencyProperty.Register("ActiveKirjainlaatanTaustavari", typeof(SolidColorBrush), typeof(MainWindow),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.LightBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnActiveKirjainlaatanTaustavariChanged)));

        public SolidColorBrush ActiveKirjainlaatanTaustavari
        {
            get { return (SolidColorBrush)GetValue(ActiveKirjainlaatanTaustavariProperty); }
            set { SetValue(ActiveKirjainlaatanTaustavariProperty, value); }
        }

        private static void OnActiveKirjainlaatanTaustavariChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            varit[0] = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// DependencyProperty jossa säilytetään tietoa Aktiivisen Kirjainlaatan reunuksen väristä
        /// </summary>
        public static readonly DependencyProperty ActiveKirjainlaatanReunusvariProperty =
            DependencyProperty.Register("ActiveKirjainlaatanReunusvari", typeof(SolidColorBrush), typeof(MainWindow),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SteelBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnActiveKirjainlaatanReunusvariChanged)));

        public SolidColorBrush ActiveKirjainlaatanReunusvari
        {
            get { return (SolidColorBrush)GetValue(ActiveKirjainlaatanReunusvariProperty); }
            set { SetValue(ActiveKirjainlaatanReunusvariProperty, value); }
        }

        private static void OnActiveKirjainlaatanReunusvariChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            varit[1] = (SolidColorBrush)args.NewValue;
        }


        /// <summary>
        /// DependencyProperty jossa säilytetään tietoa epäaktiivisen Kirjainlaatan taustaväristä
        /// </summary>
        public static readonly DependencyProperty InactiveKirjainlaatanTaustavariProperty =
            DependencyProperty.Register("InactiveKirjainlaatanTaustavari", typeof(SolidColorBrush), typeof(MainWindow),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SteelBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnInactiveKirjainlaatanTaustavariChanged)));

        public SolidColorBrush InactiveKirjainlaatanTaustavari
        {
            get { return (SolidColorBrush)GetValue(InactiveKirjainlaatanTaustavariProperty); }
            set { SetValue(InactiveKirjainlaatanTaustavariProperty, value); }
        }

        private static void OnInactiveKirjainlaatanTaustavariChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            varit[2] = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// DependencyProperty jossa säilytetään tietoa Aktiivisen Kirjainlaatan reunuksen väristä
        /// </summary>
        public static readonly DependencyProperty InactiveKirjainlaatanReunusvariProperty =
            DependencyProperty.Register("InactiveKirjainlaatanReunusvari", typeof(SolidColorBrush), typeof(MainWindow),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Goldenrod), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnInactiveKirjainlaatanReunusvariChanged)));

        public SolidColorBrush InactiveKirjainlaatanReunusvari
        {
            get { return (SolidColorBrush)GetValue(InactiveKirjainlaatanReunusvariProperty); }
            set { SetValue(InactiveKirjainlaatanReunusvariProperty, value); }
        }

        private static void OnInactiveKirjainlaatanReunusvariChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            varit[3] = (SolidColorBrush)args.NewValue;
        }
        #endregion

        #endregion

        #region Menutoiminnot

        /// <summary>
        /// Aloittaa uuden pelin
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void MenuItemNewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        /// <summary>
        /// Lopettaa ohjelman
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void MenuItemQuit_Click(object sender, RoutedEventArgs e)
        {
            LopetaPeli();
        }

        /// <summary>
        /// Alustaa ja näyttää AboutDialogin, jossa on tietoja ohjelmasta.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
        {
            NaytaPelinTiedot();
        }

        /// <summary>
        /// Näyttää pelin ohjeet järjestelmän oletuksena olevassa internetselaimessa.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void MenuItemPeliohjeet_Click(object sender, RoutedEventArgs e)
        {
            NaytaAvustus();
        }

        /// <summary>
        /// Avaa pelin asetukset -ikkunan, jossa voi vaihtaa pelin vaikeustasoa,
        /// ulkoasua, kieltä, etc.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void MenuItemSettings_Click(object sender, RoutedEventArgs e)
        {
            NaytaAsetukset();
        }


        #endregion

        #region Routed Event handlers

        #region Aloitusnäytön tapahtumien käsittelijät

        /// <summary>
        /// Käsittelijä kun pelin aloitusnäytössä painetaan 'Uusi peli'-nappia
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void Window_UusiPeli(object sender, RoutedEventArgs e)
        {
            // tismalleen sama kuin jos valikosta valittaisiin
            NewGame();
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä kun pelin aloitusnäytössä painetaan 'Asetukset'-nappia
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void Window_Asetukset(object sender, RoutedEventArgs e)
        {
            // tismalleen sama kuin jos valikoista valittaisiin
            NaytaAsetukset();
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä kun pelin aloitusnäytössä painetaan 'Lopeta peli'-nappia
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void Window_LopetaPeli(object sender, RoutedEventArgs e)
        {
            LopetaPeli();
        }

        /// <summary>
        /// Käsittelijä kun pelin aloitusnäytössä painetaan 'Ohjeet'-nappia
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void Window_NaytaAvustus(object sender, RoutedEventArgs e)
        {
            NaytaAvustus();
            e.Handled = true;
        }

        #endregion

        #region Virhetapahtumien käsittelijät
        /// <summary>
        /// Käsittelijä LaattojenSijoitteluVirheEventia varten. Jos pelilaudalle raahatut Kirjainlaatat eivät ole
        /// sallitussa muodostelmassa (Kirjainlaattoja eri riveillä/eri sarakkeissa), ilmoitetaan pelaajalle 
        /// virheestä MessageBoxilla.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void AnnaLaattojenSijoitteluVirheilmoitus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Laudalle lisättyjen laattojen pitää olla samassa rivissä tai sarakkeessa", "Virhe kirjainlaattojen sijoittelussa");
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä SanaaEiLoytynytEventia varten. Antaa virheilmoituksen MEssageBoxilla
        /// jos laudalle raahatuista Kirjainlaatoista ei muodostunut hyväksyttyä sanaa.
        /// <see cref="Pelilauta.SanaaEiLoytynytEvent"/>
        /// <seealso cref="Pelilauta.HaePisteRiviLaatat"/>
        /// <seealso cref=" Pelilauta.HaePisteSarakeLaatat"/>
        /// </summary>
        /// <param name="sender">Objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void AnnaSanaaEiLoytynytVirheilmoitus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Antamaasi sanaa '" + e.OriginalSource.ToString() + "' ei loytynyt hyvaksyttyjen sanojen listalta!", "Virheellinen sana");
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä SanaEiKiinniAikasemmassaVirheEventia varten. Antaa virheilmoituksen jos
        /// laudalle lisätty sana ei ole kiinni toisessa laudalle lisätyssä sanassa.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void SanaEiKiinniAikaisemmassaVirhe(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Antamasi sanan pitää kiinnittyä laudalla jo olevaan sanaan!", "Sanan sijoitteluvirhe");
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä EnsimmainenSanaEiAloitusruudussaVirheEventia varten. Antaa virheilmoituksen jos
        /// ensimmäinen laudalle lisätty sana ei peitä pelilaudan aloitusruutua.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void EnsimmainenSanaEiAloitusruudussaVirhe(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "Ensimmäisen sanan täytyy peittää aloitusruutu!", "Sana sijoitteluvirhe");
            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Käsittelijä Aikalaskurin AikalaskuriAlarmEventille. Poistaa käsittelijät pelilaudan PelilautaRuutu-
        /// olioista, jolloin pelilaudalle ei voida enää lisätä tai sieltä poistaa Kirjainlaattoja.
        /// Pelaajalle ilmoitetaan pelin päättymisestä ja näytetään pelaajan kokonaispisteet MessageBoxilla.
        /// <see cref="Aikalaskuri.AikalaskuriAlarmEvent"/>
        /// </summary>
        /// <param name="sender">Objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void Window_OnAikalaskuriAlarm(object sender, RoutedEventArgs e)
        {
            PeliLoppui(true);
            e.Handled = true;
        }

        private void pelilauta1_AikalaskuriAlarm(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// Käsittelijä kun laudalle lisätty tyhjä kirjainlaatta ja käyttäjältä pitää kysyä
        /// mitä kirjainta tyhjän laatan halutaan esittävän.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_LisattiinTyhjaLaatta(object sender, RoutedEventArgs e)
        {
            PelilautaRuutu source = (PelilautaRuutu)e.OriginalSource;
            Kirjainlaatta blanco = source.GetKirjainlaatta();
            kirjainValintaWindow.ShowDialog();
            String valittuKirjain = kirjainValintaWindow.GetValittuKirjain();
            blanco.Kirjain = valittuKirjain;
        }

        #endregion

        #region UI Event handlers

        /// <summary>
        /// Käsittelijä pelilaudan Loaded-tapahtumalle. Täytetään pelilauta PelilautaRuuduilla,
        /// täytetään pelaajan käytössä olevat Kirjainlaatat, luetaan hyväksyttyjen sanojen lista
        /// ohjelman käyttöön tekstitiedostosta ja käynnistetään pelin aikalaskuri.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void pelilauta1_Loaded(object sender, RoutedEventArgs e)
        {
            pelilauta1.CreatePelilautaCells();
            TaydennaKirjainlaatat();
            pelilauta1.LueHyvaksytytSanatTiedostosta("sanalista_kotus.txt");
            aikalaskuri1.Start();
        }


        /// <summary>
        /// Käsittelijä Tarkista Sana -napin painallukselle. Käynnistää tarvittavat apumetodit
        /// pelilaudalle lisätyn sanan (= Kirjainlaattojen) tarkistamista ja pistelaskua varten
        /// sekä lisää pelaajan kokonaispisteitä ja aikalaskurin aikaa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTarkistaSana_Click(object sender, RoutedEventArgs e)
        {
            bool kaikkiKaytetty = false;
            if (kirjainlaattaHolder1.LaattojaPaikalla < 1) kaikkiKaytetty = true;
            int pisteet = pelilauta1.LaskePisteet(kaikkiKaytetty);
            //Jos oli hyväksyttyjä sanoja
            if (pisteet > 0)
            {
                pistelaskuri1.IncreasePoints(pisteet);
                aikalaskuri1.IncreaseTime((pisteet * 2));
                TaydennaKirjainlaatat();
            }
            if (TarkistaLoppuikoPeli())
            {
                PeliLoppui(false);
            }
            e.Handled = true;
        }


        /// <summary>
        /// Vaihtaa pelaajan käytössä olevat kirjainlaatat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVaihdaKirjaimet_Click(object sender, RoutedEventArgs e)
        {
            // Jos pelaajalla ei ole vaihtoja jäljellä, ei voida vaihtaa
            if (KirjaintenVaihtojaJaljella < 1) return;
            else 
            {
                KirjaintenVaihtojaJaljella = --KirjaintenVaihtojaJaljella;
                PaivitaVaihtojaJaljella(KirjaintenVaihtojaJaljella);
            }
            // Jos laattoja ei ole pelissä jäljellä, ei voida vaihtaa
            if (kirjainlaattaPussi1.LaattojaJaljella > 0)
            {
                // lista johon kerätään vaihdettat laatat väliaikaisesti
                List<Kirjainlaatta> vaihdettavat = new List<Kirjainlaatta>();
                // kerätään kirjainlaattaholderissa (vasen palkki) olevat laatat
                foreach (Kirjainlaatta k in (kirjainlaattaHolder1.Content as StackPanel).Children)
                {
                    vaihdettavat.Add(k);
                }
                //kerätään pelilaudalle lisätyt laatat joilla ei ole muodostettu sanoja
                vaihdettavat.AddRange(pelilauta1.GetAktiivisetLaudalleLisatyt());

                // Lisätään pelaajan käytössä olevat laatat takaisin kirjainlaattapussiin
                // ja poistetaan ne pelaajan käytöstä kirjainlaattaholderista/pelilaudalta
                foreach (Kirjainlaatta k in vaihdettavat)
                {
                    kirjainlaattaPussi1.PutBack(k);
                    // jos Kirjainlaatta on KirjainlaattaHolderissa
                    if (kirjainlaattaHolder1.Contains(k))
                    {
                        kirjainlaattaHolder1.RemoveObjectFromHolder(k);
                    }
                    // muuten se on pelilaudalla
                    else
                    {
                        pelilauta1.RemoveKirjainlaatta(k);
                    }
                }

                // taydennetaan uudet laatat pelaajan käyttöön
                TaydennaKirjainlaatat();
                // Vähennetään pelaajan pisteitä
                if (pistelaskuri1.TotalPoints > 0)
                {
                    pistelaskuri1.DecreasePoints(10);
                }
                // Tyhjennetään vaihdettavien lista
                vaihdettavat.Clear();
            }
        }

        /// <summary>
        /// Käsittelijä ohjelman Window_Closing tapahtumalle, joka laukaistaan ohjelmaa lopetettaessa.
        /// Pitää huolen siitä että AboutDialogia varten tarvittu Window suljetaan, jotta pääohjelma voi
        /// sulkeutua ongelmitta.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">CancelEvent argumentit, ei tarvita</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(this, "Haluatko sulkea ohjelman?", "Lopeta ohjelma", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) e.Cancel = true;
            else
            {
                //aboutWindow.Close();
                Application.Current.Shutdown();
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Tarkistaa onko pelissä jäljellä Kirjainlaattoja käytettäväksi
        /// </summary>
        /// <returns>tosi jos ei enää laattoja käytettäväksi, muuten false</returns>
        private Boolean TarkistaLoppuikoPeli()
        {
            if (kirjainlaattaPussi1.LaattojaJaljella <= 0 && kirjainlaattaHolder1.LaattojaPaikalla <= 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Vaihtaa Kirjainlaattojen värit
        /// </summary>
        private static void VaihdaKirjainlaattojenVarit(List<UIElement> komponentit, List<SolidColorBrush> varit)
        {
            if (komponentit == null) return;
            Pelilauta lauta = (Pelilauta)komponentit[0];
            KirjainlaattaHolder holder = (KirjainlaattaHolder)komponentit[1];
            KirjainlaattaPussi pussi = (KirjainlaattaPussi)komponentit[2];

            // pelilaudalla olevat
            foreach (Kirjainlaatta k in lauta.GetLaudalleLisatyt())
            {
                k.ActiveFillColor = varit[0];
                k.ActiveBorderColor = varit[1];
                k.InactiveFillColor = varit[2];
                k.InactiveBorderColor = varit[3];
            }
            // KirjainlaattaHolderissa olevat
            foreach (Kirjainlaatta k in holder.GetKirjainlaatat())
            {
                k.ActiveFillColor = varit[0];
                k.ActiveBorderColor = varit[1];
                k.InactiveFillColor = varit[2];
                k.InactiveBorderColor = varit[3];
            }
            // KirjainlaattaPussissa olevat
            foreach (Kirjainlaatta k in pussi.GetKirjainlaatat())
            {
                k.ActiveFillColor = varit[0];
                k.ActiveBorderColor = varit[1];
                k.InactiveFillColor = varit[2];
                k.InactiveBorderColor = varit[3];
            }
        }


        /// <summary>
        /// Palauttaa listan komponenteista jotka sisältävät Kirjainlaattoja
        /// </summary>
        /// <param name="mainWindow">ohjelman pääikkuna</param>
        /// <returns>lista Kirjainlaattoja sisältävistä komponenteista</returns>
        private static List<UIElement> EtsiKirjainlaattojaSisaltavatKomponentit(DependencyObject mainWindow)
        {
            MainWindow main = (MainWindow)mainWindow;
            StackPanel main_content_area = (StackPanel)main.Content;
            //Jos ei olla vielä pelin puolella, ei tarvitse käydä kaikki laattoja läpi
            if (main_content_area.Children[1].GetType() == typeof(DockPanel))
            {
                // vähän purkkaratkaisu, pitäisi olla fiksumpi
                DockPanel dpanel = (DockPanel)main_content_area.Children[1];
                Pelilauta lauta = (Pelilauta)dpanel.Children[2];
                KirjainlaattaHolder holder = (KirjainlaattaHolder)((StackPanel)dpanel.Children[0]).Children[1];
                KirjainlaattaPussi pussi = (KirjainlaattaPussi)((StackPanel)dpanel.Children[1]).Children[5];

                List<UIElement> komponentit = new List<UIElement>();
                komponentit.Add(lauta);
                komponentit.Add(holder);
                komponentit.Add(pussi);
                return komponentit;
            }
            return null;
        }

        /// <summary>
        /// Lukee ohjelman ja käyttäjän asetukset ohjelman käyttöön
        /// </summary>
        private void LueAsetukset()
        {
            programTitle = Properties.Settings.Default.OhjelmanNimi;
            programAuthor = Properties.Settings.Default.TekijanNimi;
            programVersion = Properties.Settings.Default.Versio;
            programVersionDate = Properties.Settings.Default.VersioPvm;
            Vaikeusaste = Properties.Settings.Default.Vaikeustaso;
            ActiveKirjainlaatanTaustavari = Properties.Settings.Default.ActiveKirjainlaatanTaustaVari;
            ActiveKirjainlaatanReunusvari = Properties.Settings.Default.ActiveKirjainlaatanReunusvari;
            InactiveKirjainlaatanTaustavari = Properties.Settings.Default.InactiveKirjainlaatanTaustavari;
            InactiveKirjainlaatanReunusvari = Properties.Settings.Default.InactiveKirjainlaatanReunusvari;
        }

        /// <summary>
        /// Tallentaa käyttäjän asetukset
        /// </summary>
        private void TallennaAsetukset()
        {
            Properties.Settings.Default.Vaikeustaso = Vaikeusaste;
            Properties.Settings.Default.ActiveKirjainlaatanTaustaVari = ActiveKirjainlaatanTaustavari;
            Properties.Settings.Default.ActiveKirjainlaatanReunusvari = ActiveKirjainlaatanReunusvari;
            Properties.Settings.Default.InactiveKirjainlaatanTaustavari = InactiveKirjainlaatanTaustavari;
            Properties.Settings.Default.InactiveKirjainlaatanReunusvari = InactiveKirjainlaatanReunusvari;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Täyttää pelaajan käytettävissä olevat Kirjainlaatat KirjainlaattaHolderiin KirjainlaattaPussista
        /// </summary>
        private void TaydennaKirjainlaatat()
        {
            for (int i = kirjainlaattaHolder1.MaxLukumaara - kirjainlaattaHolder1.LaattojaPaikalla; i > 0; i--)
            {
                Kirjainlaatta k;
                if ((k = kirjainlaattaPussi1.GetKirjainlaatta(0)) != null)
                {
                    kirjainlaattaHolder1.AddObjectToHolder(k);
                    kirjainlaattaPussi1.Remove(0);
                }
            }
        }

        /// <summary>
        /// Päivittää tekstin jolla näytetään pelaajalle jäljellä olevien kirjainten vaihtojen määrä
        /// </summary>
        /// <param name="vaihtoja">kuinka monta kertaa voidaan vielä vaihtaa kirjaimet</param>
        private void PaivitaVaihtojaJaljella(int vaihtoja)
        {
            txtVaihtojaJaljella.Text = "Vaihtoja jäljellä: " + vaihtoja.ToString();
        }

        #endregion

        #region Pelin toimintojen apumetodit

        /// <summary>
        /// Aloittaa uuden pelin, eli alustaa alustaa pelin komponentit.
        /// </summary>
        private void NewGame()
        {
            // Tyhjennetään pelialue
            if (window_content.Children.Count > 1)
            {
                window_content.Children.Remove(aloitusnaytto);
                window_content.Children.Remove(window_dockpanel);
            }
            window_dockpanel = new DockPanel();
            if (kirjainlaattaPussi1 != null) kirjainlaattaPussi1.Clear();
            window_dockpanel.Children.Clear();
            int vaikeusaste = VAIKEUSASTE[Vaikeusaste];
            KirjaintenVaihtojaJaljella = ((vaikeusaste / 60) + 2);
            
            peliControlsPanel = new StackPanel();
            DockPanel.SetDock(peliControlsPanel, Dock.Left);
            peliControlsPanel.Width = 80;
            peliControlsPanel.Margin = new Thickness(4);
            peliControlsPanel.MaxWidth = 100;
            peliControlsPanel.Background = new SolidColorBrush(Colors.Gainsboro);

            lblLaatatKaytossa = new Label();
            lblLaatatKaytossa.Content = "Kirjaimet:";
            lblLaatatKaytossa.FontSize = 14;
            lblLaatatKaytossa.Width = 80;
            lblLaatatKaytossa.MaxWidth = 100;
            //lblLaatatKaytossa.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            //lblLaatatKaytossa.BorderThickness = new Thickness(2);

            kirjainlaattaHolder1 = new KirjainlaattaHolder();
            kirjainlaattaHolder1.Name = "Holder";
            kirjainlaattaHolder1.MaxLukumaaraModifiable = true;
            kirjainlaattaHolder1.MaxLukumaara = 7;
            kirjainlaattaHolder1.AllowDrop = true;
            kirjainlaattaHolder1.MinHeight = 350;
            kirjainlaattaHolder1.Width = 62;
            kirjainlaattaHolder1.Margin = new Thickness(2);
            kirjainlaattaHolder1.Padding = new Thickness(2);
            kirjainlaattaHolder1.BorderBrush = new SolidColorBrush(Colors.SlateGray);
            kirjainlaattaHolder1.BorderThickness = new Thickness(4);

            buttonVaihdaKirjaimet = new Button();
            buttonVaihdaKirjaimet.Content = "_Vaihda";
            buttonVaihdaKirjaimet.Click += buttonVaihdaKirjaimet_Click;
            buttonVaihdaKirjaimet.Width = 62;
            buttonVaihdaKirjaimet.Height = 44;
            buttonVaihdaKirjaimet.Margin = new Thickness(2, 10, 2, 10);
            txtVaihtojaJaljella = new TextBlock();
            txtVaihtojaJaljella.Text = "Vaihtoja jäljellä: " + KirjaintenVaihtojaJaljella;
            txtVaihtojaJaljella.FontSize = 14;
            txtVaihtojaJaljella.TextWrapping = TextWrapping.Wrap;
            txtVaihtojaJaljella.Margin = new Thickness(10, 10, 2, 10);

            peliControlsPanel.Children.Add(lblLaatatKaytossa);
            peliControlsPanel.Children.Add(kirjainlaattaHolder1);
            peliControlsPanel.Children.Add(buttonVaihdaKirjaimet);
            peliControlsPanel.Children.Add(txtVaihtojaJaljella);

            pelaajaTiedotPanel = new StackPanel();
            DockPanel.SetDock(pelaajaTiedotPanel, Dock.Right);
            pelaajaTiedotPanel.Width = 110;
            pelaajaTiedotPanel.Background = new SolidColorBrush(Colors.Gainsboro);
            pelaajaTiedotPanel.Margin = new Thickness(4);

            lblAika = new Label();
            lblAika.Content = "Aika:";
            lblAika.FontSize = 14;
            aikalaskuri1 = new Aikalaskuri(vaikeusaste);
            aikalaskuri1.IsDescendingTimer = true;
            lblPisteet = new Label();
            lblPisteet.Content = "Pisteet:";
            lblPisteet.FontSize = 14;
            pistelaskuri1 = new Pistelaskuri();
            pistelaskuri1.TotalPoints = 0;
            lblLaattojaJaljella = new Label();
            lblLaattojaJaljella.Content = "Laattoja jäljellä:";
            lblLaattojaJaljella.FontSize = 14;
            kirjainlaattaPussi1 = new KirjainlaattaPussi(varit);
            kirjainlaattaPussi1.Name = "Pussi";
            buttonLaskePisteet = new Button();
            buttonLaskePisteet.Content = "_Tarkista sana";
            buttonLaskePisteet.Margin = new Thickness(2, 10, 2, 10);
            buttonLaskePisteet.Height = 23;
            buttonLaskePisteet.Width = 100;
            buttonLaskePisteet.Click += buttonTarkistaSana_Click;

            pelaajaTiedotPanel.Children.Add(lblAika);
            pelaajaTiedotPanel.Children.Add(aikalaskuri1);
            pelaajaTiedotPanel.Children.Add(lblPisteet);
            pelaajaTiedotPanel.Children.Add(pistelaskuri1);
            pelaajaTiedotPanel.Children.Add(lblLaattojaJaljella);
            pelaajaTiedotPanel.Children.Add(kirjainlaattaPussi1);
            pelaajaTiedotPanel.Children.Add(buttonLaskePisteet);

            pelilauta1 = new Pelilauta();
            pelilauta1.Name = "Pelilauta";
            pelilauta1.Margin = new Thickness(4);
            pelilauta1.Loaded += pelilauta1_Loaded;
            aikalaskuri1.AikalaskuriAlarm += pelilauta1_AikalaskuriAlarm;

            window_dockpanel.LastChildFill = true;
            window_dockpanel.Children.Add(peliControlsPanel);
            window_dockpanel.Children.Add(pelaajaTiedotPanel);
            window_dockpanel.Children.Add(pelilauta1);
            window_dockpanel.Background = new SolidColorBrush(Colors.SteelBlue);

            window_content.Children.Add(window_dockpanel);
        }

        /// <summary>
        /// Tekee tarvittavat toimenpiteet kun peli loppuu: deaktivoi tarvittavat kontrollit,
        /// pysäyttää ajastimen, ilmoittaa pelin loppumisesta ja näyttää pelaajan pisteet.
        /// </summary>
        /// <param name="aikaLoppui">parametri joka kertoo, päättyikö peli ajan loppumiseen
        /// vai saiko pelaaja kaikki pelin Kirjainlaatat käytettyä</param>
        private void PeliLoppui(bool aikaLoppui)
        {
            this.buttonLaskePisteet.IsEnabled = false;
            this.buttonVaihdaKirjaimet.IsEnabled = false;
            if (aikaLoppui)
            {
                MessageBox.Show(this, "Aika loppui!\n Sait " + pistelaskuri1.TotalPoints + " pistettä!", "Aika loppu");
            }
            else
            {
                MessageBox.Show(this, "Onneksi olkoon!\nPääsit pelin läpi ja\nsait " + pistelaskuri1.TotalPoints + " pistettä!", "Peli päättyi");
            }
            pelilauta1.RemovePelilautaRuutuHandlers();
        }

        /// <summary>
        /// Lopettaa pelin eli sulkee ohjelman
        /// </summary>
        private void LopetaPeli()
        {
            // riittää kun yritetään sulkea ohjelman pääikkuna,
            // pääikkunan Closing-tapahtuma huolehtii lopuista
            this.Close();
        }

        /// <summary>
        /// Avaa pelin Asetukset-ikkunan, jossa pelin asetuksia voi muuttaa
        /// </summary>
        private void NaytaAsetukset()
        {
            settingsWindow = new SettingsDialog.SettingsDialog();
            settingsWindow.SetAsetukset(varit, Vaikeusaste);
            var result = settingsWindow.ShowDialog();
            
            if (result.HasValue)
            {
                if (result.Equals(true))
                {
                    if (settingsWindow.GetVaikeusaste() != Vaikeusaste)
                    {
                        MessageBox.Show("Uusi vaikeusaste astuu voimaan vasta kun aloitat uuden pelin.", "Vaikeusasteen muutos", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    Vaikeusaste = settingsWindow.GetVaikeusaste();
                    InactiveKirjainlaatanTaustavari = settingsWindow.GetInactiveLaatanTaustaVari();
                    InactiveKirjainlaatanReunusvari = settingsWindow.GetInactiveLaatanReunusVari();
                    ActiveKirjainlaatanTaustavari = settingsWindow.GetActiveLaatanTaustaVari();
                    ActiveKirjainlaatanReunusvari = settingsWindow.GetActiveLaatanReunusVari();
                    TallennaAsetukset();

                    varit.Clear();
                    varit.Add(ActiveKirjainlaatanTaustavari);
                    varit.Add(ActiveKirjainlaatanReunusvari);
                    varit.Add(InactiveKirjainlaatanTaustavari);
                    varit.Add(InactiveKirjainlaatanReunusvari);
                    VaihdaKirjainlaattojenVarit(EtsiKirjainlaattojaSisaltavatKomponentit(this), varit);
                }
            }
        }

        /// <summary>
        /// Avaa ikkunan josta voi tarkastella pelin perustietoja
        /// </summary>
        private void NaytaPelinTiedot()
        {
            aboutWindow.ShowDialog();
        }

        /// <summary>
        /// Näyttää pelin ohjeistuksen järjestelmän oletuksena olevassa internetselaimessa.
        /// </summary>
        private void NaytaAvustus()
        {
            System.Diagnostics.Process.Start("http://users.jyu.fi/~samantvi/gko2012/peliohjeet/");
        }

        #endregion

    }
}
