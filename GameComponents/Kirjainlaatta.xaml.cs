using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GameComponents
{
    /// <summary>
    /// Interaction logic for Kirjainlaatta.xaml
    /// </summary>
    public partial class Kirjainlaatta : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// Dependency Property jossa säilytetään tieto Kirjainlaatan sisältämästä kirjaimesta
        /// </summary>
        public static readonly DependencyProperty KirjainProperty =
            DependencyProperty.Register("Kirjain", typeof(string), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnKirjainChanged)));
        /// <summary>
        /// Dependency Property jossa säilytetään tieto Kirjainlaatan pistearvosta
        /// </summary>
        public static readonly DependencyProperty PistearvoProperty =
            DependencyProperty.Register("Pistearvo", typeof(int), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,
                OnPistearvoChanged));
        /// <summary>
        /// Dependency Property jossa säilytetään tieto siitä, onko Kirjainlaatta aktiivinen,
        /// ts. onko Kirjainlaatta raahattu Pelilaudalle tällä vuorolla ja voidaanko sitä vielä
        /// liikuttaa
        /// </summary>
        public static readonly DependencyProperty AktiivinenProperty =
            DependencyProperty.Register("Aktiivinen", typeof(Boolean), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(OnAktiivinenChanged)));
        /// <summary>
        /// Dependency Property jossa säilytetään tieto Kirjainlaatan sijainnista Pelilaudalla
        /// </summary>
        /// <remarks>Tämän propertyn arvo muuttuu kun Kirjainlaatta raahataan PelilautaRuudusta toiseen.</remarks>
        public static readonly DependencyProperty PositionXProperty =
            DependencyProperty.Register("PositionX", typeof(int), typeof(Kirjainlaatta));
        /// <summary>
        /// Dependency Property jossa säilytetään tieto Kirjainlaatan sijainnista Pelilaudalla
        /// </summary>
        /// <remarks>Tämän propertyn arvo muuttuu kun Kirjainlaatta raahataan PelilautaRuudusta toiseen.</remarks>
        public static readonly DependencyProperty PositionYProperty =
            DependencyProperty.Register("PositionY", typeof(int), typeof(Kirjainlaatta));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää Kirjainlaatan reunuksen väri
        /// </summary>
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SteelBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnBorderColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää Kirjainlaatan täyttöväri
        /// </summary>
        public static readonly DependencyProperty FillColorProperty =
            DependencyProperty.Register("FillColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.LightBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnFillColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää Kirjainlaatan tekstin väri
        /// </summary>
        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register("TextColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnTextColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää aktiivisen Kirjainlaatan reunuksen väri
        /// </summary>
        public static readonly DependencyProperty ActiveBorderColorProperty =
            DependencyProperty.Register("ActiveBorderColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SteelBlue), FrameworkPropertyMetadataOptions.AffectsRender, 
                new PropertyChangedCallback(OnActiveBorderColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää aktiivisen Kirjainlaatan täyttöväri
        /// </summary>
        public static readonly DependencyProperty ActiveFillColorProperty =
            DependencyProperty.Register("ActiveFillColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.LightBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnActiveFillColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää aktiivisen Kirjainlaatan tekstin väri
        /// </summary>
        public static readonly DependencyProperty ActiveTextColorProperty =
            DependencyProperty.Register("ActiveTextColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnActiveTextColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää epäaktiivisen Kirjainlaatan reunuksen väri
        /// </summary>
        public static readonly DependencyProperty InactiveBorderColorProperty =
            DependencyProperty.Register("InactiveBorderColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Goldenrod), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnInactiveBorderColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää epäaktiivisen Kirjainlaatan täyttöväri
        /// </summary>
        public static readonly DependencyProperty InactiveFillColorProperty =
            DependencyProperty.Register("InactiveFillColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.SteelBlue), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnInactiveFillColorChanged)));
        /// <summary>
        /// DependencyProperty jolla voidaan määrittää epäaktiivisen Kirjainlaatan tekstin väri
        /// </summary>
        public static readonly DependencyProperty InactiveTextColorProperty =
            DependencyProperty.Register("InactiveTextColor", typeof(SolidColorBrush), typeof(Kirjainlaatta),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnInactiveTextColorChanged)));

        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public string Kirjain
        {
            get { return (string)GetValue(KirjainProperty); }
            set { SetValue(KirjainProperty, value); }
        }
        public int Pistearvo { 
            get { return (int)GetValue(PistearvoProperty);} 
            set { SetValue(PistearvoProperty, value);} 
        }
        public Boolean Aktiivinen 
        {
            get { return (Boolean)GetValue(AktiivinenProperty);}
            set { SetValue(AktiivinenProperty, value);}
        }
        public SolidColorBrush BorderColor
        {
            get { return (SolidColorBrush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }
        public SolidColorBrush FillColor
        {
            get { return (SolidColorBrush)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }
        public SolidColorBrush TextColor
        {
            get { return (SolidColorBrush)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }
        public SolidColorBrush ActiveBorderColor
        {
            get { return (SolidColorBrush)GetValue(ActiveBorderColorProperty); }
            set { SetValue(ActiveBorderColorProperty, value); }
        }
        public SolidColorBrush ActiveFillColor
        {
            get { return (SolidColorBrush)GetValue(ActiveFillColorProperty); }
            set { SetValue(ActiveFillColorProperty, value); }
        }
        public SolidColorBrush ActiveTextColor
        {
            get { return (SolidColorBrush)GetValue(ActiveTextColorProperty); }
            set { SetValue(ActiveTextColorProperty, value); }
        }
        public SolidColorBrush InactiveBorderColor
        {
            get { return (SolidColorBrush)GetValue(InactiveBorderColorProperty); }
            set { SetValue(InactiveBorderColorProperty, value); }
        }
        public SolidColorBrush InactiveFillColor
        {
            get { return (SolidColorBrush)GetValue(InactiveFillColorProperty); }
            set { SetValue(InactiveFillColorProperty, value); }
        }
        public SolidColorBrush InactiveTextColor
        {
            get { return (SolidColorBrush)GetValue(InactiveTextColorProperty); }
            set { SetValue(InactiveTextColorProperty, value); }
        }

        #endregion

        #region Dependency Properties value change handlers

        /// <summary>
        /// Käsittelijä Kirjainlaatan Aktiivinen-propertyn arvon muutoksille. Vaihdetaan Kirjainlaatan taustaväriä
        /// sen mukaan, mikä Kirjainlaatan 'status' on.
        /// </summary>
        /// <param name="obj">Objekti jossa propertyn arvon muutos tapahtui</param>
        /// <param name="args">Muutostapahtuman argumentit</param>
        private static void OnAktiivinenChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = obj as Kirjainlaatta;
            if (args.NewValue.Equals(false))
            {
                laatta.FillColor = laatta.InactiveFillColor;
                laatta.BorderColor = laatta.InactiveBorderColor;
            }
            //else
            //{
            //    laatta.FillColor = laatta.ActiveFillColor;
            //    laatta.BorderColor = laatta.ActiveBorderColor;
            //}
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan Kirjain-propertyn arvon muutoksille.
        /// </summary>
        /// <param name="obj">Objekti jossa propertyn arvon muutos tapahtui</param>
        /// <param name="args">Muutostapahtuman argumentit</param>
        private static void OnKirjainChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = obj as Kirjainlaatta;
            laatta.kirjainLabel.Content = args.NewValue.ToString();
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan Pistearvo-propertyn arvon muutoksille.
        /// </summary>
        /// <param name="obj">Objekti jossa propertyn arvon muutos tapahtui</param>
        /// <param name="args">Muutostapahtuman argumentit</param>
        private static void OnPistearvoChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = obj as Kirjainlaatta;
            laatta.pistearvoLabel.Content = args.NewValue;
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan reunusvärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnBorderColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            laatta.border.BorderBrush = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan täyttövärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnFillColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            laatta.containerGrid.Background = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan tekstivärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnTextColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            laatta.kirjainLabel.Foreground = (SolidColorBrush)args.NewValue;
            laatta.pistearvoLabel.Foreground = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä epäaktiivisen Kirjainlaatan reunusvärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnActiveBorderColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) laatta.BorderColor = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä epäaktiivisen Kirjainlaatan täyttövärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnActiveFillColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) laatta.FillColor = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan tekstivärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnActiveTextColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) laatta.TextColor = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä epäaktiivisen Kirjainlaatan reunusvärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnInactiveBorderColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) return;
            laatta.BorderColor = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä epäaktiivisen Kirjainlaatan täyttövärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnInactiveFillColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) return;
            laatta.FillColor = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// Käsittelijä Kirjainlaatan tekstivärin arvon muutoksille
        /// </summary>
        /// <param name="obj">Kirjainlaatta jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnInactiveTextColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Kirjainlaatta laatta = (Kirjainlaatta)obj;
            if (laatta.Aktiivinen) return;
            laatta.TextColor = (SolidColorBrush)args.NewValue;
        }

        #endregion

        #region Constructors

        /// <summary>Oletusmuodostaja.</summary>
        /// <remarks>Ei käytössä.</remarks>
        public Kirjainlaatta()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja.
        /// </summary>
        /// <param name="kirjain">Kirjainlaatan sisältämä kirjain</param>
        /// <param name="pistearvo">Kirjainlaatan pistearvo</param>
        public Kirjainlaatta(string kirjain, int pistearvo)
        {
            InitializeComponent();
            this.Kirjain = kirjain;
            this.Pistearvo = pistearvo;
            this.kirjainLabel.Content = kirjain;
            this.Aktiivinen = true;
            if (pistearvo > 0) this.pistearvoLabel.Content = "" + pistearvo;
            this.MouseMove += new MouseEventHandler(Kirjainlaatta_MouseMove);
        }

        #endregion

        #region Routed Events

        /// <summary>
        /// Routed Event jolla ilmoitetaan loogisessa puussa ylemmäs että Kirjainlaatan
        /// paikka on vaihtunut, jotta voidaan tehdä tarvittavia 'säätöjä' puun ylempiin
        /// elementteihin.
        /// </summary>
        public static readonly RoutedEvent KirjainlaatanPaikkaVaihtunutEvent =
            EventManager.RegisterRoutedEvent("KirjainlaatanPaikkaVaihtunut", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Kirjainlaatta));

        public event RoutedEventHandler KirjainlaatanPaikkaVaihtunut
        {
            add { AddHandler(KirjainlaatanPaikkaVaihtunutEvent, value); }
            remove { RemoveHandler(KirjainlaatanPaikkaVaihtunutEvent, value); }
        }

        void RaiseKirjainlaatanPaikkaVaihtunutEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Kirjainlaatta.KirjainlaatanPaikkaVaihtunutEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Käsittelijä Kirjainlaatan MouseMove-tapahtumalle. Tapahtuu kun Kirjainlaattaa
        /// aletaan raahata hiirellä.
        /// </summary>
        /// <param name="sender">Objekti joka laukaisi tapahtuman (Kirjainlaatta)</param>
        /// <param name="e"></param>
        void Kirjainlaatta_MouseMove(object sender, MouseEventArgs e)
        {
            // Jos raahaamisen kohde ei ollut Kirjainlaatta, ei tehdä mitään
            if (!(sender is Kirjainlaatta)) return;
            // Pakataan Kirjainlaatta DataObjectiin
            DataObject data = new DataObject(typeof(Kirjainlaatta), ((Kirjainlaatta)sender));
            // Aloitetaan raahaustapahtuma, joka voidaan napata kiinni muissa elementeissä
            if (sender != null && e.LeftButton == MouseButtonState.Pressed && ((Kirjainlaatta)sender).Aktiivinen != false)
            {
                RaiseKirjainlaatanPaikkaVaihtunutEvent();
                DragDrop.DoDragDrop((Kirjainlaatta)sender, data, DragDropEffects.Move);
            };
            e.Handled = true;
            data = null;
        }

        #endregion
    }
}
