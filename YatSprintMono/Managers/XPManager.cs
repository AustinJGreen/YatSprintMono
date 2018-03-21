using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using YatSprint.Objects;
using YatSprint.UI;

namespace YatSprint.Managers
{
    public class XPManager : IObject
    {
        private Yat yat;
        private Texture2D orb;
        private SoundEffect twinkle;
        private Progressbar experienceBar;
        private Experience[] created;
        public bool Done = true;

        public void Generate(int amount, Vector2 Position)
        {
            Done = false;
            Experience[] orbs = new Experience[amount];
            for (int i = 0; i < orbs.Length; i++)
            {
                Vector2 origin = new Vector2(Position.X + Rand.Next(-10, 11), Position.Y + Rand.Next(-10, 11));
                Experience cur = new Experience(yat, experienceBar, orb, twinkle, origin, Rand.Next(1, 4), Rand.Next(0, 361));
                orbs[i] = cur;
            }
            this.created = orbs;
        }

        public void Update(GameTime time)
        {
            bool fin = true;
            if (created != null)
            {
                for (int i = 0; i < created.Length; i++)
                {
                    created[i].Update(time);
                    if (!created[i].Done)
                        fin = false;
                }
            }
            Done = fin;
        }

        public void UpdateWindow(Rectangle newWindow)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            if (created != null)
            {
                for (int i = 0; i < created.Length; i++)
                {
                    created[i].Draw(batch);
                }
            }
        }

        public void Reset()
        {
            this.created = null;
        }

        public XPManager New()
        {
            return this.MemberwiseClone() as XPManager;
        }

        public XPManager(Yat yat, Progressbar experienceBar, Texture2D orb, SoundEffect twinkleEffect)
        {
            this.yat = yat;
            this.experienceBar = experienceBar;
            this.orb = orb;
            this.twinkle = twinkleEffect;
        }

    }
}
