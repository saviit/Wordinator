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
using System.IO;

namespace GameComponents
{
    /// <summary>
    /// Interaction logic for Pelilauta.xaml
    /// </summary>
    public partial class Pelilauta : UserControl
    {
        #region Private properties

        /// <summary>
        /// Kokonaisluku, joka ilmaisee meneillään olevan kierroksen numeron.
        /// Tarvitaan sanan sijoittelutarkistuksiin.
        /// </summary>
        /// <see cref="Pelilauta.HaePisteRiviLaatat"/>
        /// <see cref="Pelilauta.HaePisteSarakeLaatat"/>
        private int KierrosNumero = 0;

        /// <summary>
        /// Kirjainlaattalista, jonne tallennetaan pelilaudalle lisätyt Kirjainlaatat
        /// tarkistuksia ja pistelaskua varten
        /// </summary>
        private List<Kirjainlaatta> laudalleLisatyt = new List<Kirjainlaatta>();

        /// <summary>
        /// Sanalista, jonne tallennetaan kaikki pelin hyväksymät sanat
        /// </summary>
        private List<String> hyvaksytytSanat = new List<String>();

        /// <summary>
        /// Kaksiulotteinen int-taulukko, jolla määritellään erityyppisten PelilautaRuutujen
        /// sijoittelu Pelilaudalle
        /// </summary>
        /// <remarks>Pelin tunnistamia PelilautaRuutu-tyyppejä ovat seuraavat:
        /// 0 = 'tyhjä' PelilautaRuutu, ei lisäpisteitä
        /// 1 = DL, tuplakirjain, kirjaimesta tuplapisteet
        /// 2 = TL, triplakirjain, kirjaimesta triplapisteet
        /// 3 = DW, tuplasana, koko sanasta tuplapisteet
        /// 4 = TW, triplasana, koko sanasta triplapisteet
        /// 5 = keskipiste (tähti), laudan keskipiste, koko sanasta tuplapisteet
        /// </remarks>
        /// <see cref="PelilautaRuutu.PelilautaRuutu(int tyyppi, int x, int y)"/>
        private int[,] _layout = new int[13, 13] { {4, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 4},
                                                   {0, 3, 0, 0, 2, 0, 0, 0, 2, 0, 0, 3, 0},
                                                   {0, 0, 3, 0, 0, 1, 0, 1, 0, 0, 3, 0, 0},
                                                   {1, 0, 0, 3, 0, 0, 1, 0, 0, 3, 0, 0, 1},
                                                   {0, 2, 0, 0, 2, 0, 0, 0, 2, 0, 0, 2, 0},
                                                   {0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0},
                                                   {4, 0, 0, 1, 0, 0, 5, 0, 0, 1, 0, 0, 4},
                                                   {0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0},
                                                   {0, 2, 0, 0, 2, 0, 0, 0, 2, 0, 0, 2, 0},
                                                   {1, 0, 0, 3, 0, 0, 1, 0, 0, 3, 0, 0, 1},
                                                   {0, 0, 3, 0, 0, 1, 0, 1, 0, 0, 3, 0, 0},
                                                   {0, 3, 0, 0, 2, 0, 0, 0, 2, 0, 0, 3, 0},
                                                   {4, 0, 0, 1, 0, 0, 4, 0, 0, 1, 0, 0, 4} };

        #endregion

        #region Constructor
        /// <summary>
        /// Muodostaja
        /// </summary>
        public Pelilauta()
        {
            InitializeComponent();
            //LueHyvaksytytSanatTiedostosta("sanalista_kotus.txt");
        }
        #endregion

        #region Routed Events

        /// <summary>
        /// Routed event jolla ilmoitetaan pääikkunalle että kirjainlaattoja on
        /// sijoiteltu väärin laudalle
        /// </summary>
        public static readonly RoutedEvent LaattojenSijoitteluVirheEvent =
            EventManager.RegisterRoutedEvent("LaattojenSijoitteluVirhe", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Pelilauta));

        public event RoutedEventHandler LaattojenSijoitteluVirhe
        {
            add { AddHandler(LaattojenSijoitteluVirheEvent, value); }
            remove { RemoveHandler(LaattojenSijoitteluVirheEvent, value); }
        }

        void RaiseLaattojenSijoitteluVirheEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Pelilauta.LaattojenSijoitteluVirheEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event jolla ilmoitetaan että käyttäjän laudalle lisäämää
        /// sanaa ei löytynyt hyväksyttyjen sanojen listasta
        /// </summary>
        public static readonly RoutedEvent SanaaEiLoytynytEvent =
            EventManager.RegisterRoutedEvent("SanaaEiLoytynyt", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Pelilauta));
        public event RoutedEventHandler SanaaEiLoytynyt
        {
            add { AddHandler(SanaaEiLoytynytEvent, value); }
            remove { RemoveHandler(SanaaEiLoytynytEvent, value); }
        }
        void RaiseSanaaEiLoytynytEvent(String sana)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Pelilauta.SanaaEiLoytynytEvent, sana);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event jolla ilmoitetaan että käyttäjän laudalle lisäämä
        /// sana ei ole 'kiinni' aikaisemmin lisätyssä sanassa
        /// </summary>
        public static readonly RoutedEvent SanaEiKiinniAikaisemmassaVirheEvent =
            EventManager.RegisterRoutedEvent("SanaEiKiinniAikaisemmassaVirhe", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Pelilauta));
        public event RoutedEventHandler SanaEiKiinniAikaisemmassaVirhe
        {
            add { AddHandler(SanaEiKiinniAikaisemmassaVirheEvent, value); }
            remove { RemoveHandler(SanaEiKiinniAikaisemmassaVirheEvent, value); }
        }
        void RaiseSanaEiKiinniAikaisemmassaVirheEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Pelilauta.SanaEiKiinniAikaisemmassaVirheEvent);
            RaiseEvent(newEventArgs);
        }

        /// <summary>
        /// Routed event jolla ilmoitetaan että pelin ensimmäinen laudalle
        /// lisätty sana ei peitä pelin aloitusruutua (pelilaudan keskimmäinen ruutu)
        /// </summary>
        public static readonly RoutedEvent EnsimmainenSanaEiAloitusruudussaVirheEvent =
            EventManager.RegisterRoutedEvent("EnsimmainenSanaEiAloitusruudussaVirhe", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Pelilauta));
        public event RoutedEventHandler EnsimmainenSanaEiAloitusruudussaVirhe
        {
            add { AddHandler(EnsimmainenSanaEiAloitusruudussaVirheEvent, value); }
            remove { RemoveHandler(EnsimmainenSanaEiAloitusruudussaVirheEvent, value); }
        }
        void RaiseEnsimmainenSanaEiAloitusruudussaVirheEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Pelilauta.EnsimmainenSanaEiAloitusruudussaVirheEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region Public methods/helpers

        /// <summary>
        /// Täyttää Pelilaudan PelilautaRuuduilla _layout -propertyn mukaisesti
        /// </summary>
        public void CreatePelilautaCells()
        {
            for (int row = 0; row < _layout.GetLength(0); row++)
            {
                for (int col = 0; col < _layout.GetLength(1); col++)
                {
                    PelilautaRuutu ruutu = new PelilautaRuutu(_layout[row, col], col, row);
                    Canvas.SetTop(ruutu, row * 50);
                    Canvas.SetLeft(ruutu, col * 50);
                    canvas.Children.Add(ruutu);
                }
            }
        }

        /// <summary>
        /// Palauttaa listan kaikista pelilaudalle lisätyistä kirjainlaatoista
        /// </summary>
        /// <returns>lista pelilaudalla olevista kirjainlaatoista</returns>
        public List<Kirjainlaatta> GetLaudalleLisatyt()
        {
            List<Kirjainlaatta> kaikkiLisatyt = new List<Kirjainlaatta>();
            foreach (PelilautaRuutu ruutu in canvas.Children)
            {
                Kirjainlaatta laatta = ruutu.GetKirjainlaatta();
                if (ruutu.RuutuSisaltaaLaatan && laatta != null)
                {
                    kaikkiLisatyt.Add(laatta);
                }
            }
            return kaikkiLisatyt;
        }

        /// <summary>
        /// Palauttaa listan kaikista tämän kierroksen aikana (Aktiivinen == true)
        /// pelilaudalle lisätyistä kirjainlaatoista
        /// </summary>
        /// <returns>lista pelilaudalla olevista aktiivisista kirjainlaatoista</returns>
        public List<Kirjainlaatta> GetAktiivisetLaudalleLisatyt()
        {
            laudalleLisatyt.Clear();
            foreach (PelilautaRuutu ruutu in canvas.Children)
            {
                Kirjainlaatta laatta = ruutu.GetKirjainlaatta();
                if (ruutu.RuutuSisaltaaLaatan && laatta.Aktiivinen == true)
                {
                    laudalleLisatyt.Add(laatta);
                }
            }
            return laudalleLisatyt;
        }

        /// <summary>
        /// Palauttaa hyväksyttyjen sanojen listan muiden luokkien käyttöön
        /// </summary>
        /// <returns>lista hyväksytyistä sanoista</returns>
        public List<String> GetHyvaksytytSanat()
        {
            return hyvaksytytSanat;
        }

        /// <summary>
        /// Lukee pelin hyväksymät sanat tiedostosta sanalistaan (List<String>)
        /// </summary>
        /// <param name="tiedosto">tiedosto josta sanat luetaan</param>
        public void LueHyvaksytytSanatTiedostosta(String tiedosto)
        {
            StreamReader sr = new StreamReader(tiedosto);
            String rivi;
            while ((rivi = sr.ReadLine()) != null)
            {
                //if (rivi.Contains("\n")) rivi.Replace("\n", "");
                if (rivi.Length > 13) continue;
                // Kirjainlaattojen Kirjain-propertyssa käytetään isoja kirjaimia, joten
                // hyväksyttyjen sanojen listassa pitää myös käyttää isoja kirjaimia
                hyvaksytytSanat.Add(rivi.ToUpper(new System.Globalization.CultureInfo("fi-FI")));
            }
            sr.Close();
        }

        /// <summary>
        /// Apumetodi, joka laskee pelaajan tämän kierroksen pisteet pelilaudalle lisätyistä Kirjainlaatoista.
        /// </summary>
        /// <param name="laatat">lista pelilaudalle lisätyistä Kirjainlaatoista</param>
        /// <param name="onkoKaikkiLaatatKaytetty">boolean-arvo, joka kertoo onko pelaaja käyttänyt kaikki
        /// käytössään olleet Kirjainlaatat. Jos on, pelaaja saa bonuspisteitä.</param>
        /// <returns>Pelaajan pisteet tältä kierrokselta.</returns>
        public int LaskePisteet(Boolean onkoKaikkiLaatatKaytetty)
        {
            // Haetaan Kirjainlaatat jotka pisteytetään
            List<List<Kirjainlaatta>> lista = HaePisteytettavatLaatat();
            // Jos pisteytettäviä laattoja ei löytynyt, palataan pääohjelmaan
            if (lista == null || lista.Count < 1) return 0;
            int kokonaispisteet = 0;
            int sanakerroin = 1;
            PelilautaRuutu ruutu;
            int bonuspisteet = 0;
            if (onkoKaikkiLaatatKaytetty) bonuspisteet = 50;

            foreach (List<Kirjainlaatta> laatat in lista)
            {
                foreach (Kirjainlaatta laatta in laatat)
                {
                    ruutu = GetChildAt(laatta.PositionX, laatta.PositionY);

                    //pistekerrointyypit: 0, 1, 2 -- tyhjä, 2xkirjain, 3xkirjain
                    //                    3, 4 -- 2xsana, 3xsana
                    //lasketaan pisteet laatoista ja ruutujen pistekertoimista
                    if (ruutu.PistekerroinKaytetty != true)
                    {
                        // kirjainkertoimet (2x kirjain, 3xkirjain)
                        if (ruutu.PistekerroinTyyppi == 1 || ruutu.PistekerroinTyyppi == 2)
                        {
                            kokonaispisteet += (laatta.Pistearvo * ruutu.Pistekerroin);
                            ruutu.PistekerroinKaytetty = true;
                        }
                        // sanakertoimet (2xsana, 3xsana) ja tyhjät ruudut
                        else
                        {
                            kokonaispisteet += laatta.Pistearvo;
                            //lasketaan lopullinen sanapistekerroin
                            sanakerroin = (sanakerroin * ruutu.Pistekerroin);
                            ruutu.PistekerroinKaytetty = true;
                        }
                    }
                    // jos ruudusta saatava kerroin on jo laskettu aikaisemmin
                    else
                    {
                        kokonaispisteet += laatta.Pistearvo;
                    }
                    laatta.Aktiivinen = false;
                }
            }
            kokonaispisteet = (kokonaispisteet * sanakerroin) + bonuspisteet;
            // Sana hyväksyttiin, kasvatetaan KierrosNumeroa tarkistuksia varten
            KierrosNumero++;
            return kokonaispisteet;
        }

        /// <summary>
        /// Poistaa tietyn Kirjainlaatan Pelilaudalta
        /// </summary>
        /// <param name="k">Kirjainlaatta joka halutaan poistaa</param>
        public void RemoveKirjainlaatta(Kirjainlaatta k)
        {
            PelilautaRuutu r = GetChildAt(k.PositionX, k.PositionY);
            r.RemoveKirjainlaatta();
        }

        /// <summary>
        /// Käy läpi Pelilaudan PelilautaRuutu-oliot ja poistaa jokaiselta käsittelijät,
        /// jolloin Pelilautaa ei voi enää muokata.
        /// </summary>
        public void RemovePelilautaRuutuHandlers()
        {
            foreach (PelilautaRuutu ruutu in canvas.Children)
            {
                ruutu.RemoveHandlers();
            }
        }

        #endregion

        #region Private helper methods

        /// <summary>
        /// Palauttaa viitteen haluttuun PelilautaRuutuun.
        /// </summary>
        /// <param name="x">ruudun x-koordinaatti pelilaudalle, pelilaudan koordinaateissa</param>
        /// <param name="y">ruudun y-koordinaatti pelilaudalle, pelilaudan koordinaateissa</param>
        /// <returns>viitteen PelilautaRuutuun joka löytyy pelilaudan koordinaateista x, y</returns>
        private PelilautaRuutu GetChildAt(int x, int y)
        {
            foreach (PelilautaRuutu ruutu in canvas.Children)
            {
                if (ruutu.PositionX == x && ruutu.PositionY == y) return ruutu;
            }
            return null;
        }

        /// <summary>
        /// Palauttaa Kirjainlaatta-listan Kirjainlaatoista jotka pitää huomioida pistelaskussa.
        /// </summary>
        /// <returns>lista Kirjainlaatoista jotka muodostavat pisteytettävän sanan</returns>
        private List<List<Kirjainlaatta>> HaePisteytettavatLaatat()
        {
            List<List<Kirjainlaatta>> pisteytettavat = null;
            laudalleLisatyt = GetAktiivisetLaudalleLisatyt();
            //tarkistetaan ensin että laatat sallitussa 'muodostelmassa'
            // laudalleLisatyt == pelaajan tässä vuorossa laudalle lisäämät
            // laatat (laatat joiden Aktiivinen property == true)
            Boolean laatatSamallaRivilla = onkoLisatytSamallaRivilla(laudalleLisatyt);
            Boolean laatatSamallaSarakkeella = onkoLisatytSamallaSarakkeella(laudalleLisatyt);

            if (laatatSamallaRivilla && laatatSamallaSarakkeella)
            {
                //if ((HaePisteRiviLaatat()) != null)
                //{
                    pisteytettavat = HaePisteRiviLaatat();
                //}

                List<List<Kirjainlaatta>> lisattava;
                //if ((HaePisteSarakeLaatat()) != null)
                if ((lisattava = HaePisteSarakeLaatat()) != null)
                {
                    //List<List<Kirjainlaatta>> lisattava = HaePisteSarakeLaatat();
                    if (pisteytettavat != null)
                    {
                        if (pisteytettavat.Count < 1 && lisattava[0].Count > 1) pisteytettavat.Add(lisattava[0]);
                    }
                    else
                    {
                        if (lisattava[0].Count > 1) pisteytettavat = lisattava;
                    }
                }
                return pisteytettavat;
            }
            else if (laatatSamallaRivilla && !laatatSamallaSarakkeella)
            {
                pisteytettavat = HaePisteRiviLaatat();
                if (pisteytettavat == null || pisteytettavat.Count < 2 && pisteytettavat[0].Count < 2)
                {
                    List<List<Kirjainlaatta>> lisattava;
                    if ((lisattava = HaePisteSarakeLaatat()) != null)
                    {
                        //pisteytettavat = HaePisteSarakeLaatat();
                        pisteytettavat = lisattava;
                    }
                }
                return pisteytettavat;
            }
            else if (laatatSamallaSarakkeella && !laatatSamallaRivilla)
            {
                pisteytettavat = HaePisteSarakeLaatat();
                if (pisteytettavat == null || pisteytettavat.Count < 2 && pisteytettavat[0].Count < 2)
                {
                    List<List<Kirjainlaatta>> lisattava;
                    if ((lisattava = HaePisteRiviLaatat()) != null)
                    {
                        //pisteytettavat = HaePisteRiviLaatat();
                        pisteytettavat = lisattava;
                    }
                }
                return pisteytettavat;
            }
            else RaiseLaattojenSijoitteluVirheEvent();
            return pisteytettavat;
        }

        /// <summary>
        /// Tarkistaa että kaikki pelaajan tällä kierroksella lisäämät Kirjainlaatat
        /// ovat samalla rivillä. Jo ei, palauttaa false.
        /// </summary>
        /// <param name="laatat">lista lisätyistä kirjainlaatoista</param>
        /// <returns>true jos kaikki samalla rivillä, muuten false</returns>
        private Boolean onkoLisatytSamallaRivilla(List<Kirjainlaatta> laatat)
        {
            if (laatat.Count < 1) return false;
            if (laatat.Count == 1) return true;
            else
            {
                int posy = laatat[0].PositionY;
                // Laattojen sijainti Y-akselilla pitää olla sama
                for (int i = 1; i < laatat.Count; i++)
                {
                    if (laatat[i].PositionY != posy) return false;
                    //else posy = laatat[i].PositionY;
                }
                return true;
            }
        }

        /// <summary>
        /// Tarkistaa että kaikki pelaajan tällä kierroksella lisäämät Kirjainlaatat
        /// ovat samassa sarakkeessa. Jos ei, palauttaa false.
        /// </summary>
        /// <param name="laatat">lista lisätyistä kirjainlaatoista</param>
        /// <returns>true jos kaikki samassa sarakkeessa, muuten false</returns>
        private Boolean onkoLisatytSamallaSarakkeella(List<Kirjainlaatta> laatat)
        {
            if (laatat.Count < 1) return false;
            if (laatat.Count == 1) return true;
            else
            {
                int posx = laatat[0].PositionX;
                // Laattojen sijainti X-akselilla pitää olla sama
                for (int i = 1; i < laatat.Count; i++)
                {
                    if (laatat[i].PositionX != posx) return false;
                    //else posx = laatat[i].PositionX;
                }
                return true;
            }
        }

        /// <summary>
        /// Hakee Kirjainlaatat (ensimmäisen lisätyn laatan määräämältä riviltä), jotka 
        /// muodostavat sanan pelaajan tällä kierroksella lisäämien
        /// Kirjainlaattojen kanssa ja tarkistaa että niiden muodostama sana löytyy hyväksyttyjen
        /// sanojen listalta.
        /// </summary>
        /// <returns>Listan Kirjainlaattalistoista jotka muodostavat sanoja</returns>
        private List<List<Kirjainlaatta>> HaePisteRiviLaatat()
        {
            List<List<Kirjainlaatta>> pisteytettavatSanat = new List<List<Kirjainlaatta>>();
            List<Kirjainlaatta> laatat = new List<Kirjainlaatta>();

            int vanhojaLaattojaSanassa = 0;
            bool keskimmainenRuutuPeitossa = false;
            int ensimmainen = laudalleLisatyt[0].PositionX, viimeinen = laudalleLisatyt[0].PositionX;

            // Etsitään sanan aloitus- ja lopetuspisteet
            for (int i = 1; i < laudalleLisatyt.Count; i++)
            {
                if (laudalleLisatyt[i].PositionX < ensimmainen) ensimmainen = laudalleLisatyt[i].PositionX;
                if (laudalleLisatyt[i].PositionX > viimeinen) viimeinen = laudalleLisatyt[i].PositionX;
            }

            // Lisätään kaikki Kirjainlaatat alku-ja loppupisteiden välistä
            // tarkistettavien listaan.
            for (int i = ensimmainen; i <= viimeinen; i++)
            {
                PelilautaRuutu ruutu = GetChildAt(i, laudalleLisatyt[0].PositionY);
                if (ruutu.RuutuSisaltaaLaatan)
                {
                    Kirjainlaatta k = ruutu.GetKirjainlaatta();
                    laatat.Add(k);
                }
                else
                {
                    RaiseLaattojenSijoitteluVirheEvent();
                    return null;
                }
            }
            
            //Tarkistetaan onko lisättyjen laattojen etu/takapuolella laattoja
            //ja lisätään ne tarkistettavaksi
            List<Kirjainlaatta> viereiset = new List<Kirjainlaatta>();
            viereiset.AddRange(laatat);

            int eka = viereiset[0].PositionX, vika = viereiset[viereiset.Count - 1].PositionX;
            int sarake = viereiset[0].PositionY;
            PelilautaRuutu r; // = GetChildAt(eka - 1, sarake);
            // edessä olevat
            while ((r = GetChildAt(eka - 1, sarake)) != null && r.RuutuSisaltaaLaatan != false)
            {
                eka = r.PositionX;
                viereiset.Insert(0, r.GetKirjainlaatta());
                //r = GetChildAt(eka - 1, sarake);
            }
            //takana olevat
            //r = GetChildAt(vika + 1, sarake);
            while ((r = GetChildAt(vika + 1, sarake)) != null && r.RuutuSisaltaaLaatan != false)
            {
                vika = r.PositionX;
                viereiset.Add(r.GetKirjainlaatta());
                //r = GetChildAt(vika + 1, sarake);
            }

            // Tarkistetaan sanan sijoittelu
            foreach (Kirjainlaatta k in viereiset)
            {
                PelilautaRuutu ruutu = GetChildAt(k.PositionX, k.PositionY);
                if (KierrosNumero == 0 && ruutu.PistekerroinTyyppi == 5)
                {
                    keskimmainenRuutuPeitossa = true;
                }
                if (k.Aktiivinen == false && KierrosNumero > 0)
                {
                    vanhojaLaattojaSanassa++;
                }
            }

            // Tarkistetaan että ensimmäinen lisätty sana peittää
            // pelilaudan keskimmäisen ruuden (aloitusruutu)
            if (KierrosNumero == 0 && !keskimmainenRuutuPeitossa)
            {
                RaiseEnsimmainenSanaEiAloitusruudussaVirheEvent();
                return null;
            }
            // Tarkistetaan että lisätty sana on 'kiinni' jossakin 
            // aikaisemmin lisätyssä sanassa
            if (laudalleLisatyt.Count > 1 && !onkoLisatytSamallaSarakkeella(laudalleLisatyt))
            {
                if (KierrosNumero > 0 && !(vanhojaLaattojaSanassa > 0))
                {
                    RaiseSanaEiKiinniAikaisemmassaVirheEvent();
                    return null;
                }
            }
            String kokosana = "";
            foreach (Kirjainlaatta kirjain in viereiset)
            {
                kokosana += kirjain.Kirjain;
            }
            if (EtsiSanalistasta(kokosana))
            {
                pisteytettavatSanat.Add(viereiset);
            }
            else
            {
                // ilmoitetaan käyttäjälle virheestä
                RaiseSanaaEiLoytynytEvent(kokosana);
                // palautetaan null-lista, jotta ei anneta pisteitä virheellisistä sijoituksista
                return null;
            }

            List<Kirjainlaatta> lisasana = new List<Kirjainlaatta>();

            foreach (Kirjainlaatta laatta in laatat)
            {
                lisasana.Clear();
                int rivi = laatta.PositionX;
                eka = laatta.PositionY;
                vika = laatta.PositionY;
                //r = GetChildAt(rivi, eka - 1);
                //Jos ylhäällä tai alhaalla laattoja, lisätään tämä laatta listaan
                if ((r = GetChildAt(rivi, eka - 1)) != null && r.RuutuSisaltaaLaatan
                    || (GetChildAt(rivi, vika + 1) != null) && GetChildAt(rivi, vika + 1).RuutuSisaltaaLaatan)
                {
                    lisasana.Add(laatta);
                }
                else
                {
                    // siirry seuraavan laatan kohdalle
                    continue;
                }

                // ylhäällä olevat
                while ((r = GetChildAt(rivi, eka - 1)) != null && r.RuutuSisaltaaLaatan != false)
                {
                    eka = r.PositionY;
                    lisasana.Insert(0, r.GetKirjainlaatta());
                    //r = GetChildAt(rivi, eka - 1);
                }

                // alhaalla olevat
                //r = GetChildAt(rivi, vika + 1);
                while ((r = GetChildAt(rivi, vika + 1)) != null && r.RuutuSisaltaaLaatan != false)
                {
                    vika = r.PositionY;
                    lisasana.Add(r.GetKirjainlaatta());
                    //r = GetChildAt(rivi, vika + 1);
                }

                // tarkistetaan muodostuiko hyväksyttyvä sana
                String uusisana = "";
                foreach (Kirjainlaatta kirjain in lisasana)
                {
                    uusisana += kirjain.Kirjain;
                }
                if (EtsiSanalistasta(uusisana))
                {
                    pisteytettavatSanat.Add(lisasana);
                }
                else
                {
                    // ilmoitetaan käyttäjälle virheestä; jos joku 'lisäsanoista'
                    // on virheellinen, kirjainlaatat eivät voi olla tässä muodostelmassa
                    // vaikka 'pääsana' olisi oikein
                    RaiseSanaaEiLoytynytEvent(uusisana);
                    // palautetaan null-lista, jotta ei anneta pisteitä virheellisistä sijoituksista
                    return null;
                }
            }
            return pisteytettavatSanat;
        }

        /// <summary>
        /// Hakee Kirjainlaatat (ensimmäisen lisätyn laatan määräämästä sarakkeesta), jotka 
        /// muodostavat sanan pelaajan tällä kierroksella lisäämien Kirjainlaattojen kanssa 
        /// ja tarkistaa että niiden muodostama sana löytyy hyväksyttyjen
        /// sanojen listalta.
        /// </summary>
        /// <returns>Listan Kirjainlaatoista jotka muodostavat sanan</returns>
        private List<List<Kirjainlaatta>> HaePisteSarakeLaatat()
        {
            List<List<Kirjainlaatta>> pisteytettavatSanat = new List<List<Kirjainlaatta>>();
            List<Kirjainlaatta> laatat = new List<Kirjainlaatta>();
            
            bool keskimmainenRuutuPeitossa = false;
            int vanhojaLaattojaSanassa = 0;
            int ensimmainen = laudalleLisatyt[0].PositionY, viimeinen = laudalleLisatyt[0].PositionY;

            // Etsitään sanan aloitus- ja lopetuspisteet
            for (int i = 1; i < laudalleLisatyt.Count; i++)
            {
                if (laudalleLisatyt[i].PositionY < ensimmainen) ensimmainen = laudalleLisatyt[i].PositionY;
                if (laudalleLisatyt[i].PositionY > viimeinen) viimeinen = laudalleLisatyt[i].PositionY;
            }

            // Lisätään kaikki Kirjainlaatat alku-ja loppupisteiden välistä
            // tarkistettavien listaan.
            for (int i = ensimmainen; i <= viimeinen; i++)
            {
                PelilautaRuutu ruutu = GetChildAt(laudalleLisatyt[0].PositionX, i);
                if (ruutu.RuutuSisaltaaLaatan)
                {
                    Kirjainlaatta k = ruutu.GetKirjainlaatta();
                    laatat.Add(k);
                }
                else
                {
                    RaiseLaattojenSijoitteluVirheEvent();
                    return null;
                }
            }

            //Tarkistetaan onko lisättyjen laattojen ala/yläpuolella laattoja
            //ja lisätään ne tarkistettavaksi
            List<Kirjainlaatta> viereiset = new List<Kirjainlaatta>();
            viereiset.AddRange(laatat);

            int eka = viereiset[0].PositionY, vika = viereiset[viereiset.Count - 1].PositionY;
            int rivi = viereiset[0].PositionX;
            PelilautaRuutu r; // = GetChildAt(rivi, eka - 1);
            // ylhäällä olevat
            while ((r = GetChildAt(rivi, eka - 1)) != null && r.RuutuSisaltaaLaatan != false)
            {
                eka = r.PositionY;
                viereiset.Insert(0, r.GetKirjainlaatta());
                //r = GetChildAt(rivi, eka - 1);
            }
            //alhaalla olevat
            //r = GetChildAt(rivi, vika + 1);
            while ((r = GetChildAt(rivi, vika + 1)) != null && r.RuutuSisaltaaLaatan != false)
            {
                vika = r.PositionY;
                viereiset.Add(r.GetKirjainlaatta());
                //r = GetChildAt(rivi, vika + 1);
            }

            // Tarkistetaan sanan sijoittelu
            foreach (Kirjainlaatta k in viereiset)
            {
                PelilautaRuutu ruutu = GetChildAt(k.PositionX, k.PositionY);
                if (KierrosNumero == 0 && ruutu.PistekerroinTyyppi == 5)
                {
                    keskimmainenRuutuPeitossa = true;
                }
                if (k.Aktiivinen == false && KierrosNumero > 0)
                {
                    vanhojaLaattojaSanassa++;
                }
            }
            // Tarkistetaan että ensimmäinen lisätty sana peittää
            // pelilaudan keskimmäisen ruuden (aloitusruutu)
            if (KierrosNumero == 0 && !keskimmainenRuutuPeitossa)
            {
                RaiseEnsimmainenSanaEiAloitusruudussaVirheEvent();
                return null;
            }
            // Tarkistetaan että lisätty sana on 'kiinni' jossakin 
            // aikaisemmin lisätyssä sanassa
            if (laudalleLisatyt.Count > 1 && !onkoLisatytSamallaRivilla(laudalleLisatyt))
            {
                if (KierrosNumero > 0 && !(vanhojaLaattojaSanassa > 0))
                {
                    RaiseSanaEiKiinniAikaisemmassaVirheEvent();
                    return null;
                }
            }

            String kokosana = "";
            foreach (Kirjainlaatta kirjain in viereiset)
            {
                kokosana += kirjain.Kirjain;
            }
            if (EtsiSanalistasta(kokosana))
            {
                pisteytettavatSanat.Add(viereiset);
            }
            else
            {
                // ilmoitetaan käyttäjälle virheestä
                RaiseSanaaEiLoytynytEvent(kokosana);
                // palautetaan null-lista, jotta ei anneta pisteitä virheellisistä sijoituksista
                return null;
            }

            //Tarkistetaan muodostaako jokin syötetyn sanan Kirjainlaatoista
            //'lisäsanoja' vasemmalla/oikealla olevien laattojen kanssa
            List<Kirjainlaatta> lisasana = new List<Kirjainlaatta>();

            foreach (Kirjainlaatta laatta in laatat)
            {
                lisasana.Clear();
                int sarake = laatta.PositionY;
                eka = laatta.PositionX;
                vika = laatta.PositionX;
                //r = GetChildAt(eka - 1, sarake);
                //Jos vasemmalle tai oikealla laattoja, lisätään tämä laatta listaan
                if ((r = GetChildAt(eka - 1, sarake)) != null && r.RuutuSisaltaaLaatan
                    || (GetChildAt(vika + 1, sarake)) != null && GetChildAt(vika + 1, sarake).RuutuSisaltaaLaatan)
                {
                    lisasana.Add(laatta);
                }
                else
                {
                    // siirry seuraavan laatan kohdalle
                    continue;
                }

                // vasemmalla olevat
                while ((r = GetChildAt(eka - 1, sarake)) != null && r.RuutuSisaltaaLaatan != false)
                {
                    eka = r.PositionX;
                    lisasana.Insert(0, r.GetKirjainlaatta());
                    //r = GetChildAt(eka - 1, sarake);
                }

                // oikealla olevat
                //r = GetChildAt(vika + 1, sarake);
                while ((r = GetChildAt(vika + 1, sarake)) != null && r.RuutuSisaltaaLaatan != false)
                {
                    vika = r.PositionX;
                    lisasana.Add(r.GetKirjainlaatta());
                    //r = GetChildAt(vika + 1, sarake);
                }

                // tarkistetaan muodostuiko hyväksyttyvä sana
                String uusisana = "";
                foreach (Kirjainlaatta kirjain in lisasana)
                {
                    uusisana += kirjain.Kirjain;
                }
                if (EtsiSanalistasta(uusisana))
                {
                    pisteytettavatSanat.Add(lisasana);
                }
                else
                {
                    // ilmoitetaan käyttäjälle virheestä; jos joku 'lisäsanoista'
                    // on virheellinen, kirjainlaatat eivät voi olla tässä muodostelmassa
                    // vaikka 'pääsana' olisi oikein
                    RaiseSanaaEiLoytynytEvent(uusisana);
                    // palautetaan null-lista, jotta ei anneta pisteitä virheellisistä sijoituksista
                    return null;
                }
            }
            return pisteytettavatSanat;
        }

        /// <summary>
        /// Tarkistaa löytyykö pelaajan lisäämien Kirjainlaattojen muodostama sana
        /// hyväksyttyjen sanojen listalta. Jos ei löytynyt, palauttaa false.
        /// </summary>
        /// <param name="laatat">lista Kirjainlaatoista jotka muodostavat sanan</param>
        /// <returns>true jos löytyi, muuten false</returns>
        private bool EtsiSanalistasta(String sana)
        {
            if (hyvaksytytSanat.Contains(sana)) return true;
            return false;
        }

        [Obsolete("Tätä metodia ei käytetä")]
        /// <summary>
        /// Rekursiivinen aliohjelma joka hakee PelilautaRuudun viereiset Kirjainlaatat
        /// ja lisää ne Kirjainlaattalistaan tarkistuksia ja pistelaskua varten
        /// </summary>
        /// <param name="edellinen">PelilautaRuutu jonka viereisiä haetaan</param>
        /// <param name="viereiset">lista laatoista jotka muodostavat sanan</param>
        /// <param name="suunta">mihin suuntaan etsitään</param>
        /// <remarks>int suunta: 1, 2, 3 tai 4
        /// 1 == ylös
        /// 2 == alas
        /// 3 == vasemmalle
        /// 4 == oikealle
        /// </remarks>
        private void HaeSeuraavaViereinen(PelilautaRuutu edellinen, List<Kirjainlaatta> viereiset, int suunta)
        {
            PelilautaRuutu ruutu = null;
            // ylös
            if (suunta == 1) ruutu = GetChildAt(edellinen.PositionX, edellinen.PositionY + 1);
            // alas
            if (suunta == 2) ruutu = GetChildAt(edellinen.PositionX, edellinen.PositionY - 1);
            // vasemmalle
            if (suunta == 3) ruutu = GetChildAt(edellinen.PositionX - 1, edellinen.PositionY);
            // oikealle
            if (suunta == 4) ruutu = GetChildAt(edellinen.PositionX + 1, edellinen.PositionY);
            if (ruutu == null || ruutu.RuutuSisaltaaLaatan != true) return;
            else
            {
                if (suunta == 1 || suunta == 3) viereiset.Insert(0, ruutu.GetKirjainlaatta());
                if (suunta == 2 || suunta == 4) viereiset.Add(ruutu.GetKirjainlaatta());
                HaeSeuraavaViereinen(ruutu, viereiset, suunta);
            }
        }

        #endregion
    }
}
