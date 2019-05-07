using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Media;

namespace Audio
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer player = new MediaPlayer();
        DispatcherTimer Timer = new DispatcherTimer();
        
        Dictionary<string, string> list = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Interval = new TimeSpan(0, 0, 0, 1);
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
            volume.Value = 10;            player.Volume = 1;
            SystemSounds.Asterisk.Play();
        }
        public int[] x;
        int i = 0;
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "mp3 |*.MP3";
            dlg.Multiselect = true;
            dlg.ShowDialog();

            foreach (string file in dlg.FileNames)
            {
            
                string text = file.ToString().Remove(0, 17);
                //Spisok.Items.Add(dlg.FileName.ToString());
                list.Add(text, file.ToString());
                Spisok.Items.Add(text);
                

            }

            x = new int[Spisok.Items.Count];
            zap(ref x);
        }

        public void zap(ref int[] x)
        {
            for (int i = 0; i<Spisok.Items.Count; i++)
            {
                x[i] = i;
            }
        }

        private void timeline_ValueChanged(object sender, DragCompletedEventArgs e)
        {
            int SliderValue = (int)timeline.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            player.Position = ts;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timeline.Value = (int)player.Position.TotalMilliseconds;

                lb.Content = ((string)player.Position.ToString("hh':'mm':'ss") + "/" + (string)player.NaturalDuration.TimeSpan.ToString("hh':'mm':'ss"));
            }
            catch
            {
               
            }

        }
        private void Spisok_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Spisok.SelectedIndex != -1)
            {
                timeline.Value = 0;
                player.Open(new Uri(list[Spisok.SelectedItem.ToString()], UriKind.Relative));
                player.Play();
                player.MediaOpened += MediaOpened;
                player.MediaEnded += player_Media_Ended;
                Timer.Start();
            }
            else
            {

                lb.Content = "00:00:00/00:00:00";
            }

        }


        private void MediaOpened(object sender, EventArgs e)
        {
            timeline.Maximum = player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void player_Media_Ended(object sender, EventArgs e)
        {
            if (Rand.IsChecked == false & Spisok.Items.Count>=1)
            {
                if (Spisok.SelectedIndex != (Spisok.Items.Count - 1))
                {
                    player.Stop();
                    (Spisok.SelectedIndex)++;
                    timeline.Value = 0;
                    player.Play();
                }
                else
                {
                    timeline.Value = 0;
                    player.Stop();
                    Spisok.SelectedIndex = -1;
                    Timer.Stop();
                }
            }
            else
            {
                if (i < Spisok.Items.Count)
                {
                    Spisok.SelectedIndex = x[i];
                    i++;

                }
            //Gog:
                
                //Random random = new Random();
                //int i = random.Next(0, Spisok.Items.Count);
                //if (i == Spisok.SelectedIndex)
                //{
                //    goto Gog;
                //}
                //else
                //{
                //    Spisok.SelectedIndex = i;
                    
                //}

                

            }
        }


        
        
    

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            player.Play();  
            Timer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            player.Pause();
            Timer.Tick -= new EventHandler(dispatcherTimer_Tick);
            SystemSounds.Asterisk.Play();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
            timeline.Value = 0;

            lb.Content = "00:00:00/00:00:00";
            SystemSounds.Asterisk.Play();
        }

        private void Volume_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            player.Volume = (volume.Value / 10);
        }

        private void Timeline_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            int SliderValue = (int)timeline.Value;

            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            player.Position = ts;
        }
    }
}
