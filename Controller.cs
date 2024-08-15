using SFML.Graphics;
using SFML.Window;

namespace To_The_Stars_hashtag_version
{
    internal class Controller
    {
        public Animator Animator { get; private set; }
        public Server? Server;
        public Client? Client;
        public Controller()
        {
            Animator = new();
        }
        static private void DeleteBullets(List<Bullet> bullets)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].State == "destroyed")
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }
        static private void DeleteBoosters(List<Booster> boosters)
        {
            for (int i = 0; i < boosters.Count; i++)
            {
                if (boosters[i].State == "destroyed")
                {
                    boosters.RemoveAt(i);
                    i--;
                }
            }
        }
        static private void SpawnBooster(List<Booster> boosters, Player player, Enemy enemy)//this method creates new player booster
        {
            double x = enemy.EntityRect.Left + enemy.EntityRect.Width / 2 - 10;//start x coord
            double y = enemy.EntityRect.Top + enemy.EntityRect.Height / 2 - 10;//start y coord
            Rectangle borders = new(x - 1, y, 21, 1200);//create booster movement borders
            int num = Entity.GetRandNum(0, 6);//generate random num (0 to execute booster creation code (~15%?))
            if (num == 0)
            {
                num = Entity.GetRandNum(0, 1);//generate random num (booster type)
                if (num == 0)//if num is 0 then create player upgrade booster
                {
                    if (player.UpgradeLvl < 2)//if upgrade level is below 2
                        boosters.Add(new(new(x, y, 20, 20), borders, "down", 1.5, "upgradeBooster"));//spawn booster
                    else
                        boosters.Add(new(new(x, y, 20, 20), borders, "down", 1.5, "hpBooster"));//otherwise spawn hp booster
                }
                else
                    boosters.Add(new(new(x, y, 20, 20), borders, "down", 1.5, "hpBooster"));//spawn hp booster
            }
        }
        static private void DeleteEnemies(List<Enemy> enemies, List<Booster> boosters, Player player, int enemyNum)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].CurrAnimation.Name == "destroyed" && enemies[i].AnimHasEnded)
                {
                    SpawnBooster(boosters, player, enemies[i]);
                    enemies.RemoveAt(i);
                    i--;
                    player.Score += 100 * enemyNum;
                }
            }
        }
        private void UpdateBullets(List<Bullet> bullets, List<Enemy> enemies, Player player, Player comrade, double elapsedTime)
        {
            foreach (var bullet in bullets)//execute the loop
            {
                bullet.UpdateBehaviour(elapsedTime);//update bullet state
                if (bullet.CheckCollision(player.EntityRect) && player.State != "destroyed" && player.State != "evaded")//check on collision
                {
                    if (Entity.GetRandNum(0, player.Hp) == 0)//get random number (~15% chance at the beginning)
                        player.SetEntityState("evaded", true);//player has evaded the bullet
                    else
                    {
                        player.Hp -= 2;//otherwise decrease player hp
                        Storage.PlayHit();//play hit sound
                        if (player.State != "damaged")
                            player.SetEntityState("damaged", true);//player has taken damage
                    }
                }
                if (comrade != null && (bullet.CheckCollision(comrade.EntityRect) && comrade.State != "destroyed" && comrade.State != "evaded"))//check on collision
                {
                    if (Entity.GetRandNum(0, comrade.Hp) == 0)//get random number (~15% chance at the beginning)
                        comrade.SetEntityState("evaded", true);//player has evaded the bullet
                    else
                    {
                        comrade.Hp -= 2;//otherwise decrease player hp
                        Storage.PlayHit();//play hit sound
                        if (comrade.State != "damaged")
                            comrade.SetEntityState("damaged", true);//player has taken damage
                    }
                }

                foreach (var e in enemies)//execute the loop
                    if (bullet.CheckCollision(e.EntityRect) && e.State != "destroyed" && (e.State != "evaded"))//check on collision
                    {
                        if (Entity.GetRandNum(0, e.Hp) == 0)//get random number (~15% chance at the beginning)
                            e.SetEntityState("evaded", true);//enemy has evaded the bullet
                        else
                        {
                            e.Hp -= 2; ;//otherwise decrease enemy hp
                            Storage.PlayHit();//play hit sound
                            if (bullet.Creator == "player")
                                player.Score += 50;//increase player score
                            else if (bullet.Creator == "comrade")
                                comrade.Score += 50;
                            if (e.State != "damaged")
                                e.SetEntityState("damaged", true);//enemy has taken damage
                        }
                    }
            }
            DeleteBullets(bullets);//remove all destroyed bullets from the list
        }
        private void UpdateBoosters(List<Booster> boosters, Player player, Player comrade, double elapsedTime)
        {
            foreach (var booster in boosters)//execute the loop
            {
                booster.UpdateBehaviour(elapsedTime);//update booster state
                if (booster.CheckCollision(player.EntityRect) && player.State != "destroyed" && player.State != "evaded")//check on collision
                {
                    if (booster.Tag == "upgradeBooster" && player.UpgradeLvl < 2)
                        player.UpgradeLvl += 1;//player ship has been upgraded
                    else
                        if (booster.Tag == "hpBooster")
                        player.Hp += 2;
                }
                if (comrade != null && booster.CheckCollision(comrade.EntityRect) && comrade.State != "destroyed" && comrade.State != "evaded")//check on collision
                {
                    if (booster.Tag == "upgradeBooster" && comrade.UpgradeLvl < 2)
                        comrade.UpgradeLvl += 1;//player ship has been upgraded
                    else
                        if (booster.Tag == "hpBooster")
                        comrade.Hp += 2;
                }
            }
            DeleteBoosters(boosters);//remove all collected boosters from the list
        }
        static private void UpdateEnemies(List<Enemy> enemies, List<Bullet> bullets, List<Booster> boosters, Player player, int enemyNum, double elapsedTime)
        {
            foreach (var enemy in enemies)//execute the loop
            {
                enemy.UpdateBehaviour(elapsedTime);//update enemy's state
                if (enemy.ShootTimer >= 1500 && enemy.State != "destroyed")//if reload has finished
                {
                    enemy.Shoot(bullets, "enemy");//shoot a bullet
                    enemy.ShootTimer = 0;//reset timer
                }
            }
            DeleteEnemies(enemies, boosters, player, enemyNum);//remove all destroyed enemies from the list
        }
        static private void CreateEnemy(Model model, uint xSize)
        {
            double rectWidth = (xSize / model.EnemyNum);//set new enemy border width
            double offset = rectWidth * model.CurrEnemyNum;//set offset from window (0,0) - top left corner
            double startPosX = offset + ((rectWidth / 2) - 32);//set enemy start posx
            double startPosY = 81;//set enemy start posy
            Rectangle startBorders = new(0 + offset, 80, rectWidth, 500);//create enemy movement borders based on given values
            model.Enemies.Add(new(new(startPosX, startPosY, 64, 92), startBorders, "idle", 1.4 + 0.5 * model.EnemyUpgradeLvl, "enemy", 10, 2, model.EnemyUpgradeLvl));//create an object in enemy list with given values
            model.CurrEnemyNum++;//increase current enemy number
        }
        public static void RestoreEnemies(Model model, uint xSize)
        {
            if (model.Enemies.Count == 0)//if there are no enemies left in the list
            {
                model.EnemyNum = Entity.GetRandNum(1, 3);//get a randon value between 1 and 3 (enemies in the single wave number)
                model.CurrEnemyNum = 0;
                for (int i = 0; i < model.EnemyNum; i++)
                    CreateEnemy(model, xSize);//create new object in enemy list
            }
        }
        public static void RestartGame(Model model)
        {
            model.Bullets.Clear();
            model.Enemies.Clear();
            model.Boosters.Clear();
            model.Comrade = null;
        }
        public void EndTransmission()
        {
            Client?.Kill();
            Server?.Kill();
            Client = null;
            Server = null;
        }
        public void ChooseGameMode(Model model, Mutex mut, uint xSize, uint ySize)
        {
            if (Program.IsServer)
            {
                if (Program.CheckInternetConnection())
                {
                    model.Comrade = new Player(new(xSize / 2 + 118, ySize - 95, 64, 92), new(300, 600, 300, 600), "idle", 1.5, "player", 10, 2);
                    Server = new(model, mut);
                    if(!Server.AwaitConnection())
                    {
                        Model.Handler.SetMenuActive("cerror");
                        return;
                    }
                    Server.StartRecSendRoutine();
                    model.Player = new Player(new(xSize / 2 - 118, ySize - 95, 64, 92), new(0, 600, 300, 600), "idle", 1.5, "player", 10, 2);
                }
                else
                {
                    Model.Handler.SetMenuActive("cerror");
                }
            }
            else if (Program.IsClient)
            {
                if (Program.CheckInternetConnection())
                {
                    Console.WriteLine("Enter the server ip: ");
                    string serverIp = Console.ReadLine();
                    model.Comrade = new Player(new(xSize / 2 - 118, ySize - 95, 64, 92), new(0, 600, 300, 600), "idle", 1.5, "player", 10, 2);
                    Client = new(model, mut, serverIp);
                    Client.StartRecSendRoutine();
                    model.Player = new Player(new(xSize / 2 + 118, ySize - 95, 64, 92), new(300, 600, 300, 600), "idle", 1.5, "player", 10, 2);
                }
                else
                {
                    Model.Handler.SetMenuActive("cerror");
                }
            }
            else
            {
                model.Player = new Player(new(xSize / 2 - 32, ySize - 95, 64, 92), new(0, 600, 600, 600), "idle", 1.5, "player", 10, 2);
            }

        }
        public void Update(Model model, double elapsedTime)
        {
            Model.Background.UpdatePosition(elapsedTime);
            if (Client == null)
            {
                UpdateBullets(model.Bullets, model.Enemies, model.Player, model.Comrade, elapsedTime);
                UpdateBoosters(model.Boosters, model.Player, model.Comrade, elapsedTime);
                UpdateEnemies(model.Enemies, model.Bullets, model.Boosters, model.Player, model.EnemyNum, elapsedTime);
                model.Player.UpdateBehaviour(elapsedTime);
                model.Comrade?.UpdateBehaviour(elapsedTime);
                if (model.Player.IsShooting)
                    model.Player.Shoot(model.Bullets, "player");
                if (model.Comrade != null)
                    if (model.Comrade.IsShooting)
                        model.Comrade.Shoot(model.Bullets, "comrade");
            }
        }
    }
}
