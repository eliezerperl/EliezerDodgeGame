using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;

namespace EliezerDodgeGame
{
    internal class Player
    {
        DispatcherTimer timer;
        Image player_img;
        double playerxAxis;
        double playeryAxis;
        double playerspeed;
        double playerWidth;
        double playerHeight;
        bool left, right, up, down;

        public DispatcherTimer Timer { get { return timer; } }
        public Image Player_img { get { return player_img; } }
        public double PlayerY { get { return playeryAxis; } set { playeryAxis = value; } }
        public double PlayerX { get { return playerxAxis; } set { playerxAxis = value; } }
        public double PlayerSpeed { get { return playerspeed; } set { playerspeed = value; } }
        public double PlayerW { get { return playerWidth; } set { playerWidth = value; player_img.Width = value; } }
        public double PlayerH { get { return playerHeight; } set { playerHeight = value; player_img.Height = value; } }


        public Player()
        {
            player_img = new Image();
            playerxAxis = 900;
            playeryAxis = 400;
            playerspeed = 5;
            playerHeight = 80;
            playerWidth = 67;

            Window.Current.CoreWindow.KeyDown += player_KeyDown;
            Window.Current.CoreWindow.KeyUp += player_KeyUp;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += PlayerMove_Tick;
            timer.Start();
        }
        private void PlayerMove_Tick(object sender, object e)
        {
            PlayerMovementDirection();
        }
        private void player_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.W)
                up = false;
            if (args.VirtualKey == Windows.System.VirtualKey.S)
                down = false;
            if (args.VirtualKey == Windows.System.VirtualKey.A)
                left = false;
            if (args.VirtualKey == Windows.System.VirtualKey.D)
                right = false;

        }
        private void player_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.W)
                up = true;
            if (args.VirtualKey == Windows.System.VirtualKey.S)
                down = true;
            if (args.VirtualKey == Windows.System.VirtualKey.A)
                left = true;
            if (args.VirtualKey == Windows.System.VirtualKey.D)
                right = true;


        }
        public void PlayerMovementDirection()
        {
            if (up == true && playeryAxis > 0)
                Canvas.SetTop(player_img, playeryAxis - playerspeed);
            if (down == true && playeryAxis < 890)
                Canvas.SetTop(player_img, playeryAxis + playerspeed);
            if (left == true && playerxAxis > -15)
                Canvas.SetLeft(player_img, playerxAxis - playerspeed);
            if (right == true && playerxAxis < 1865)
                Canvas.SetLeft(player_img, playerxAxis + playerspeed);

            playerxAxis = Canvas.GetLeft(player_img);
            playeryAxis = Canvas.GetTop(player_img);
        }
        public bool IsPlayerOnCanvas(Canvas canvas)
        {
            if (canvas.Children.Contains(player_img))
                return true;
            return false;
        }
        public Image AddPlayer()
        {
            player_img.Name = "Player1";
            player_img.Height = playerHeight;
            player_img.Width = playerWidth;
            player_img.Source = new BitmapImage(new Uri("ms-appx:///Assets/zombie.png"));
            Canvas.SetLeft(player_img, playerxAxis);
            Canvas.SetTop(player_img, playeryAxis);
            return player_img;
        }
        public void ResetPlayerMovement()
        {
            up = false;
            down = false;
            right = false;
            left = false;
        }
    }
}
