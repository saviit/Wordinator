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
using System.Windows.Threading;

namespace GameComponents
{
    /// <summary>
    /// Interaction logic for PelilautaRuutu.xaml
    /// </summary>
    public partial class PelilautaRuutu : UserControl
    {
        #region Private fields
        /// <summary>
        /// Ajastin PleilautaRuutujen läpinäkyvyyden 'animointia' varten
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer();
        /// <summary>
        /// SolidColorBrush, johon tallennetaan kunkin ruudun alkuperäinen
        /// reunusväri. Tarvitaan jotta voidaan palauttaa ruutu alkuperäisen
        /// väriseksi ruudun korostamisen jälkeen.
        /// </summary>
        private SolidColorBrush originalBorderColor;

        /// <summary>
        /// Taulukko, josta saadaan kunkin PelilautaRuudun tyyppi ja vastaavaa
        /// tyyppiä edustava kirjainyhdistelmä
        /// </summary>
        private string[,] _pisteKerroinTyypit = new string[6, 2] { 
                          {"0", ""},   //tyhjä solu
                          {"1", "DL"}, // tuplakirjainpisteet (DL, double letter)
                          {"2", "TL"}, // triplakirjainpisteet (TL, triple letter)
                          {"3", "DW"}, // tuplasanapisteet (DW, double word)
                          {"4", "TW"}, // triplasanapisteet (TW, triple word)
                          {"5", "*"}}; // tuplasanapisteet (keskiruutu, star)
        /// <summary>
        /// Taulukko väreistä joilla kunkin ruudun tyyppiä edustava kirjainyhdistelmä
        /// esitetään PelilautaRuudussa.
        /// </summary>
        private Color[] _varit = new Color[]{Colors.DarkGray, //tyhjä
                                                   Colors.DodgerBlue,  // DL
                                                   Colors.LimeGreen,   // TL
                                                   Colors.Salmon,      // DW
                                                   Colors.Red,         // TW
                                                   Colors.Purple       // Centre piece
                                                   };
        #endregion

        #region Dependency Properties
        /// <summary>
        /// Dependency Property, jossa PelilautaRuudun pistekerroin säilytetään
        /// </summary>
        public static readonly DependencyProperty PistekerroinProperty =
            DependencyProperty.Register("Pistekerroin", typeof(int), typeof(PelilautaRuutu));
        /// <summary>
        /// Dependency Property, jossa PelilautaRuudun pistekertoimen tyyppi säilytetään
        /// </summary>
        public static readonly DependencyProperty PistekerroinTyyppiProperty =
            DependencyProperty.Register("PistekerroinTyyppi", typeof(int), typeof(PelilautaRuutu));
        /// <summary>
        /// Dependency Property, jossa säilytetään tieto siitä onko PelilautaRuudussa Kirjainlaatta
        /// </summary>
        public static readonly DependencyProperty RuutuSisaltaaLaatanProperty =
            DependencyProperty.Register("RuutuSisaltaaLaatan", typeof(Boolean), typeof(PelilautaRuutu),
            new FrameworkPropertyMetadata(false));
        /// <summary>
        /// Dependency Property, jossa säilytetään tieto siitä missä kohti Pelilaudan Canvasta PelilautaRuudun
        /// kuuluu olla (Canvas.Left)
        /// </summary>
        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.Register("PositionX", typeof(int), typeof(PelilautaRuutu));
        /// <summary>
        /// Dependency Property, jossa säilytetään tieto siitä missä kohti Pelilaudan Canvasta PelilautaRuudun
        /// kuuluu olla (Canvas.Top)
        /// </summary>
        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.Register("PositionY", typeof(int), typeof(PelilautaRuutu));
        /// <summary>
        /// Dependency Property, jossa säilytetään tieto siitä, onko PelilautaRuudusta saatavaa
        /// pistebonusta/pistekerrointa jo käytetty pistelaskennassa.
        /// </summary>
        public static readonly DependencyProperty PistekerroinKaytettyProperty =
            DependencyProperty.Register("PistekerroinKaytetty", typeof(Boolean), typeof(PelilautaRuutu),
            new FrameworkPropertyMetadata(false));


        #region Dependency property getters/setters

