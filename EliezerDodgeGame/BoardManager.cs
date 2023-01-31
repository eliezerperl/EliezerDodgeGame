using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace EliezerDodgeGame
{
    internal class BoardManager
    {
        Canvas canvas;
        Player player;
        Enemy[] enemies;
        GameManager game;
        DispatcherTimer timer;
        int enemyCount;

        public DispatcherTimer Timer { get { return timer; } }
        public BoardManager(Canvas canvas)
        {
            this.canvas = canvas;
            enemyCount = 10;
            enemies = new Enemy[enemyCount];
            player = new Player();
            game = new GameManager(canvas);


        }
        public void PlacePlayer()
        {
            Image p = player.AddPlayer();
            canvas.Children.Add(p);
        }
        public bool PlayerWon()
        {
            if (game.OneEnemyLeft(enemies))
                return true;
            return false;
        }
        public bool PlayerLost()
        {
            if (player.IsPlayerOnCanvas(canvas))
                return false;
            return true;
        }

        public void Pause() //Pause Button
        {
            timer.Stop();
            player.Timer.Stop();
        }
        public void Play() //Pause Button (play function)
        {
            timer.Start();
            player.Timer.Start();
        }


        public void PlaceEnemies(Enemy[] enemyArr)
        {
            for (int i = 0; i < enemyArr.Length; i++)
            {
                enemyArr[i] = new Enemy();

                while (game.Collision(enemyArr[i].Enemy_Img, player.Player_img, 200))
                    enemyArr[i].PlaceEnemy();
                for (int j = 0; j < i; j++)
                {
                    //Replacing an enemy if he is placed 100px of another enemy && Replacing an enemy if he is placed within 200px of the player
                    if (game.Collision(enemyArr[i].Enemy_Img, enemyArr[j].Enemy_Img, 100) || game.Collision(enemyArr[i].Enemy_Img, player.Player_img, 200))
                    {
                        enemyArr[i].PlaceEnemy();
                        j = -1;
                    }
                }

                //Adding the current itirations' enemy onto the board
                canvas.Children.Add(enemyArr[i].Enemy_Img);
            }
        }
        public void RemoveEnemies()
        {
            foreach (Enemy item in enemies)
            {
                canvas.Children.Remove(item.Enemy_Img);
            }
        }


        public void StartNewGame() //For New Game Button
        {
            player.ResetPlayerMovement();
            RemoveEnemies();
            StartupGame();
            if (!player.Timer.IsEnabled)
                player.Timer.Start();
        }
        public void StartupGame() // For Start button (initializes game then disapears and is replaced by New Game button or if loaded game is pressed the same result happens)
        {
            PlaceEnemies(enemies);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            timer.Tick += EnemiesPlaced_Timer;
            timer.Start();
        }
        public void EnemiesMovements()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].EnemyMovement(player);
            }
        }
        private void EnemiesPlaced_Timer(object sender, object e)//GAME TIMER that checks collisions and game won or lost
        {
            EnemiesMovements();
            game.EnemyColide(enemies);
            game.PlayerCollision(enemies, player);
            if (PlayerLost())
            {
                timer.Stop();
                LoseMessage();
            }
            if (PlayerWon())
            {
                timer.Stop();
                WonMessage();
            }
        }


        // Game ended Message Dialog scenarios
        public async void WonMessage()
        {
            player.Timer.Stop();
            MessageDialog winMessage = new MessageDialog("Congratulations, you won my first game!", "You won!!");

            winMessage.Commands.Add(new UICommand("Start New Game", StartNewGameFromWin));
            winMessage.Commands.Add(new UICommand("Exit", Close));

            await winMessage.ShowAsync();
        }
        void StartNewGameFromWin(IUICommand command)
        {
            player.ResetPlayerMovement();
            RemoveEnemies();
            StartupGame();
            if (!player.Timer.IsEnabled)
                player.Timer.Start();
        }
        public async void LoseMessage()
        {
            MessageDialog LostMassage = new MessageDialog("Loser! You should go home crying to your mommy.", "YOU LOST!");

            LostMassage.Commands.Add(new UICommand("Start New Game", StartNewGameFromLoss));
            LostMassage.Commands.Add(new UICommand("Close", Close));

            await LostMassage.ShowAsync();

        }
        void StartNewGameFromLoss(IUICommand command)
        {
            player.ResetPlayerMovement();
            RemoveEnemies();
            PlacePlayer();
            StartupGame();
        }
        void Close(IUICommand command)
        {
            Application.Current.Exit();
        }

        // Save / Load
        public async void SaveGame()
        {
            player.Timer.Stop();
            timer.Stop();

            //Saving Players width, height aand speed to a file
            string player_w_h_speed = "";
            player_w_h_speed += player.PlayerW;
            player_w_h_speed += ',';
            player_w_h_speed += player.PlayerH;
            player_w_h_speed += ',';
            player_w_h_speed += player.PlayerSpeed;
            File.WriteAllText("playerwhspeed.txt", player_w_h_speed);

            //Saving Enemys width, height aand speed to a file
            string enemy_w_h_speed = "";
            enemy_w_h_speed += enemies[0].EnemyW;
            enemy_w_h_speed += ',';
            enemy_w_h_speed += enemies[0].EnemyH;
            enemy_w_h_speed += ',';
            enemy_w_h_speed += enemies[0].EnemySpeed;
            File.WriteAllText("enemywhspeed.txt", enemy_w_h_speed);

            //Saving Players x and y to a file
            string player_x_y = "";
            player_x_y += player.PlayerX;
            player_x_y += ",";
            player_x_y += player.PlayerY;
            File.WriteAllText("playerlocation.txt", player_x_y);

            //Saving Enemys x and y to a file
            string enemys_x_y = "";

            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i].IsEnemyOnCanvas(canvas))
                {
                    if (i != 0)
                        enemys_x_y += ",";
                    enemys_x_y += enemies[i].EnemyX;
                    enemys_x_y += ",";
                    enemys_x_y += enemies[i].EnemyY;
                }

            }
            File.WriteAllText("enemylocations.txt", enemys_x_y);

            // Showing message and exiting after save
            MessageDialog messageSave = new MessageDialog("Success!", "Game Saved succesfully!");

            messageSave.Commands.Add(new UICommand("Exit Game", Close));

            await messageSave.ShowAsync();

        }
        public void LoadGame()
        {
            // Extracting players' x and y and setting them
            if (File.Exists("playerlocation.txt"))
            {
                string player_x_y = File.ReadAllText("playerlocation.txt");
                string[] playerArr_x_y = player_x_y.Split(',');
                player.PlayerX = double.Parse(playerArr_x_y[0]);
                player.PlayerY = double.Parse(playerArr_x_y[1]);
                Canvas.SetLeft(player.Player_img, player.PlayerX);
                Canvas.SetTop(player.Player_img, player.PlayerY);
            }

            // Removing enemies if there are any on the board
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                {
                    RemoveEnemies();
                    break;
                }
            }

            // Extracting enemys' x and y and setting them
            if (File.Exists("enemylocations.txt"))
            {
                string enemy_x_y = File.ReadAllText("enemylocations.txt");
                string[] enemies_x_y = enemy_x_y.Split(',');
                enemies = new Enemy[enemyCount];

                for (int i = 0; i < enemies_x_y.Length / 2; i++)
                {
                    enemies[i] = new Enemy();
                    enemies[i].EnemyX = double.Parse(enemies_x_y[i * 2]);
                    enemies[i].EnemyY = double.Parse(enemies_x_y[i * 2 + 1]);
                    Canvas.SetLeft(enemies[i].Enemy_Img, enemies[i].EnemyX);
                    Canvas.SetTop(enemies[i].Enemy_Img, enemies[i].EnemyY);
                    canvas.Children.Add(enemies[i].Enemy_Img);
                }

                // This is to ensure that if the game is reloaded (New Game) after loading a game with x amount of enemys, that the new game will go back to ten enemys
                for (int i = enemies_x_y.Length / 2; i < enemyCount; i++)
                    enemies[i] = new Enemy();

                // If user Loads a game from the begining and the timer is null (making the timer)
                if (timer == null)
                {
                    timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                    timer.Tick += EnemiesPlaced_Timer;
                }
                timer.Start();


                // Extracting players width, height and speed and setting them
                if (File.Exists("playerwhspeed.txt"))
                {
                    string player_w_h_speed = File.ReadAllText("playerwhspeed.txt");
                    string[] playerwhspeed = player_w_h_speed.Split(',');
                    player.PlayerW = double.Parse(playerwhspeed[0]);
                    player.PlayerH = double.Parse(playerwhspeed[1]);
                    player.PlayerSpeed = double.Parse(playerwhspeed[2]);
                }

                // Extracting enemys width, height and speed and setting them
                if (File.Exists("enemywhspeed.txt"))
                {
                    string enemy_w_h_speed = File.ReadAllText("enemywhspeed.txt");
                    string[] enemywhspeed = enemy_w_h_speed.Split(',');
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i].EnemyW = double.Parse(enemywhspeed[0]);
                        enemies[i].EnemyH = double.Parse(enemywhspeed[1]);
                        enemies[i].EnemySpeed = double.Parse(enemywhspeed[2]);
                    }
                }
            }
        }
        public void SetSpeedSliderValues(Slider enemyspeed, Slider playerspeed, Slider enemysize, Slider playersize) //Function to set slider values to what the user saved them at
        {
            enemyspeed.Value = enemies[0].EnemySpeed;
            playerspeed.Value = player.PlayerSpeed;
            enemysize.Value = (enemies[0].EnemyH - 80) / 20;
            playersize.Value = (player.PlayerH - 80) / 20;

        }

        // Player/Enemy Sliders
        public void ChangeEnemySize(double val)
        {
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].EnemyH = val * 20 + 80;
                    enemies[i].EnemyW = val * 16 + 67;
                }
            }
        }
        public void ChangeEnemySpeed(double val)
        {
            if (enemies != null)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].EnemySpeed = val;
                }
            }
        }
        public void ChangePlayerSize(double val)
        {
            player.PlayerH = val * 20 + 80;
            player.PlayerW = val * 16 + 67;
        }
        public void ChangePlayerSpeed(double val)
        {
            if (player != null)
            {
                player.PlayerSpeed = val;
            }
        }
    }
}
