namespace To_The_Stars_hashtag_version
{
    internal class Animator
    {
        static Dictionary<string, Dictionary<string, Animation>> Animations;
        public Animator()
        {
            Animations = new();
            Animations.Add("player",
                new()
                {
                {"idle",new("idle", 1, false,false)},
                {"left", new ("left", 3, false,true)},
                {"right",new ("right", 3, false,true)},
                {"up" ,new("up", 6, true,false)},
                {"down" ,new("down", 6, true, false)},
                {"destroyed",new("destroyed", 15, false, false)},
                {"evaded" ,new ("evaded", 8, false, false)},
                {"damaged",new ("damaged", 9, false,false)},
                }
                );
            Animations.Add("enemy",
                new()
                {
                {"idle",new("idle", 1, false,false)},
                {"left", new ("left", 2, false,true)},
                {"right",new ("right", 2, false,true)},
                {"up" ,new("up", 6, true,false)},
                {"down" ,new("down", 6, true, false)},
                {"destroyed",new("destroyed", 10, false, false)},
                {"evaded" ,new ("evaded", 9, false, false)},
                {"damaged",new ("damaged", 10, false,false)},
                }
                );
            Animations.Add("bullet", new() { { "down", new("down", 4, true, false) }, { "up", new("up", 1, true, false) } });
            Animations.Add("hpBooster", new() { { "down", new("down", 1, true, false) } });
            Animations.Add("upgradeBooster", new() { { "down", new("down", 1, true, false) } });
        }
        public static void ResetCurrentAnimation(Entity entity)
        {
            if (entity.CurrAnimation.Loop)
            {
                entity.AnimHasEnded = true;
                return;
            }
            if (entity.CurrAnimation.IsReversible && !entity.AnimIsReversing)
            {
                entity.CurrFrame = entity.CurrAnimation.FrameQuantity;
                entity.AnimIsReversing = true;
            }
        }
        public static void SelectAnimation(Entity entity)
        {
            entity.AnimHasEnded = false;
            entity.AnimIsReversing = false;
            entity.CurrFrame = 0;
            if (Animations[entity.Tag].TryGetValue(entity.State, out Animation? value))
                entity.CurrAnimation = Animations[entity.Tag][entity.State];
        }
        public static void Update(Entity entity, double time)
        {
            if (entity.CurrAnimation.Name != "idle")
            {
                if (!entity.AnimHasEnded)
                {
                    entity.CurrFrame = (entity.AnimIsReversing) ? entity.CurrFrame - time / 1000 * 7 : entity.CurrFrame + time / 1000 * 7;
                    if (entity.CurrAnimation.Loop)
                    {
                        if (entity.CurrFrame >= entity.CurrAnimation.FrameQuantity - 1)
                            entity.CurrFrame = 0;
                    }
                    else
                    {
                        if (!entity.AnimIsReversing)
                        {
                            if (entity.CurrFrame >= entity.CurrAnimation.FrameQuantity)
                            {
                                entity.CurrFrame = entity.CurrAnimation.FrameQuantity - 1;
                                entity.AnimHasEnded = !entity.CurrAnimation.IsReversible;
                            }
                        }
                        else
                        {
                            if (entity.CurrFrame < 0)
                            {
                                entity.CurrFrame = 0;
                                entity.AnimIsReversing = false;
                                entity.AnimHasEnded = true;
                            }
                        }
                    }
                }
            }
            else
            {
                entity.AnimHasEnded = true;
            }
        }
    }
}