        public Boolean DragActionCanContinue { get; set; }
        public Boolean PistekerroinKaytetty { get; set; }
        public int Pistekerroin { get; set; }
        public int PistekerroinTyyppi { get; set; }
        public Boolean RuutuSisaltaaLaatan
        {
            get { return (Boolean)GetValue(RuutuSisaltaaLaatanProperty);}
            set { SetValue(RuutuSisaltaaLaatanProperty, value); } 
        }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        
        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Oletusmuodostaja
        /// </summary>
        /// <remarks>Ei käytetä.</remarks>
        public PelilautaRuutu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostajametodi Pelilaudan PelilautaRuudulle
        /// </summary>
        /// <param name="tyyppi">PelilautaRuudun tyyppi</param>
        /// <param name="x">X-koordinaatti, joka määrää ruudun paikan Canvaksella (Canvas.Left)</param>
        /// <param name="y">Y-koordinaatti, joka määrää ruudun paikan Canvaksella (Canvas.Top)</param>
        public PelilautaRuutu(int tyyppi, int x, int y)
        {
            InitializeComponent();
            if (tyyppi > 5 || tyyppi < 0) return;
            this.PistekerroinTyyppi = tyyppi;

            // Määrätään PelilautaRuudun pistekerroin ruudun tyypin mukaan
            if (tyyppi == 0) { this.Pistekerroin = 1; }
            if (tyyppi == 1 || tyyppi == 3) { this.Pistekerroin = 2; }
            if (tyyppi == 2 || tyyppi == 4) { this.Pistekerroin = 3; }
            ruutu_content.Foreground = new SolidColorBrush(_varit[tyyppi]);

            // keskimmäisen ruudun muotoilu vähän erikoisempi
            if (tyyppi == 5)
            {
                this.Pistekerroin = 2;
                ruutu_content.Foreground = new SolidColorBrush(Colors.Yellow);
                ruutu_content.FontSize = 62;
                border.Background = new SolidColorBrush(_varit[tyyppi]);
            }

            ruutu_content.Content = "" + _pisteKerroinTyypit[tyyppi, 1].ToString();
            border.BorderBrush = new SolidColorBrush(_varit[tyyppi]);
            this.PositionX = x;
            this.PositionY = y;
            this.DragActionCanContinue = true;
            this.RuutuSisaltaaLaatan = false;
            // Asetetaan käsittelijät ruudun tapahtumille
            this.DragEnter += new DragEventHandler(PelilautaRuutu_DragEnter);
            this.DragLeave += new DragEventHandler(PelilautaRuutu_DragLeave);
            this.Drop += new DragEventHandler(PelilautaRuutu_Drop);
        }

        #endregion

        #region Routed Events

        /// <summary>
        /// Routed Event jolla ilmoitetaan loogisessa puussa ylemmäs että lisätty Kirjainlaatta
        /// oli tyhjä laatta ('wildcard') jotta osataan kysyä käyttäjältä mitä kirjainta laatalla
        /// on tarkoistus esittää
        /// </summary>
        public static readonly RoutedEvent LisattiinTyhjaLaattaEvent =
            EventManager.RegisterRoutedEvent("LisattiinTyhjaLaatta", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(PelilautaRuutu));

        public event RoutedEventHandler LisattiinTyhjaLaatta
        {
            add { AddHandler(LisattiinTyhjaLaattaEvent, value); }
            remove { RemoveHandler(LisattiinTyhjaLaattaEvent, value); }
        }

        void RaiseLisattiinTyhjaLaattaEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PelilautaRuutu.LisattiinTyhjaLaattaEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Drag and Drop event handlers
        /// <summary>
        /// Käsittelijä Pelilaudan ruutujen Drop-tapahtumille
        /// </summary>
        /// <param name="sender">PelilautaRuutu joka laukaisi tapahtuman</param>
        /// <param name="e">Raahaustapahtuman argumentit, käytännössä data (pelilaudalle pudotettava
        ///  Kirjainlaatta) jota kuljetettiin</param>
        void PelilautaRuutu_Drop(object sender, DragEventArgs e)
        {
            this.border.BorderBrush = originalBorderColor;
            Kirjainlaatta laatta = e.Data.GetData(typeof(Kirjainlaatta)) as Kirjainlaatta;

            StackPanel parent = laatta.Parent as StackPanel;
            if (((PelilautaRuutu)sender).RuutuSisaltaaLaatan != true)
            {
                if (parent.Parent is KirjainlaattaHolder)
                {
                    (parent.Parent as KirjainlaattaHolder).RemoveObjectFromHolder(laatta);
                }
                else
                {
                    parent.Children.Remove(laatta);
                }
                laatta.PositionX = this.PositionX;
                laatta.PositionY = this.PositionY;
                //this.KirjainlaattaContainer = laatta;
                this.RuutuSisaltaaLaatan = true;
                this.kirjainlaattaContainer.Children.Add(laatta);

                // Jos lisätty laatta on 'wildcard' laatta eli tyhjä laatta
                // aiheutetaan routed event jolla kerrotaan pääohjelmalle
                // että käyttäjältä pitää kysyä mitä kirjainta tyhjällä laatalla
                // halutaan esittää
                if (laatta.Pistearvo == 0) RaiseLisattiinTyhjaLaattaEvent();
            }
            else
            {
                // Jos ruudussa on jo Kirjainlaatta, sen ruudun päälle ei voi raahata
                e.Effects = DragDropEffects.None;
            }
            laatta = null;
            e.Handled = true;
        }

