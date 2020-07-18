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
            KnapsackProblem problem = new KnapsackProblem();
            problem.N = 6;
            problem.K = 20;
            problem.waga = new int[] { 9, 7, 7, 9, 8, 10 };
            problem.wartosc = new int[] { 9, 7, 7, 9, 8, 10 };
            problem.Solve();
        }
    }
}
