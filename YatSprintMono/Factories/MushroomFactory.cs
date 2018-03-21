using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatSprint.Managers;
using YatSprint.Objects;
using YatSprint.Objects.Blocks;
using YatSprint.UI;

namespace YatSprint.Factories
{
    public class MushroomFactory : IFactory<IEntity>
    {
        private Texture2D spriteSheet, fireballSheet;
        private Vector2 frameSize;
        private Level level;
        private Yat yat;
        private SoundEffect sound, fire;
        private XPManager xpgen;
        private Rectangle window;

        public IEntity[] Generate()
        {
            List<Mushroom> mushrooms = new List<Mushroom>();
            Block[] collisions = level.Manager.GetAllBlocks(BlockFilter.Top, false);
            int maximum = Rand.Next(2, 5);
            for (int i = 0; i < collisions.Length; i++)
            {
                if (Rand.Next(2) == 0)
                {
                    Vector2 spawn = new Vector2(collisions[i].X, collisions[i].Y - Mushroom.Size.Y);
                    Fireball fb = new Fireball(fireballSheet, null, Fireball.Size, new Vector2(0), Rand.Next(1, 6));
                    Mushroom current = new Mushroom(spriteSheet,
                        Mushroom.Size,
                        spawn,
                        level,
                        yat,
                        fb,
                        null,//sound.CreateInstance(),
                        xpgen.New(),
                        window);
                    mushrooms.Add(current);
                    if (mushrooms.Count >= maximum)
                        break;
                }
            }
            return mushrooms.ToArray();
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            window = newWindow;
        }

        public MushroomFactory(Texture2D spriteSheet, 
            Texture2D fireSheet,
            Vector2 frameSize,
            Level level,
            Yat yat, 
            SoundEffect fire, 
            SoundEffect sound, 
            XPManager xpgen,
            Rectangle window)
	    {
            this.spriteSheet = spriteSheet;
            this.fireballSheet = fireSheet;
            this.frameSize = frameSize;
            this.level = level;
            this.yat = yat;
            this.fire = fire;
            this.sound = sound;
            this.xpgen = xpgen;
            this.window = window;
	    }
    }
}
