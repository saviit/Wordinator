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
    /// Interaction logic for Pistelaskuri.xaml
    /// </summary>
    public partial class Pistelaskuri : UserControl
    {

        private static int startingPoints = 0;

        #region Dependency properties

        /// <summary>
        /// Dependency property for the player's current total points
        /// </summary>
        public static DependencyProperty TotalPointsProperty =
            DependencyProperty.Register("TotalPoints",
                                        typeof(int),
                                        typeof(Pistelaskuri),
                                        new FrameworkPropertyMetadata(startingPoints,
                                            FrameworkPropertyMetadataOptions.AffectsRender,
                                            new PropertyChangedCallback(OnTotalPointsChanged)));

        public int TotalPoints
        {
            get { return (int)GetValue(TotalPointsProperty); }
            set { SetValue(TotalPointsProperty, value); }
        }


        private static void OnTotalPointsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            Pistelaskuri laskuri = (Pistelaskuri)obj;
            laskuri.pistenaytto.Content = args.NewValue.ToString();
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructs a new points display
        /// </summary>
        public Pistelaskuri()
        {
            InitializeComponent();
        }

        #endregion


        #region Public methods/helpers

        /// <summary>
        /// Sets the player's points to the specified number
        /// </summary>
        /// <remarks>Used when the points display needs to be reset</remarks>
        /// <param name="points">value to be set</param>
        public void SetPoints(int points)
        {
            TotalPoints = points;
        }

        /// <summary>
        /// Adds the specified amount of points to the player's total points
        /// </summary>
        /// <param name="points">number of points to be added to the current total</param>
        public void IncreasePoints(int points)
        {
            TotalPoints += points;
        }

        /// <summary>
        /// Subtracts the specified number of points from the player's current total
        /// </summary>
        /// <param name="points">the number of points to be subtracted</param>
        public void DecreasePoints(int points)
        {
            TotalPoints -= points;
        }

        #endregion

    }
    
}
