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

namespace GameComponents
{
    /// <summary>
    /// Interaction logic for KirjainlaattaHolder.xaml
    /// </summary>
    public partial class KirjainlaattaHolder : UserControl
    {

        #region Dependency properties

        /// <summary>
        /// Dependency property joka määrää sen voidaanko KirjainlaattaHolder-komponentin
        /// kapasiteettia (sen sisältämien Kirjainlaattojen maksimimäärää) muuttaa
        /// </summary>
        public static readonly DependencyProperty MaxLukumaaraModifiableProperty =
             DependencyProperty.Register("MaxLukumaaraModifiable", typeof(bool), typeof(KirjainlaattaHolder),
             new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Getter and setter for DependencyProperty 'MaxLukumaaraModifiable'
        /// </summary>
        public bool MaxLukumaaraModifiable
        {
            get { return (bool)GetValue(MaxLukumaaraModifiableProperty); }
            set { SetValue(MaxLukumaaraModifiableProperty, value); }
        }

        /// <summary>
        /// Dependency property joka määrää kuinka monta Kirjainlaattaa KirjainlaattaHolder-komponentti
        /// voi maksimissaan sisältää.
        /// </summary>
        public static readonly DependencyProperty MaxLukumaaraProperty =
            DependencyProperty.Register("MaxLukumaara", typeof(int), typeof(KirjainlaattaHolder),
            new FrameworkPropertyMetadata(7, FrameworkPropertyMetadataOptions.None,
                OnMaxLukumaaraChanged));

        /// <summary>
        /// Getter and setter for DependencyProperty 'MaxLukumaara'
        /// </summary>
        public int MaxLukumaara
        {
            get { return (int)GetValue(MaxLukumaaraProperty); }
            set { SetValue(MaxLukumaaraProperty, value); }
        }

        /// <summary>
        /// Käsittelijä kun KirjainlaattaHolderin MaxLukumaara dependency propertyn arvoa muutetaan.
        /// Tarkistetaan voidaanko arvoa muuttaa, ja jos voi niin päivitetään propertyn arvo.
        /// </summary>
        /// <param name="obj">objekti jossa muutos tapahtui</param>
        /// <param name="args">argumentit</param>
        private static void OnMaxLukumaaraChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            KirjainlaattaHolder holder = obj as KirjainlaattaHolder;
            if (holder.MaxLukumaaraModifiable)
            {
                holder.MaxLukumaara = (int)args.NewValue;
            }
        }

        /// <summary>
        /// Dependency property joka säilyttää tietoa siitä, kuinka monta Kirjainlaattaa
        /// KirjainlaattaHolder-komponentti tällä hetkellä sisältää
        /// </summary>
        public static readonly DependencyProperty LaattojaPaikallaProperty =
            DependencyProperty.Register("LaattojaPaikalla", typeof(int), typeof(KirjainlaattaHolder));


        /// <summary>
        /// Getter and setter for DependencyProperty 'LaattojaPaikalla'
        /// </summary>
        public int LaattojaPaikalla
        {
            get { return (int)GetValue(LaattojaPaikallaProperty); }
            set { SetValue(LaattojaPaikallaProperty, value); }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Muodostaja
        /// </summary>
        public KirjainlaattaHolder()
        {
            InitializeComponent();
        }
        #endregion

        #region Public methods/helpers

        /// <summary>
        /// Lisää halutun Kirjainlaatan KirjainlaattaHolderiin ja päivittää LaattojaPaikalla
        /// dependency propertyn arvon.
        /// </summary>
        /// <param name="laatta">KirjainlaattaHolderiin lisättävä Kirjainlaatta</param>
        public void AddObjectToHolder(Kirjainlaatta laatta)
        {
            if (LaattojaPaikalla < MaxLukumaara)
            {
                kirjainlaattaHolder.Children.Add(laatta);
                laatta.Aktiivinen = true;
                LaattojaPaikalla++;
            }
        }

        /// <summary>
        /// Poistaa halutun Kirjainlaatan KirjainlaattaHolderista ja päivittää LaattojaPaikalla
        /// dependency propertyn arvon.
        /// </summary>
        /// <param name="laatta"></param>
        public void RemoveObjectFromHolder(Kirjainlaatta laatta)
        {
            if (kirjainlaattaHolder.Children.Contains(laatta))
            {
                kirjainlaattaHolder.Children.Remove(laatta);
                if (LaattojaPaikalla > 0) LaattojaPaikalla--;
            }
        }

        /// <summary>
        /// Palauttaa listan KirjainlaattaHolder komponentissa tällä hetkellä olevista Kirjainlaatoista
        /// </summary>
        /// <returns>listan KirjainlaattaHolder komponentissa tällä hetkellä olevista Kirjainlaatoista</returns>
        public List<Kirjainlaatta> GetKirjainlaatat()
        {
            List<Kirjainlaatta> laatat = new List<Kirjainlaatta>();
            foreach (UIElement element in kirjainlaattaHolder.Children)
            {
                if (element.GetType() == typeof(Kirjainlaatta)) laatat.Add((Kirjainlaatta)element);
            }
            return laatat;
        }

        /// <summary>
        /// Kertoo sisältääkö KirjainlaattaHolder komponentti tietyn Kirjainlaatan
        /// </summary>
        /// <param name="k">Kirjainlaatta jota etsitään</param>
        /// <returns>Boolean arvon joka ilmaisee löydettiinkö määriteltyä Kirjainlaattaa vaiko ei</returns>
        public Boolean Contains(Kirjainlaatta k)
        {
            if (kirjainlaattaHolder.Children.Contains(k)) return true;
            return false;
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// Käsittelijä KirjainlaattaHolderin Drop-eventille. Kaivetaan Kirjainlaatta
        /// raahausdatasta ja lisätään se holderiin, samalla poistetaan Kirjainlaatta sen edellisestä
        /// sijainnista.
        /// </summary>
        /// <param name="sender">objekti joka laukaisi tapahtuman</param>
        /// <param name="e">argumentit</param>
        private void kirjainlaattaHolder_Drop(object sender, DragEventArgs e)
        {
            Kirjainlaatta laatta = e.Data.GetData(typeof(Kirjainlaatta)) as Kirjainlaatta;
            StackPanel parent = laatta.Parent as StackPanel;
            //KirjainlaattaHolderin sisällä ei ole järkeä drag&dropata
            if (parent.Parent.GetType().Equals(typeof(KirjainlaattaHolder))) return;
            parent.Children.Remove(laatta);
            // Jos laatta oli alunperin tyhjä laatta (pistearvo == 0),
            // poistetaan sen sisältämä Kirjain, jottei sitä sekoiteta muihin
            //Kirjainlaattoihin
            if (laatta.Pistearvo == 0) laatta.Kirjain = "";
            AddObjectToHolder(laatta);            
        }

        #endregion
    }
}
