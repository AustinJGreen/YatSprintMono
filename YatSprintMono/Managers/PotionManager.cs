using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using YatSprint.Objects;
using YatSprint.Objects.Potions;
using YatSprint.UI;

namespace YatSprint.Managers
{
    public class PotionManager : IObject
    {
        private const Keys hotkey = Keys.V; //Vial :D

        private Yat yat;
        private Experiencebar xpbar;
        private Texture2D[] vials;
        private SoundEffect effect;
        private List<Potion> created;
        private int padding;
        private bool wasActivated = true;
        private bool readyToActivate = false;

        public void Generate()
        {
            int amount = Rand.NextExp(1, 4, 1);
            Potion[] potions = new Potion[amount];
            Vector2 pos = new Vector2(xpbar.Position.X + xpbar.cage.Width, xpbar.Position.Y);
            for (int i = 0; i < amount; i++)
            {
                pos.X += padding;
                int r = Rand.Next(4);
                Texture2D texture = vials[r];
                Potion p = null;
                switch (r)
                {
                    case 0:
                        p = new HealthPotion(yat, texture, null, pos, Rand.Next(2, 16), Rand.Next(10, 20), Rand.Next(1, 11));
                        break;
                    case 1:
                        p = new InvulnerabilityPotion(yat, texture, null, pos, Rand.Next(1, 11));
                        break;
                    case 2:
                        p = new JumpPotion(yat, texture, null, pos, Rand.Next(10, 20));
                        break;
                    case 3:
                        p = new SpeedPotion(yat, texture, null, pos, Rand.Next(10, 20), Rand.Next(2, 5));
                        break;
                }
                potions[i] = p;
                pos.X += texture.Width;
            }
#if DEBUG
            Debugger.Log(1, "Main", string.Format("Generated {0} potions...\n", amount)); ;
#endif
            this.created.AddRange(potions);
        }

        public void AlignPotions()
        {
            Vector2 pos = new Vector2(xpbar.Position.X + xpbar.cage.Width, xpbar.Position.Y);
            for (int i = 0; i < created.Count; i++)
            {
                pos.X++;
                created[i].Position = pos;
                pos.X += created[i].vial.Width;
            }
        }

        public void Update(GameTime time)
        {
            if (wasActivated && Keyboard.GetState().IsKeyUp(hotkey))
                readyToActivate = true;

            bool activateOne = false;

            if (readyToActivate && Keyboard.GetState().IsKeyDown(hotkey))
                activateOne = true;

            bool firstPotionDone = false;
            if (created != null)
            {
                for (int i = 0; i < created.Count; i++)
                {
                    if (i == 0)
                    {
                        if (!created[i].Activated && activateOne)
                            created[i].Activate();
                        if (created[i].Activated && !created[i].InEffect)
                            firstPotionDone = true;
                    }
                    created[i].Update(time);
                }
            }
            if (firstPotionDone)
            {
                created[0].End();
                created.RemoveAt(0);
                AlignPotions();
                wasActivated = true;
                readyToActivate = false;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            if (created != null)
            {
                for (int i = 0; i < created.Count; i++)
                {
                    created[i].Draw(batch);
                }
            }
        }

        public void Reset()
        {
            this.created.Clear();
        }

        public PotionManager New()
        {
            return this.MemberwiseClone() as PotionManager;
        }

        public PotionManager(Yat yat, Experiencebar xpbar, Texture2D[] vials, SoundEffect effect, int padding)
        {
            this.yat = yat;
            this.xpbar = xpbar;
            this.vials = vials;
            this.effect = effect;
            this.padding = padding;
            this.created = new List<Potion>();
        }
    }
}
