using SFML.Audio;
using SFML.Graphics;

namespace To_The_Stars_hashtag_version
{
    internal static class Storage
    {
        static Dictionary<string, int> TextureManager = new()
        {
            {"idle",0},
            {"left",1},
            {"right",2},
            {"up",3},
            {"down",4},
            {"destroyed",5},
            {"damaged",6},
            {"evaded",7}
        };
        static Dictionary<string, int> TagManager = new()
        {
            {"player",1},
            {"enemy",-1},
            {"bullet",0},
            {"upgradeBooster",2},
            {"hpBooster",3}
        };
        static Texture[]
            PlayerLeftTexture = new Texture[3],
            PlayerRightTexture = new Texture[3],
            PlayerUpDownTexture = new Texture[6],
            PlayerDestroyedTexture = new Texture[15],
            PlayerEvasionTexture = new Texture[8],
            PlayerDamagedTexture = new Texture[9],
            EnemyBulletTexture = new Texture[4],
            EnemyLeftTexture = new Texture[2],
            EnemyRightTexture = new Texture[2],
            EnemyUpDownTexture = new Texture[6],
            EnemyDestroyedTexture = new Texture[10],
            EnemyDamagedTexture = new Texture[10],
            EnemyEvasionTexture = new Texture[9],
            BackgroundPictures = new Texture[18];
        static Texture PlayerBullet, PlayerIdleTexture, HpIco, UpgradeBooster, HpBooster, EnemyIdleTexture;
        static SoundBuffer ExploBuff, HitBuff, ShotBuff;
        static Sound ExplosionSound, HitSound, ShotSound;
        static string[] Songs = new string[11];
        static Music CurrSong;
        static bool OnMusic, OnSound;
        public const int
         PLAYER = 1,
         ENEMY = -1,
         BULLET = 0,
         UPGRADEBOOSTER = 2,
         HPBOOSTER = 3,
         IDLE = 0,
         LEFT = 1,
         RIGHT = 2,
         UP = 3,
         DOWN = 4,
         DESTROYED = 5,
         DAMAGED = 6,
         EVADED = 7;
        static Storage()
        {
            for (int i = 0; i < 18; i++)
                BackgroundPictures[i] = new("RSS/Models/Background/" + i.ToString() + ".png");
            PlayerIdleTexture = new("RSS/Models/Player/Idle/Idle.png");
            EnemyIdleTexture = new("RSS/Models/Enemy/Idle/Idle.png");
            PlayerBullet = new("RSS/Models/Charges/Player.png");
            UpgradeBooster = new("RSS/Models/Boosters/upgrade.png");
            HpBooster = new("RSS/Models/Boosters/hp.png");
            ExploBuff = new("RSS/Sounds/explosion.wav");
            ExplosionSound = new(ExploBuff);
            ShotBuff = new("RSS/Sounds/shot.wav");
            ShotSound = new(ShotBuff);
            HitBuff = new("RSS/Sounds/hit.wav");
            HitSound = new(HitBuff);
            HpIco = new("RSS/Ico/ico.png");
            for (int i = 0; i < 3; i++)
            {
                PlayerLeftTexture[i] = new("RSS/Models/Player/Turn_1/" + i.ToString() + ".png");
                PlayerRightTexture[i] = new("RSS/Models/Player/Turn_2/" + i.ToString() + ".png");
            }
            for (int i = 0; i < 2; i++)
            {
                EnemyLeftTexture[i] = new("RSS/Models/Enemy/Turn_1/" + i.ToString() + ".png");
                EnemyRightTexture[i] = new("RSS/Models/Enemy/Turn_2/" + i.ToString() + ".png");
            }
            for (int i = 0; i < 6; i++)
            {
                PlayerUpDownTexture[i] = new("RSS/Models/Player/Up_Down/" + i.ToString() + ".png");
                EnemyUpDownTexture[i] = new("RSS/Models/Enemy/Up_Down/" + i.ToString() + ".png");
            }
            for (int i = 0; i < 10; i++)
            {
                EnemyDestroyedTexture[i] = new("RSS/Models/Enemy/Destroy/" + i.ToString() + ".png");
                EnemyDamagedTexture[i] = new("RSS/Models/Enemy/Damage/" + i.ToString() + ".png");
            }
            for (int i = 0; i < 9; i++)
            {
                PlayerDamagedTexture[i] = new("RSS/Models/Player/Damage/" + i.ToString() + ".png");
                EnemyEvasionTexture[i] = new("RSS/Models/Enemy/Evasion/" + i.ToString() + ".png");
            }

            for (int i = 0; i < 8; i++)
                PlayerEvasionTexture[i] = new("RSS/Models/Player/Evasion/" + i.ToString() + ".png");
            for (int i = 0; i < 15; i++)
                PlayerDestroyedTexture[i] = new("RSS/Models/Player/Destroyed/" + i.ToString() + ".png");
            for (int i = 0; i < 4; i++)
                EnemyBulletTexture[i] = new("RSS/Models/Charges/Enemy/" + i.ToString() + ".png");
            for (int i = 0; i < 11; i++)
                Songs[i] = "RSS/Music/s" + i.ToString() + ".wav";
            CurrSong = new(Songs[Entity.GetRandNum(0, 11)]);
        }
        public static void ResetSong()
        {
            CurrSong.Stop();
        }
        public static void UpdateSong(int SongPos)
        {
            if (OnMusic)
                if (CurrSong.Status == SoundStatus.Stopped)
                {
                    CurrSong = new(Songs[SongPos]);
                    CurrSong.Play();
                }
        }
        public static void PlayExplosion()
        {
            if (OnSound)
                ExplosionSound.Play();
        }
        public static void PlayHit()
        {
            if (OnSound)
                HitSound.Play();
        }
        public static void PlayShot()
        {
            if (OnSound)
                ShotSound.Play();
        }
        public static Texture GetTexture(int tPos, string tag, string name)
        {
            switch (TagManager[tag])//check entity that requests texture
            {
                case PLAYER:
                    switch (TextureManager[name])//return texture based on the passed name
                    {
                        case IDLE:
                            return PlayerIdleTexture;
                        case LEFT:
                            return PlayerLeftTexture[tPos];
                        case RIGHT:
                            return PlayerRightTexture[tPos];
                        case UP:
                            return PlayerUpDownTexture[tPos];
                        case DOWN:
                            return PlayerUpDownTexture[tPos];
                        case DESTROYED:
                            return PlayerDestroyedTexture[tPos];
                        case DAMAGED:
                            return PlayerDamagedTexture[tPos];
                        case EVADED:
                            return PlayerEvasionTexture[tPos];
                    }
                    break;
                case ENEMY:
                    switch (TextureManager[name])//return texture based on the passed name
                    {
                        case IDLE:
                            return EnemyIdleTexture;
                        case LEFT:
                            return EnemyLeftTexture[tPos];
                        case RIGHT:
                            return EnemyRightTexture[tPos];
                        case UP:
                            return EnemyUpDownTexture[tPos];
                        case DOWN:
                            return EnemyUpDownTexture[tPos];
                        case DESTROYED:
                            return EnemyDestroyedTexture[tPos];
                        case DAMAGED:
                            return EnemyDamagedTexture[tPos];
                        case EVADED:
                            return EnemyEvasionTexture[tPos];
                    }
                    break;
                case BULLET:
                    switch (TextureManager[name])//return texture based on the passed name
                    {
                        case DOWN:
                            return EnemyBulletTexture[tPos];
                        case UP:
                            return PlayerBullet;
                    }
                    break;
                case UPGRADEBOOSTER:
                    return UpgradeBooster;
                case HPBOOSTER:
                    return HpBooster;
            }
            return null;
        }
        public static void SetSFXStatus(bool sound, bool music)
        {
            OnSound = sound;
            OnMusic = music;
        }
        public static Texture GetBackgroundImage(int tPos)
        {
            return BackgroundPictures[tPos];
        }
        public static Texture GetIco()
        {
            return HpIco;
        }
    }
}
