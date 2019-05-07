using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Media;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace MediaLab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer player = new MediaPlayer();
        //        SpeechSynthesizer synth = new SpeechSynthesizer();
        DispatcherTimer Timer = new DispatcherTimer();
        SoundPlayer sp = new SoundPlayer();
       

        public MainWindow()
        {
            InitializeComponent();            
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            volume.Value = 10;
             sp.Stream = Properties.Resources.hitmarker;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timeline.Value = (int)mpe.Position.TotalMilliseconds;

                lb.Content = ((string)mpe.Position.ToString("hh':'mm':'ss") + "/" + (string)mpe.NaturalDuration.TimeSpan.ToString("hh':'mm':'ss"));
            }
            catch 
            {

            }               

            //lb.Content = (((int)mpe.Position.TotalSeconds + "/" + mpe.NaturalDuration);
        }

        private void timeline_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            int SliderValue = (int)timeline.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            mpe.Position = ts;

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            player.Open(new Uri(dlg.FileName, UriKind.Relative));
            mpe.Source = new Uri(dlg.FileName, UriKind.Relative);
            mpe.Volume = 100.0 / 100.0;
            mpe.MediaOpened += mpe_MediaOpened;
            mpe.MediaEnded += mpe_Media_Ended;

            mpe.Play();
            Timer.Start();
        }

        private void mpe_Media_Ended(object sender, RoutedEventArgs e)
        {
            mpe.Stop();
            player.Stop();
            timeline.Value = 0;
        }

        private void mpe_MediaOpened(object sender, RoutedEventArgs e)
        {     //где timeline – компонент типа Slider
            sp.Play();
            timeline.Maximum = mpe.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
               mpe.Play();
            sp.Play();
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            mpe.Pause();
            Timer.Tick -= new EventHandler(dispatcherTimer_Tick);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            SystemSounds.Asterisk.Play();
            timeline.Value = 0;
            lb.Content = "00:00:00/00:00:00";
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 0);
            mpe.Position = ts;
            player.Position = ts;
            mpe.Stop();
        }

        private void Volume_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            mpe.Volume = (volume.Value/10);
        }
    }
}
