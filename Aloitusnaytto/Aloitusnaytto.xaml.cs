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

namespace Aloitusnaytto
{
    /// <summary>
    /// Interaction logic for Aloitusnaytto.xaml
    /// </summary>
    public partial class Aloitusnaytto : UserControl
    {
        /// <summary>Muodostaja</summary>
        public Aloitusnaytto()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja, joka antaa määrittää tekstikenttien sisältöä.
        /// </summary>
        /// <param name="version">ohjelman versio</param>
        /// <param name="vdate">ohjelman versiopäiväys</param>
        public Aloitusnaytto(String version, String vdate)
        {
            InitializeComponent();
            peliVersio.Text = "Versio: " + version + ", " + vdate;
        }

        #region UI Event Handlers

        /// <summary>
        /// Käsittelijä kun hiiri siirretään 'Uusi peli' -napin päälle
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnUusiPeli_MouseEnter(object sender, MouseEventArgs e)
        {
            uusiPeliKuvaus.Text = "Aloittaa uuden pelin nykyisillä peliasetuksilla.";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään pois 'Uusi peli'-napin päältä
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnUusiPeli_MouseLeave(object sender, MouseEventArgs e)
        {
            uusiPeliKuvaus.Text = "";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään 'Asetukset' -napin päälle
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnAsetukset_MouseEnter(object sender, MouseEventArgs e)
        {
            asetuksetKuvaus.Text = "Näyttää pelin asetukset.";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään pois 'Asetukset'-napin päältä
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnAsetukset_MouseLeave(object sender, MouseEventArgs e)
        {
            asetuksetKuvaus.Text = "";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään 'Lopeta peli' -napin päälle
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnLopetaPeli_MouseEnter(object sender, MouseEventArgs e)
        {
            lopetaPeliKuvaus.Text = "Sulkee peliohjelman.";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään pois 'Lopeta peli'-napin päältä
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnLopetaPeli_MouseLeave(object sender, MouseEventArgs e)
        {
            lopetaPeliKuvaus.Text = "";
        }

        /// <summary>
        /// Käsittelijä kun hiiri siirretään 'Ohjeet'-napin päälle
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnAvustus_MouseEnter(object sender, MouseEventArgs e)
        {
            ohjeetKuvaus.Text = "Näyttää pelin ohjeet internetselaimessa.";
        }

        private void btnAvustus_MouseLeave(object sender, MouseEventArgs e)
        {
            ohjeetKuvaus.Text = "";
        }

        /// <summary>
        /// Käsittelijä kun painetaan 'Uusi peli'-nappia. Laukaisee routed eventin
        /// jolla kerrotaan pääohjelmalle että pitää alustaa ja käynnistää uusi peli.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnUusiPeli_Click(object sender, RoutedEventArgs e)
        {
            RaiseUusiPeliEvent();
        }

        /// <summary>
        /// Käsittelijä kun painetaan 'Asetukset'-nappia. Laukaisee routed eventin,
        /// jolla kerrotaan pääohjelmalle että pitää näyttää pelin asetukset -dialogi.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnAsetukset_Click(object sender, RoutedEventArgs e)
        {
            RaiseAsetuksetEvent();
        }

        /// <summary>
        /// Käsittelijä kun painetaan 'Lopeta peli'-nappia. Laukaisee routed eventin,
        /// jolla kerrotaan pääohjelmalle että pitää sulkea koko ohjelma.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnLopetaPeli_Click(object sender, RoutedEventArgs e)
        {
            RaiseLopetaPeliEvent();
        }

        /// <summary>
        /// Käsittelijä kun painetaan 'Ohjeet'-nappia. Laukaisee routed eventin jolla
        /// kerrotaan pääohjelmalle että pitää näyttää pelin ohjeistus käyttäjälle.
        /// </summary>
        /// <param name="sender">ei käytössä</param>
        /// <param name="e">ei käytössä</param>
        private void btnAvustus_Click(object sender, RoutedEventArgs e)
        {
            RaiseNaytaAvustusEvent();
        }

        #endregion

        #region Routed events

        /// <summary>
        /// Routed Event, jolla kerrotaan pääohjelmalle että halutaan aloittaa
        /// uusi peli (alustaa peli).
        /// </summary>
        public static readonly RoutedEvent UusiPeliEvent =
            EventManager.RegisterRoutedEvent("UusiPeli", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Aloitusnaytto));

        public event RoutedEventHandler UusiPeli
        {
            add { AddHandler(UusiPeliEvent, value); }
            remove { RemoveHandler(UusiPeliEvent, value); }
        }

        void RaiseUusiPeliEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Aloitusnaytto.UusiPeliEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event jolla kerrotaan pääohjelmalle että halutaan avata asetukset-dialogi
        /// pelin asetusten muuttamista varten.
        /// </summary>
        public static readonly RoutedEvent AsetuksetEvent =
            EventManager.RegisterRoutedEvent("Asetukset", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Aloitusnaytto));

        public event RoutedEventHandler Asetukset
        {
            add { AddHandler(AsetuksetEvent, value); }
            remove { RemoveHandler(AsetuksetEvent, value); }
        }

        void RaiseAsetuksetEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Aloitusnaytto.AsetuksetEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event, jolla kerrotaan pääohjelmalle että halutaan lopettaa pelaaminen
        /// (halutaan sulkea koko ohjelma).
        /// </summary>
        public static readonly RoutedEvent LopetaPeliEvent =
            EventManager.RegisterRoutedEvent("LopetaPeli", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Aloitusnaytto));

        public event RoutedEventHandler LopetaPeli
        {
            add { AddHandler(LopetaPeliEvent, value); }
            remove { RemoveHandler(LopetaPeliEvent, value); }
        }

        void RaiseLopetaPeliEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Aloitusnaytto.LopetaPeliEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event, jolla kerrotaan pääohjelmalle että pitää näyttää käyttäjälle
        /// pelin ohjeet.
        /// </summary>
        public static readonly RoutedEvent NaytaAvustusEvent =
            EventManager.RegisterRoutedEvent("NaytaAvustus", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Aloitusnaytto));

        public event RoutedEventHandler NaytaAvustus
        {
            add { AddHandler(NaytaAvustusEvent, value); }
            remove { RemoveHandler(NaytaAvustusEvent, value); }
        }

        void RaiseNaytaAvustusEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Aloitusnaytto.NaytaAvustusEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion
    }
}
