using Blackjack.Application;
using Blackjack.Infrastructure;
using Blackjack.Wpf.ViewModels;
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

namespace Blackjack.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GameViewModel ViewModel { get; }
        public MainWindow()
        {
            InitializeComponent();

            Randomizer randomizer = new Randomizer();
            GameService gameService = new GameService(randomizer);

            ViewModel = new GameViewModel(gameService);

            DataContext = ViewModel;
        }

        private void StartGame_Pressed(object sender, RoutedEventArgs e)
        {
            ViewModel.StartGame("Nicholai");
        }

        private void Hit_Pressed(object sender, RoutedEventArgs e)
        {
            ViewModel.Hit();
        }

        private void Stand_Pressed(object sender, RoutedEventArgs e)
        {
            ViewModel.Stand();
        }
    }
}