using Solver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPyatnashki
{
    /// <summary>
    /// Interaction logic for SolverForm.xaml
    /// </summary>
    public partial class SolverForm : Window
    {
        private CancellationTokenSource cancellationTokenSource;

        public SolverForm()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
        }

        private async void btnstop_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var blocks = new int[][]
                {
                    new int[] {int.Parse(txtbox00.Text) , int.Parse(txtbox01.Text) },
                    new int[] { int.Parse(txtbox10.Text) , int.Parse(txtbox11.Text)},
                    new int[] { int.Parse(txtbox20.Text), int.Parse(txtbox21.Text) }
                };
            await CalculateSolution(blocks, cancellationTokenSource.Token);
        }

        private async Task CalculateSolution(int[][] blocks, CancellationToken token)
        {
            var board = new Board(blocks);
            var solver = new Solver.Solver(board,token);
            listbox.Dispatcher.Invoke(() =>
            {
                listbox.ItemsSource = solver.solution();
            });
        }
    }
}
