using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace EliezerDodgeGame
{
    internal class GameManager
    {
        Canvas canvas;

        public GameManager(Canvas canvas)
        {
            this.canvas = canvas;
        }
        public void EnemyColide(Enemy[] enemyArr) // Indicates when two enemys collide and removes one
        {
            for (int i = 0; i < enemyArr.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (Collision(enemyArr[i].Enemy_Img, enemyArr[j].Enemy_Img, 0))
                    {
                        canvas.Children.Remove(enemyArr[i].Enemy_Img);
                    }
                }
            }
        }
        public bool Collision(Image image1, Image image2, int range) //Provides information if a collision happened between two Images (range is a sort of padding i added for the player/enemy placements in order for them not to be placed to close)
        {
            double xImg1 = Canvas.GetLeft(image1);
            double xImg2 = Canvas.GetLeft(image2);
            double yImg1 = Canvas.GetTop(image1);
            double yImg2 = Canvas.GetTop(image2);
            if (xImg1 <= xImg2 + image2.Width + range && xImg1 + image1.Width + range >= xImg2)
                if (yImg1 <= yImg2 + image2.Height + range && yImg1 + image1.Height + range >= yImg2)
                    return true;
            return false;
        }
        public void PlayerCollision(Enemy[] enemyArr, Player player) //Indicates whether player lost
        {
            for (int i = 0; i < enemyArr.Length; i++)
            {
                if (Collision(enemyArr[i].Enemy_Img, player.Player_img, 0) && enemyArr[i].IsEnemyOnCanvas(canvas))
                    canvas.Children.Remove(player.Player_img);
            }
        }
        public bool OneEnemyLeft(Enemy[] enemyArr) //Indicate whether player won or not
        {
            int counter = 0;
            for (int i = 0; i < enemyArr.Length; i++)
            {
                if (!enemyArr[i].IsEnemyOnCanvas(canvas))
                    counter++;
            }
            if (counter == enemyArr.Length - 1)
                return true;
            return false;
        }
    }
}
