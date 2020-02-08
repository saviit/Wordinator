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
    /// Interaction logic for Aikalaskuri.xaml
    /// </summary>
    public partial class Aikalaskuri : UserControl
    {
        #region Private properties
        /// <summary>
        /// Internal timer for countdown/countup events
        /// </summary>
        private DispatcherTimer _internalTimer;
        /// <summary>
        /// Store for current/remaining time in the count-up/countdown
        /// </summary>
        private TimeSpan _timeLeft;
        /// <summary>
        /// Format string for displaying current/remaining time
        /// </summary>
        private String _timeFormat = @"mm\:ss";
        #endregion

        #region Dependency Properties
        /// <summary>
        /// DependencyProperty to determine whether timer is counting up or down
        /// </summary>
        public static DependencyProperty IsDescendingTimerProperty =
                               DependencyProperty.Register("IsDescendingTimer",
                                                           typeof(Boolean),
                                                           typeof(Aikalaskuri),
                                                           new FrameworkPropertyMetadata(true));
        public Boolean IsDescendingTimer
        {
            get { return (Boolean)GetValue(IsDescendingTimerProperty); }
            set { SetValue(IsDescendingTimerProperty, value); }
        }
        #endregion

        #region Routed Events
        /// <summary>
        /// Routed event joka ilmoittaa ylöspäin kun aika on loppunut
        /// </summary>
        public static readonly RoutedEvent AikalaskuriAlarmEvent =
            EventManager.RegisterRoutedEvent("AikalaskuriAlarm", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(Aikalaskuri));

        public event RoutedEventHandler AikalaskuriAlarm
        {
            add { AddHandler(AikalaskuriAlarmEvent, value); }
            remove { RemoveHandler(AikalaskuriAlarmEvent, value); }
        }

        void RaiseAikalaskuriAlarmEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(Aikalaskuri.AikalaskuriAlarmEvent);
            RaiseEvent(newEventArgs);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs a default countdown timer component with a 
        /// starting time (time left) of 60 seconds. The component
        /// displays the current time left in a label and automatically
        /// decreases that time whenever the timer is running.
        /// </summary>
        public Aikalaskuri()
        {
            InitializeComponent();
            _internalTimer = new DispatcherTimer();
            _internalTimer.Interval = TimeSpan.FromSeconds(1);
            _internalTimer.Tick +=new EventHandler(_internalTimer_Tick);
            // default starting value for countdown is 1 minute
            _timeLeft = TimeSpan.FromSeconds(60);
            aikanaytto.Content = _timeLeft.ToString(_timeFormat);
        }


        /// <summary>
        /// Constructs a new countdown/timer component with the
        /// specified starting time (time left).
        /// </summary>
        /// <param name="seconds">time left in the countdown</param>
        public Aikalaskuri(int seconds)
        {
            InitializeComponent();
            //_isDescendingTimer = isDescending;
            _internalTimer = new DispatcherTimer();
            _internalTimer.Interval = TimeSpan.FromSeconds(1);
            _internalTimer.Tick += new EventHandler(_internalTimer_Tick);
            // set starting countdown value
            _timeLeft = TimeSpan.FromSeconds(seconds);
            aikanaytto.Content = _timeLeft.ToString(_timeFormat);
        }
        #endregion

        #region Public methods to control the timer
        
        /// <summary>
        /// Starts/resumes the timer.
        /// </summary>
        public void Start()
        {
            _internalTimer.Start();
        }

        /// <summary>
        /// Stops/pauses the timer.
        /// </summary>
        public void Stop()
        {
            _internalTimer.Stop();
        }

        /// <summary>
        /// Gets the current time left in the countdown timer
        /// </summary>
        /// <returns>a TimeSpan object that represents the current time left in the countdown</returns>
        public TimeSpan GetTimeLeft()
        {
            return _timeLeft;
        }

        /// <summary>
        /// Adds the specified number of seconds to the current countdown timer and
        /// updates the countdown display.
        /// </summary>
        /// <param name="seconds">number of seconds to add</param>
        public void IncreaseTime(int seconds)
        {
            _timeLeft += TimeSpan.FromSeconds(seconds);
            UpdateCountdownDisplay();
        }

        /// <summary>
        /// Subtracts the specified number of seconds from the current countdown timer
        /// and updates the countdown display.
        /// </summary>
        /// <param name="seconds">number of seconds to subtract</param>
        public void DecreaseTime(int seconds)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(seconds));
            UpdateCountdownDisplay();
        }
        #endregion

        #region Private helper methods
        /// <summary>
        /// Updates the countdown display.
        /// </summary>
        private void UpdateCountdownDisplay()
        {
            aikanaytto.Content = _timeLeft.ToString(_timeFormat);
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// EventHandler for timer Tick-event. Decreases the time left
        /// until it reaches zero.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void _internalTimer_Tick(object sender, EventArgs e)
        {
            // available time cannot be negative and available time
            // should not exceed 60 minutes since the countdown display
            // only shows minutes and seconds
            if (_timeLeft > TimeSpan.Zero && _timeLeft.Minutes < 60)
            {
                if (IsDescendingTimer) DecreaseTime(1);
                else IncreaseTime(1);
            }
            else
            {
                _internalTimer.Stop();
                RaiseAikalaskuriAlarmEvent();
            }
        }
        #endregion
    }
    
}
