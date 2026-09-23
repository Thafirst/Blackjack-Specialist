using Blackjack.Application.Services;
using Blackjack.Infrastructure;
using Blackjack.Infrastructure.Data;
using Blackjack.Infrastructure.Repositories;
using Blackjack.Infrastructure.Security;
using Blackjack.Wpf.ViewModels;
using Microsoft.EntityFrameworkCore;
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
        public UserService UserService { get; }
        public MainWindow()
        {
            InitializeComponent();

            Randomizer randomizer = new Randomizer();
            GameService gameService = new GameService(randomizer);

            ViewModel = new GameViewModel(gameService);

            DataContext = ViewModel;

            BlackjackDbContext context = BlackjackDbContextFactory.Create();

            context.Database.Migrate();

            UserRepository userRepository = new UserRepository(context);
            BCryptPasswordHasher passwordHasher = new BCryptPasswordHasher();
            
            UserService = new UserService(userRepository, passwordHasher);
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

        private void Login_Pressed(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            bool loginSuccessful = UserService.Login(username, password);

            if (loginSuccessful)
                LoginStatusText.Text = "Login Successful!";
            else
                LoginStatusText.Text = "Invalid username or password.";
        }

        private void Register_Pressed(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            try
            {
                UserService.Register(username, password);

                bool loginSuccessful = UserService.Login(username, password);

                if(loginSuccessful)
                    LoginStatusText.Text = $"Registration Successful, and logged in as {username}!";
            }
            catch (InvalidOperationException)
            {
                LoginStatusText.Text = "That username already exists.";
            }
            catch (ArgumentException)
            {
                LoginStatusText.Text = "Username or password fields cannot be empty.";
            }

        }
    }
}