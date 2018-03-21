using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using YatSprint.Managers;
using YatSprint.Objects;

namespace YatSprint.Factories
{
    public class BirdFactory : IFactory<IEntity>
    {
        private Level level;
        private Yat yat;
        private Texture2D spriteSheet;
        private SoundEffect hawk;
        private Rectangle window;
        private XPManager xpgen;

        public IEntity[] Generate()
        {
            int amount = Rand.Next(1, 4);
            List<Bird> birds = new List<Bird>();
            for (int i = 0; i < amount; i++)
            {
                int height = Rand.Next((int)Bird.Size.Y, window.Height / 3);
                int x = (int)-Bird.Size.X;
                float angle = MathHelper.ToRadians(0);
                if (Rand.Next(2) == 0)//Right side
                {
                    x = window.Width + (int)Bird.Size.X;
                    angle = MathHelper.ToRadians(180);
                }
                Bird b = new Bird(level, 
                    yat, 
                    spriteSheet, 
                    null,//hawk.CreateInstance(), 
                    Bird.Size, 
                    new Vector2(x, height), 
                    angle, 
                    xpgen.New(), 
                    window);
                birds.Add(b);        
            }
            return birds.ToArray();
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            window = newWindow;
        }

        public BirdFactory(Level level, Yat yat, Texture2D spriteSheet, SoundEffect hawk, XPManager xpgen, Rectangle window)
        {
            this.level = level;
            this.yat = yat;
            this.spriteSheet = spriteSheet;
            this.hawk = hawk;
            this.xpgen = xpgen;
            this.window = window;
        }
    }
}
