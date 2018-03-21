using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.Objects.Potions
{
    public abstract class Potion : IObject
    {
        protected TimeSpan started = TimeSpan.MinValue;
        protected SoundEffectInstance sound;
        protected Yat yat;
        protected Color poisonColor;
        public Vector2 Position;
        public Texture2D vial;
        public int strength;
        public int duration;
        public bool Activated { get; set; }
        public bool InEffect { get; set; }

        public virtual void Update(GameTime time)
        {
            if (started == TimeSpan.MinValue && Activated)
            {
                started = time.TotalGameTime;
                InEffect = true;
            }

            if (started != TimeSpan.MinValue)
            {
                if (time.TotalGameTime.Subtract(started).TotalSeconds > duration)
                {
                    InEffect = false;
                }
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(vial, Position, Color.White);
        }

        public void Reset()
        {
        }

        public void Activate()
        {
            Activated = true;
            InEffect = true;
            yat.poison = poisonColor;
            Play();
        }

        public virtual void End()
        {
            yat.poison = null;
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //sound.Volume = Settings.Volume;
                //sound.Play();
            }
        }
    }
}
