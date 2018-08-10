using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Scene3DLib
{
    /// <summary>
    /// Provides a borderless window in the shape of a speech bubble to populate with custom content.
    /// </summary>
    public partial class CalloutWindow : Window
    {
        /// <summary>
        /// Overrides metadata of existing DependencyProperty elements.
        /// </summary>
        static CalloutWindow()
        {
            BorderBrushProperty.OverrideMetadata(typeof(CalloutWindow), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromArgb(255, 238, 156, 88))));
            HorizontalContentAlignmentProperty.OverrideMetadata(typeof(CalloutWindow), new FrameworkPropertyMetadata(HorizontalAlignment.Center));
            VerticalContentAlignmentProperty.OverrideMetadata(typeof(CalloutWindow), new FrameworkPropertyMetadata(VerticalAlignment.Center));
        }

        /// <summary>
        /// Initialises a new empty BorderlessWindow object with default values.
        /// </summary>
        /// 
        public CalloutWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Represents the Brush object to color the background of the window with.
        /// </summary>
        public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(CalloutWindow), new PropertyMetadata(new LinearGradientBrush(Colors.White, Color.FromArgb(255, 250, 191, 143), 90)));

        /// <summary>
        /// Gets or sets the Brush object to color the background of the window with.
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public static void showHint(string txt, int time, Window hint4Window)
        {
            CalloutWindow cw = new CalloutWindow();
            cw.Width    = 225;
            cw.Height   = 120;
            cw.FontSize = 12;
            cw.Padding = new Thickness(3);
            cw.Content = txt;
            double TopEnd = 0.0;
            {
                cw.Left = hint4Window.Left - cw.Width;
                cw.Top = hint4Window.Top + (hint4Window.Height / 9);
                TopEnd = hint4Window.Top + (hint4Window.Height - (hint4Window.Height / 6));
            }
            cw.Show();
            hint4Window.Focus();

            {
                Storyboard sb = new Storyboard();
                DoubleAnimation anim = new DoubleAnimation();
                sb.Children.Add(anim);
                Storyboard.SetTarget(anim, cw);
                Storyboard.SetTargetProperty(anim, new PropertyPath("Top"));
                anim.To = TopEnd;
                Duration duration = new Duration(TimeSpan.FromSeconds(time / 1000));
                anim.Duration = duration;
                cw.BeginAnimation(Window.TopProperty, anim);
            }

            Task.Delay(time).ContinueWith(t2 => {

                cw.Close();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}