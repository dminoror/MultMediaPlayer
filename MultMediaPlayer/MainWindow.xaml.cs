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
using System.IO;
using WMPLib;

namespace MultMediaPlayer
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        WindowsMediaPlayer[] wmp = new WindowsMediaPlayer[3];

        BitmapImage imagePlay;
        BitmapImage imagePause;
        BitmapImage imageStop;

        bool[] isPlaying = new bool[] { false, false, false };
        bool isAllPlaying = false;
        int[] indexPlaying = new int[] { 0, 0, 0 };
        List<FileInfo> mediaFiles;

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            imagePlay = new BitmapImage(new Uri("Image/play.png", UriKind.Relative));
            imagePause = new BitmapImage(new Uri("Image/pause.png", UriKind.Relative));
            imageStop = new BitmapImage(new Uri("Image/stop.png", UriKind.Relative));
            wmp[0] = new WindowsMediaPlayer();
            wmp[0].settings.setMode("loop", true);
            wmp[1] = new WindowsMediaPlayer();
            wmp[1].settings.setMode("loop", true);
            wmp[2] = new WindowsMediaPlayer();
            wmp[2].settings.setMode("loop", true);
            mediaFiles = new List<FileInfo>();
        }

        private void Files_Drop(object sender, DragEventArgs e)
        {
            mediaFiles.Clear();
            var paths = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
            StringBuilder builder = new StringBuilder();
            char[] spliter = new char[] { '.' };
            foreach (var obj in paths)
            {
                string path = obj.ToString();
                if (!File.Exists(path)) { continue; }
                FileInfo file = new FileInfo(path);
                string exten = file.Extension.ToLower();
                if (exten == ".mp3" || exten == ".wav")
                {
                    mediaFiles.Add(file);
                    builder.AppendLine(file.Name);
                }
            }
            MessageBox.Show(builder.ToString());
            indexPlaying[0] = 0;
            indexPlaying[1] = 0;
            indexPlaying[2] = 0;
            if (mediaFiles.Count > 0)
            {
                txtTrack1.Text = mediaFiles[0].Name;
                txtTrack2.Text = mediaFiles[0].Name;
                txtTrack3.Text = mediaFiles[0].Name;
            }
        }

        void TrackPrev(int index, TextBlock txt)
        {
            indexPlaying[index]--;
            if (indexPlaying[index] < 0)
            { indexPlaying[index] = mediaFiles.Count - 1; }
            txt.Text = mediaFiles[indexPlaying[index]].Name;
        }
        void TrackNext(int index, TextBlock txt)
        {
            indexPlaying[index]++;
            if (indexPlaying[index] >= mediaFiles.Count)
            { indexPlaying[index] = 0; }
            txt.Text = mediaFiles[indexPlaying[index]].Name;
        }

        private void Track1Prev_Click(object sender, RoutedEventArgs e)
        {
            TrackPrev(0, txtTrack1);
        }
        private void Track1Next_Click(object sender, RoutedEventArgs e)
        {
            TrackNext(0, txtTrack1);
        }
        private void Track2Prev_Click(object sender, RoutedEventArgs e)
        {
            TrackPrev(1, txtTrack2);
        }
        private void Track2Next_Click(object sender, RoutedEventArgs e)
        {
            TrackNext(1, txtTrack2);
        }
        private void Track3Prev_Click(object sender, RoutedEventArgs e)
        {
            TrackPrev(2, txtTrack3);
        }
        private void Track3Next_Click(object sender, RoutedEventArgs e)
        {
            TrackNext(2, txtTrack3);
        }

        void TrackStatusToggle(int index, Image image)
        {
            isPlaying[index] = !isPlaying[index];
            if (isPlaying[index])
            {
                image.Source = imageStop;
                wmp[index].URL = mediaFiles[indexPlaying[index]].FullName;
                wmp[index].controls.play();
                isAllPlaying = true;
            }
            else
            {
                image.Source = imagePlay;
                wmp[index].controls.stop();
            }
        }

        private void Track1Status_Click(object sender, RoutedEventArgs e)
        {
            TrackStatusToggle(0, imageTrack1Status);
        }
        private void Track2Status_Click(object sender, RoutedEventArgs e)
        {
            TrackStatusToggle(1, imageTrack2Status);
        }
        private void Track3Status_Click(object sender, RoutedEventArgs e)
        {
            TrackStatusToggle(2, imageTrack3Status);
        }

        private void StopAll_Click(object sender, RoutedEventArgs e)
        {
            isPlaying[0] = false;
            isPlaying[1] = false;
            isPlaying[2] = false;
            wmp[0].controls.stop();
            wmp[1].controls.stop();
            wmp[2].controls.stop();
            imageTrack1Status.Source = imagePlay;
            imageTrack2Status.Source = imagePlay;
            imageTrack3Status.Source = imagePlay;
        }

        private void StatusAll_Click(object sender, RoutedEventArgs e)
        {
            if (isAllPlaying)
            {
                isPlaying[0] = false;
                isPlaying[1] = false;
                isPlaying[2] = false;
                wmp[0].controls.pause();
                wmp[1].controls.pause();
                wmp[2].controls.pause();
                imageTrack1Status.Source = imagePlay;
                imageTrack2Status.Source = imagePlay;
                imageTrack3Status.Source = imagePlay;
                imageAllStatus.Source = imagePlay;
            }
            else
            {
                isPlaying[0] = true;
                isPlaying[1] = true;
                isPlaying[2] = true;
                wmp[0].controls.play();
                wmp[1].controls.play();
                wmp[2].controls.play();
                imageTrack1Status.Source = imageStop;
                imageTrack2Status.Source = imageStop;
                imageTrack3Status.Source = imageStop;
                imageAllStatus.Source = imagePause;
            }
            isAllPlaying = !isAllPlaying;
        }

    }
}
