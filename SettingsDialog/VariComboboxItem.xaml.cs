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

namespace SettingsDialog
{
    /// <summary>
    /// Interaction logic for VariComboboxItem.xaml.
    /// </summary>
    public partial class VariComboboxItem : UserControl
    {

        /// <summary>
        /// DependencyProperty jossa säilytetään VariComboboxItemin edustamaa väriarvoa
        /// </summary>
        public static readonly DependencyProperty VariProperty =
            DependencyProperty.Register("Vari", typeof(SolidColorBrush), typeof(VariComboboxItem),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.White), FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnVariChanged)));

        public SolidColorBrush Vari
        {
            get { return (SolidColorBrush)GetValue(VariProperty); }
            set { SetValue(VariProperty, value); }
        }

        private static void OnVariChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            VariComboboxItem item = (VariComboboxItem)obj;
            item.variRectangle.Fill = (SolidColorBrush)args.NewValue;
        }

        /// <summary>
        /// DependencyProperty jossa säilytetään VariComboboxItemin edustaman väriarvon nimeä
        /// </summary>
        public static readonly DependencyProperty TekstiProperty =
            DependencyProperty.Register("Teksti", typeof(String), typeof(VariComboboxItem),
            new FrameworkPropertyMetadata("White", FrameworkPropertyMetadataOptions.AffectsRender,
                new PropertyChangedCallback(OnTekstiChanged)));

        public String Teksti
        {
            get { return (String)GetValue(TekstiProperty); }
            set { SetValue(TekstiProperty, value); }
        }

        private static void OnTekstiChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            VariComboboxItem item = (VariComboboxItem)obj;
            item.variLabel.Content = (String)args.NewValue;
        }

        #region Constructors

        /// <summary>
        /// Oletusmuodostaja
        /// </summary>
        public VariComboboxItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muodostaja
        /// </summary>
        /// <param name="vari">Väri joka asetetaan</param>
        /// <param name="varinNimi">Asetettavan värin nimi</param>
        public VariComboboxItem(SolidColorBrush vari, String varinNimi)
        {
            InitializeComponent();
            this.Vari = vari;
            this.Teksti = varinNimi;
        }

        #endregion
    }
}
