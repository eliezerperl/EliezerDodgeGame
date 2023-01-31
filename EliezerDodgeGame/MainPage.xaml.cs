using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EliezerDodgeGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        BoardManager board;
        DispatcherTimer timer;
        public MainPage()
        {
            this.InitializeComponent();
            board = new BoardManager(myCanvas);
            board.PlacePlayer();
        }
        private void start_game(object sender, RoutedEventArgs e)
        {
            board.StartupGame();
            EnableButtons();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += BoardTimer_Tick;
            timer.Start();
        }
        private void BoardTimer_Tick(object sender, object e)
        {
            if (board.PlayerWon() || board.PlayerLost())
                ResetSlidersToDefault();
        }


        public void EnableButtons() // Function to manage the buttons (enabling or collapsing) after start is pressed 
        {
            EnemySizeSlider.IsEnabled = true;
            EnemySpeedSlider.IsEnabled = true;
            PauseButton.IsEnabled = true;
            Save.IsEnabled = true;
            PlayButton.Visibility = Visibility.Collapsed;
            PlayButtonSeperator.Visibility = Visibility.Collapsed;
            NewButton.Visibility = Visibility.Visible;
            NewButtonSeperator.Visibility = Visibility.Visible;
        }


        private void exit_click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void checked_pause(object sender, RoutedEventArgs e)
        {
            board.Pause();
            PauseButton.Content = "Play";
        }
        private void unchecked_paused(object sender, RoutedEventArgs e)
        {
            board.Play();
            PauseButton.Content = "Pause";
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            board.SaveGame();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (timer == null)
            {
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                timer.Tick += BoardTimer_Tick;
            }
            timer.Start();
            if ((bool)PauseButton.IsChecked)
                PauseButton.IsChecked = false;
            EnableButtons();
            board.LoadGame();
            board.SetSpeedSliderValues(EnemySpeedSlider, PlayerSpeedSlider, EnemySizeSlider, PlayerSizeSlider);
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)PauseButton.IsChecked)
                PauseButton.IsChecked = false;
            board.Timer.Stop();
            board.StartNewGame();
            ResetSlidersToDefault();
        }


        // Player / Enemy Sliders Value Changed Events
        private void Enemy_Size(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (board != null)
                board.ChangeEnemySize(EnemySizeSlider.Value);
        }
        private void Enemy_Speed(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (board != null)
                board.ChangeEnemySpeed(EnemySpeedSlider.Value);
        }
        private void Player_Size(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (board != null)
                board.ChangePlayerSize(PlayerSizeSlider.Value);
        }
        private void Player_Speed(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (board != null)
                board.ChangePlayerSpeed(PlayerSpeedSlider.Value);
        }


        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetSlidersToDefault();
        }
        public void ResetSlidersToDefault() // Restore sliders to default starting values
        {
            EnemySizeSlider.Value = 0;
            EnemySpeedSlider.Value = 1;
            PlayerSizeSlider.Value = 0;
            PlayerSpeedSlider.Value = 5;
        }
    }
}
