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
    /// Interaction logic for KirjainlaattaPussi.xaml
    /// </summary>
    public partial class KirjainlaattaPussi : UserControl
    {
        #region Private property fields

        /// <summary>
        /// Kirjainlaattalista, jossa peliin luotuja Kirjainlaattoja säilytetään
        /// kunnes ne siirretään pelaajan käyttöön.
        /// </summary>
        private static List<Kirjainlaatta> kirjainlaatat = new List<Kirjainlaatta>();

        private static int _laattoja;
        
        /// <summary>
        /// Taulukko säännöistä, joiden mukaan Kirjainlaatat luodaan peliin.
        /// <see cref="KirjainlaattaSaannot"/>
        /// </summary>
        private KirjainlaattaSaannot[] kirjaimet = new KirjainlaattaSaannot[] {
            new KirjainlaattaSaannot("A", 10, 1),
            new KirjainlaattaSaannot("I", 10, 1),
            new KirjainlaattaSaannot("N", 9, 1),
            new KirjainlaattaSaannot("T", 9, 1),
            new KirjainlaattaSaannot("E", 8, 1),
            new KirjainlaattaSaannot("S", 7, 1),
            new KirjainlaattaSaannot("K", 5, 2),
            new KirjainlaattaSaannot("L", 5, 2),
            new KirjainlaattaSaannot("O", 5, 2),
            new KirjainlaattaSaannot("Ä", 5, 2),
            new KirjainlaattaSaannot("U", 4, 3),
            new KirjainlaattaSaannot("M", 3, 3),
            new KirjainlaattaSaannot("H", 2, 4),
            new KirjainlaattaSaannot("J", 2, 4),
            new KirjainlaattaSaannot("P", 2, 4),
            new KirjainlaattaSaannot("R", 2, 4),
            new KirjainlaattaSaannot("V", 2, 4),
            new KirjainlaattaSaannot("Y", 2, 4),
            new KirjainlaattaSaannot("D", 1, 7),
            new KirjainlaattaSaannot("Ö", 1, 7),
            new KirjainlaattaSaannot("B", 1, 8),
            new KirjainlaattaSaannot("F", 1, 8),
            new KirjainlaattaSaannot("G", 1, 8),
            new KirjainlaattaSaannot("C", 1, 10),
            new KirjainlaattaSaannot(" ", 2, 0)
        };

        #endregion

        #region Dependency properties

        /// <summary>
        /// DependencyProperty jossa säilytetään tietoa siitä kuinka monta Kirjainlaattaa
        /// pelissä on vielä jäljellä.
        /// </summary>
        public static readonly DependencyProperty LaattojaJaljellaProperty =
            DependencyProperty.Register("LaattojaJaljella", typeof(int), typeof(KirjainlaattaPussi),
            new FrameworkPropertyMetadata(_laattoja, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// Getter and setter for DependencyProperty 'LaattojaJaljella'
        /// </summary>
        public int LaattojaJaljella { get { return (int)GetValue(LaattojaJaljellaProperty); }
                                      set { SetValue(LaattojaJaljellaProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Muodostaja. Luodaan Kirjainlaatat peliin sääntöjen mukaisesti ja sekoitetaan
        /// ne sattumanvaraiseen järjestykseen.
        /// </summary>
        public KirjainlaattaPussi(List<SolidColorBrush> colors)
        {
            LuoKirjainLaatat(colors);
            _laattoja = kirjainlaatat.Count;
            Shuffle();
            InitializeComponent();
            LaattojaJaljella = _laattoja;
        }
        #endregion

        #region Private helper methods

        /// <summary>
        /// Alustaa kirjainlaatat peliin
        /// </summary>
        /// <param name="colors">värit jotka laatalle annetaan</param>
        private void LuoKirjainLaatat(List<SolidColorBrush> colors)
        {
            foreach (KirjainlaattaSaannot saanto in kirjaimet)
            {
                for (int i = 0; i < saanto.GetLkm(); i++)
                {
                    Kirjainlaatta laatta = new Kirjainlaatta(saanto.GetKirjain(), saanto.GetPistearvo());
                    laatta.Aktiivinen = true;
                    laatta.FillColor = colors[0];
                    laatta.BorderColor = colors[1];
                    laatta.InactiveFillColor = colors[2];
                    laatta.InactiveBorderColor = colors[3];
                    kirjainlaatat.Add(laatta);
                }
            }
        }

        /// <summary>
        /// Sekoittaa Kirjainlaatat sattumanvaraiseen järjestykseen käyttäen
        /// yksinkertaista toteutusta Fisher-Yates sekoitusalgoritmista.
        /// </summary>
        private void Shuffle()
        {
            Random random = new Random();
            for (int i = kirjainlaatat.Count; i > 1; i--)
            {
                int j = random.Next(i);
                Kirjainlaatta temp = kirjainlaatat[j];
                kirjainlaatat[j] = kirjainlaatat[i - 1];
                kirjainlaatat[i - 1] = temp;
            }
        }

        #endregion

        #region Public methods/helpers

        /// <summary>
        /// Palauttaa Kirjainlaatan joka löytyy kirjainlaattalistan (KirjainlaattaPussin)
        /// indeksistä index
        /// </summary>
        /// <param name="index">listan indeksi josta Kirjainlaattaa haetaan</param>
        /// <returns>Kirjainlaatan</returns>
        public Kirjainlaatta GetKirjainlaatta(int index)
        {
            if (index > kirjainlaatat.Count - 1) return null;
            return kirjainlaatat[index];
        }

        /// <summary>
        /// Palauttaa listan KirjainlaattaPussissa olevista Kirjainlaatoista
        /// </summary>
        /// <returns>listan KirjainlaattaPussissa olevista Kirjainlaatoista</returns>
        public List<Kirjainlaatta> GetKirjainlaatat()
        {
            return kirjainlaatat;
        }

        /// <summary>
        /// Poistaa KirjainlaattaPussista Kirjainlaatan joka löytyy indeksistä index
        /// ja päivittää jäljellä olevien Kirjainlaattojen määrää.
        /// </summary>
        /// <param name="index">listan indeksi josta Kirjainlaatta löytyy</param>
        public void Remove(int index)
        {
            if (index > kirjainlaatat.Count - 1) return;
            kirjainlaatat.RemoveAt(index);
            LaattojaJaljella -= 1;
        }

        /// <summary>
        /// Pistää annetun Kirjainlaatan takaisin KirjainlaattaPussin valikoimaan
        /// </summary>
        /// <param name="laatta">Kirjainlaatta joka laitetaan takaisin KirjainlaattaPussiin</param>
        public void PutBack(Kirjainlaatta laatta)
        {
            kirjainlaatat.Add(laatta);
            LaattojaJaljella++;
            // sekoitetaan pussin sisältö jotta takaisin pistetyt kirjainlaatat eivät ole aina viimeisinä
            Shuffle();
        }

        /// <summary>
        /// Tyhjentää KirjainlaattaPussin sisällön
        /// </summary>
        public void Clear()
        {
            kirjainlaatat.Clear();
        }

        #endregion
    }


    /// <summary>
    /// Apuriluokka, jolla yksittäisen Kirjainlaatan luomista koskevat
    /// säännöt voidaan esittää helposti ymmärrettävässä ja käytettävässä muodossa.
    /// </summary>
    public class KirjainlaattaSaannot
    {
        #region Private properties
        /// <summary>Kirjainlaatan sisältämä kirjain</summary>
        private String _kirjain = "";
        /// <summary>Kuinka monta tällaista Kirjainlaattaa peliin pitää luoda</summary>
        private int _lkm = 2;
        /// <summary>Kirjainlaatan sisältämä pistearvo</summary>
        private int _pistearvo = 0;
        #endregion

        #region Constructors
        /// <summary>Oletusmuodostaja</summary>
        public KirjainlaattaSaannot() { }

        /// <summary>
        /// Muodostaja.
        /// </summary>
        /// <param name="kirjain">Kirjain, joka luotavalle Kirjainlaatalle annetaan</param>
        /// <param name="lkm">Kuinka monta tällaista Kirjainlaattaa peliin pitää luoda</param>
        /// <param name="pistearvo">Pistearvo, joka luotavalle Kirjainlaatalle annetaan</param>
        public KirjainlaattaSaannot(string kirjain, int lkm, int pistearvo)
        {
            this._kirjain = kirjain;
            this._lkm = lkm;
            this._pistearvo = pistearvo;
        }
        #endregion

        #region Public accessors
        /// <summary>
        /// Palauttaa kokonaisluvun, joka ilmaisee kuinka monta Kirjainlaatta pitää luoda tämän säännön mukaan
        /// </summary>
        /// <returns>kokonaisluvun, joka ilmaisee kuinka monta Kirjainlaatta pitää luoda tämän säännön mukaan
        /// </returns>
        public int GetLkm() { return _lkm; }
        /// <summary>
        /// Palauttaa kokonaisluvun, joka ilmaisee tämän säännön mukaisten Kirjainlaattojen pistearvon
        /// </summary>
        /// <returns>kokonaisluvun tämän säännön mukaisten Kirjainlaattojen pistearvosta</returns>
        public int GetPistearvo() { return _pistearvo; }
        /// <summary>
        /// Palauttaa kirjaimen, joka tämän säännön mukaisesti luoduille Kirjainlaatoille annetaan
        /// </summary>
        /// <returns>kirjaimen, joka tämän säännön mukaisesti luoduille Kirjainlaatoille annetaan</returns>
        public string GetKirjain() { return _kirjain; }
        #endregion
    }
    
}
