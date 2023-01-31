using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace EliezerDodgeGame
{
    internal class Enemy
    {
        static Random rnd = new Random();
        Image enemy_img;
        double enemyXaxis;
        double enemyYaxis;
        double enemyWidth;
        double enemyHeight;
        double enemyspeed;


        public Image Enemy_Img { get { return enemy_img; } set { enemy_img = value; } }
        public double EnemyW { get { return enemyWidth; } set { enemyWidth = value; enemy_img.Width = value; } }
        public double EnemyH { get { return enemyHeight; } set { enemyHeight = value; enemy_img.Height = value; } }
        public double EnemyX { get { return enemyXaxis; } set { enemyXaxis = value; } }
        public double EnemyY { get { return enemyYaxis; } set { enemyYaxis = value; } }
        public double EnemySpeed { get { return enemyspeed; } set { enemyspeed = value; } }

        public Enemy()
        {
            enemyHeight = 80;
            enemyWidth = 67;
            enemyspeed = 1;
            AddEnemy();
            PlaceEnemy();

        }
        public Image AddEnemy()
        {
            int x = rnd.Next(4);
            enemy_img = new Image();
            enemy_img.Width = enemyWidth;
            enemy_img.Height = enemyHeight;
            enemy_img.Source = new BitmapImage(new Uri($"ms-appx:///Assets/pacmanenemy{x}.png"));
            return enemy_img;
        }
        public void PlaceEnemy()
        {
            enemyXaxis = rnd.Next(1820);
            enemyYaxis = rnd.Next(870);
            Canvas.SetLeft(enemy_img, enemyXaxis);
            Canvas.SetTop(enemy_img, enemyYaxis);
        }

        public void EnemyMovement(Player player)
        {
            enemyXaxis = Canvas.GetLeft(enemy_img);
            enemyYaxis = Canvas.GetTop(enemy_img);
            double enemyX = enemyXaxis;
            double enemyY = enemyYaxis;
            double playerX = player.PlayerX;
            double playerY = player.PlayerY;

            if (enemyX < playerX)
                enemyX = enemyX + enemyspeed;
            else if (enemyX == playerX)
                enemyX = playerX;
            else
                enemyX = enemyX - enemyspeed;
            if (enemyY < playerY)
                enemyY = enemyY + enemyspeed;
            else if (enemyY == playerY)
                enemyY = playerY;
            else
                enemyY = enemyY - enemyspeed;

            Canvas.SetLeft(enemy_img, enemyX);
            Canvas.SetTop(enemy_img, enemyY);

        }
        public bool IsEnemyOnCanvas(Canvas canvas)
        {
            if (canvas.Children.Contains(enemy_img))
                return true;
            else
                return false;
        }
    }
}
