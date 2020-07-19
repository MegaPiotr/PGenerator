using System;
using System.Collections.Generic;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            KnapsackProblem problem = new KnapsackProblem(new List<int> { 9, 7, 7, 9, 8, 10 }, 20);
            var result = problem.Solve();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    List<Track> Tracks = new List<Track>();
                    tePath.Text = fbd.SelectedPath;
                    string[] files = Directory.GetFiles(fbd.SelectedPath, "*.mp3");
                    foreach(string file in files)
                    {
                        Track tr = new Track();
                        var tfile = TagLib.File.Create(file);
                        tr.Time = tfile.Properties.Duration;
                        tr.Title = tfile.Tag.Title;
                        tr.Path = file;
                        tr.Name = System.IO.Path.GetFileNameWithoutExtension(file);
                        Tracks.Add(tr);
                    }
                    lvTracks.ItemsSource = Tracks;
                }
            }
        }
    }
    public class Track
    {
        public string Path { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Length => Time.ToString(@"hh\:mm\:ss");
        public TimeSpan Time { get; set; }
    }
}
