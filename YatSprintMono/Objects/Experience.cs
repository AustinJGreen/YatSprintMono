using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YatSprint.UI;

namespace YatSprint.Objects
{
    public class Experience : IObject
    {
        private int moveSpeed = 6;
        private float soundDimmer = 0.0F;

        private Texture2D orb;
        private SoundEffectInstance twinkle;
        private Progressbar experienceBar;
        private Vector2 Position;
        private Yat yat;
        private int strength;
        private double Angle;
        public bool Visible { get; set; }
        public bool Done { get { return !Visible; } }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, orb.Width, orb.Height);
        }

        public void Update(GameTime time)
        {
            if (Visible)
            {
                Vector2 pos = experienceBar.Position;
                Vector2 center = new Vector2(pos.X + (experienceBar.cage.Width / 2), pos.Y + (experienceBar.cage.Height / 2));
                Angle = Math.Atan2(center.Y - Position.Y, center.X - Position.X);

                Position.X += (moveSpeed * Settings.gameSpeed) * (float)Math.Cos(Angle);
                Position.Y += (moveSpeed * Settings.gameSpeed) * (float)Math.Sin(Angle);

                Rectangle cage = experienceBar.GetBoundingBox();
                Rectangle orbRect = GetBoundingBox();
                if (cage.Intersects(orbRect))
                {
#if DEBUG
                    Debugger.Log(1, "Main", string.Format("XPBar gained {0} experience points.\n", strength));
#endif
                    Visible = false;
                    Play();
                    yat.ExperiencePoints += strength;
                }
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            if (Visible)
            {
                batch.Draw(orb, Position, Color.White);
            }
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //twinkle.Volume = Settings.Volume - soundDimmer;
                //twinkle.Play();
            }
        }

        public void Reset()
        {
            
        }

        public Experience(Yat yat, Progressbar experienceBar, Texture2D orb, SoundEffect twinkleEffect, Vector2 position, int strength, double startAngle)
        {
            this.yat = yat;
            this.experienceBar = experienceBar;
            this.orb = orb;
            this.twinkle = null;// twinkleEffect.CreateInstance();
            this.Position = position;
            this.strength = strength;
            this.Visible = true;
            this.Angle = startAngle;
        }
    }
}
