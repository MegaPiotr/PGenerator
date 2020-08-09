using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Track> Tracks = new List<Track>();

        public event PropertyChangedEventHandler PropertyChanged;

        public ICollectionView Results { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            string path = Properties.Settings.Default.Path;
            this.tePath.Text = path;
            if(!string.IsNullOrWhiteSpace(path))
                LoadFiles(Properties.Settings.Default.Path);
        }

        private void Button_Run(object sender, RoutedEventArgs e)
        {
            KnapsackProblem problem = new KnapsackProblem(Tracks, tsTime.Value.Value);
            problem.MaxResultCount = 1000;
            Task t = Task.Run(async () =>
            {
                var result = await problem.Solve();
                try
                {
                    List<Track> tracks = new List<Track>();
                    int i = 0;
                    var hundred = result.Songs.OrderBy(h => h.GetHashCode()).Take(100);
                    foreach (var lis in hundred)
                    {
                        Playlist pl = new Playlist();
                        i++;
                        pl.Name = "lista_" + i;
                        pl.Songs = lis;
                        foreach (var track in lis)
                        {
                            Track tr = new Track(track) { Playlist = pl };
                            tracks.Add(tr);
                        }
                    }
                    Results = CollectionViewSource.GetDefaultView(tracks);

                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Playlist.Name");
                    Results.GroupDescriptions.Add(groupDescription);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Results"));
                    Dispatcher.Invoke((Action)(() => tbResultTime.Text = result.Time.ToString(@"hh\:mm\:ss")));
                }
                catch (Exception ex)
                {

                }
            });
        }

        private void Button_Path(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.ShowNewFolderButton = false;
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Tracks = new List<Track>();
                    tePath.Text = fbd.SelectedPath;
                    LoadFiles(fbd.SelectedPath);
                }
            }
        }

        private void LoadFiles(string path)
        {
            string[] files = Directory.GetFiles(path, "*.mp3");
            TimeSpan total = new TimeSpan();
            foreach (string file in files)
            {
                Track tr = new Track();
                var tfile = TagLib.File.Create(file);
                tr.Time = tfile.Properties.Duration;
                total += tr.Time;
                tr.Title = tfile.Tag.Title;
                tr.Path = file;
                tr.Name = System.IO.Path.GetFileName(file);
                Tracks.Add(tr);
            }
            lvTracks.ItemsSource = Tracks;
            tbTotal.Text = total.ToString(@"hh\:mm\:ss");
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button btn = sender as System.Windows.Controls.Button;
            //var group = (btn.Tag).Items();
            CollectionViewGroup gr = (CollectionViewGroup)btn.Tag;

            using (var folderBrowserDialog1 = new FolderBrowserDialog())
            {
                folderBrowserDialog1.SelectedPath = tePath.Text;
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string folderName = folderBrowserDialog1.SelectedPath;

                    foreach (Track file in gr.Items)
                    {
                        string newPath = System.IO.Path.Combine(folderName, file.Name);
                        File.Copy(file.Path, newPath);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.Path = tePath.Text;
            Properties.Settings.Default.Save();
        }
    }
    public class Track
    {
        public Track()
        {

        }
        public Track(Track track)
        {
            Path = track.Path;
            Title = track.Title;
            Name = track.Name;
            Time = track.Time;
            Playlist = track.Playlist;
        }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Length => Time.ToString(@"hh\:mm\:ss");
        public TimeSpan Time { get; set; }
        public int TotalSecunds => (int)Time.TotalSeconds;
        public Playlist Playlist { get; set; }
    }
    public class Playlist
    {
        public string Name { get; set; }
        public List<Track> Songs { get; set; }
    }
}