        /// <summary>
        /// Käsittelijä PelilautaRuudun DragLeave-tapahtumalle. Poistaa korostuksen ruudusta
        /// kun hiirellä raahattavana oleva objekti poistuu ruudun alueelta.
        /// </summary>
        /// <param name="sender">PelilautaRuutu joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        void PelilautaRuutu_DragLeave(object sender, DragEventArgs e)
        {
            this.border.BorderBrush = originalBorderColor;
        }

        /// <summary>
        /// Käsittelijä PelilautaRuudun DragEnter-tapahtumalle. Tapahtuu kun hiirellä raahattavana
        /// oleva objekti tulee PelirautaRuudun päälle. Asettaa raahauksen kohteena olevaan Pelilautaruutuun
        /// korostusvärin, jotta käyttäjän on helpompi nähdä mihin ruutuun laattaa ollaan raahaamassa.
        /// </summary>
        /// <param name="sender">PelilautaRuutu joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        void PelilautaRuutu_DragEnter(object sender, DragEventArgs e)
        {
            this.originalBorderColor = this.border.BorderBrush as SolidColorBrush;
            this.border.BorderBrush = new SolidColorBrush(Colors.Pink);
        }

        /// <summary>
        /// Poistaa tapahtumankäsittelijät PelilautaRuudulta. Käytetään kun peli loppuu
        /// eikä haluta että käyttäjä voi vielä muokata peliä ja aiheuttaa ongelmatilanteen.
        /// </summary>
        public void RemoveHandlers()
        {
            this.Drop -= PelilautaRuutu_Drop;
            this.DragEnter -= PelilautaRuutu_DragEnter;
            this.DragLeave -= PelilautaRuutu_DragLeave;
        }


        #endregion

        #region Timed 'animations'

        /// <summary>
        /// Käsittelijä PelilautaRuudun Loaded-tapahtumalle. Tapahtuu kun ruutu on
        /// alustettu ja lisätty Pelilaudalle. Asettaa ruudun läpinäkyväksi ja alustaa
        /// 'animaatio'ajastimen ominaisuudet halutuiksi.
        /// </summary>
        /// <param name="sender">Objekti joka laukaisi tapahtuman (PelilautaRuutu) </param>
        /// <param name="e">tapahtuman argumentit</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UIElement element = sender as UIElement;
            element.Opacity = 0;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += new EventHandler(timer_Tick);
            timer.IsEnabled = true;
        }

        /// <summary>
        /// Käsittelijä 'animaatio'ajastimen Tick-tapahtumalle. Laukaistaan ajastimelle
        /// asetetun intervallin (100 ms) välein. Muuttaa PelilautaRuudun läpinäkyvyyttä
        /// näkyvämmäksi pienin askelein kunnes ruutu ei ole ollenkaan läpinäkyvä.
        /// </summary>
        /// <param name="sender">Objekti joka laukaisee tapahtuman (ajastin)</param>
        /// <param name="e">tapahtuman argumentit</param>
        void timer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1.0)
            {
                this.Opacity += 0.1;
            }
            else
            {
                timer.Stop();
                timer.IsEnabled = false;
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Palauttaa viitteen ruudussa sijaitsevaan kirjainlaattaan
        /// </summary>
        /// <returns>Kirjainlaatta tai null jos ruudussa ei ole kirjainlaattaa</returns>
        public Kirjainlaatta GetKirjainlaatta()
        {
            if (this.kirjainlaattaContainer.Children.Count > 0)
            {
                return (Kirjainlaatta)this.kirjainlaattaContainer.Children[0];
            }
            else return null;
        }

        /// <summary>
        /// Poistaa Kirjainlaatan pelilautaruudusta
        /// </summary>
        public void RemoveKirjainlaatta()
        {
            this.kirjainlaattaContainer.Children.Clear();
            this.RuutuSisaltaaLaatan = false;
        }

        #endregion

        #region Routed Event handlers


        /// <summary>
        /// Käsittelijä KirjainlaatanPaikkaVaihtunut -tapahtumalle 
        /// <see cref="Kirjainlaatta.KirjainlaatanPaikkaVaihtunutEvent"/>.
        /// Tällä varmistetaan, että PelilautaRuutu, josta Kirjainlaatta on siirretty pois,
        /// ei osallistu pistelaskentaan.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">tapahtuman argumentit</param>
        private void peliruutu_kirjainlaatanPaikkaVaihtunut(object sender, RoutedEventArgs e)
        {
            this.RuutuSisaltaaLaatan = false;
            e.Handled = true;
        }


        #endregion
    }
}
