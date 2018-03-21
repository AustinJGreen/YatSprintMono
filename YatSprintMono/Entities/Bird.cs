using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using YatSprint.Global;
using YatSprint.Managers;

namespace YatSprint.Objects
{
    public class Bird : SpriteSheet, IEntity
    {
        private const float moveSpeed = 4;

        private const float yatHoldDistance = 6;

        private const float soundDimmer = 0.8F;

        private const int attackChance = 10005;

        private float attackAngle = 0;

        private SoundEffectInstance hawk;

        private Rectangle window;

        private Yat yat;

        private Level level;

        private XPManager xpgen;

        public XPManager Manager { get { return xpgen; } }

        private bool gotYat = false;

        public bool Attacking { get; set; }

        public bool InBounds()
        {
            return !(Position.Y < 0 || Position.X < 0 || Position.X > window.Width || Position.Y > window.Height);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Bird.Size.X, (int)Bird.Size.Y);
        }

        public void ShiftX(float amount)
        {
        }

        public void Update(GameTime time)
        {
            UpdateFrame(time);
            if (!InBounds() && gotYat && !Actions.Gameover)
            {
                Actions.Gameover = true;
            }
            float aX = moveSpeed * (float)Math.Cos(attackAngle);
            float aY = moveSpeed * (float)Math.Sin(attackAngle);
            Position.X += aX * Settings.gameSpeed;
            Position.Y += aY * Settings.gameSpeed;
            if (gotYat)
            {
                yat.BirdAngle = attackAngle;
                float cX = yatHoldDistance * (float)Math.Cos(attackAngle);
                float cY = yatHoldDistance * (float)Math.Sin(attackAngle);
                yat.Vector = new Vector2(Position.X + cX, Position.Y + cY);
                float deg = MathHelper.ToDegrees(attackAngle);
                if (deg > 90 && deg < 270)
                    attackAngle = MathHelper.ToRadians(deg + (1 * Settings.gameSpeed));
                else if (deg < 90 && deg > -90)
                    attackAngle = MathHelper.ToRadians(deg - (1 * Settings.gameSpeed));
            }
            if (Attacking && !gotYat)
            {
                Rectangle birdy = GetBoundingBox();
                if (yat != null && !gotYat && !yat.Invulnerable)
                {
                    Rectangle yatBox = yat.GetBoundingBox();
                    if (birdy.Intersects(yatBox))
                    {
                        //TODO
                        gotYat = true;
                        yat.BeingCarriedByBird = true;
                        yat.BirdAngle = attackAngle;
                        
#if DEBUG
                        Debugger.Log(1, "Main", string.Format("Bird has grabbed yat.\n",  GetHashCode()));
#endif
                    }
                }
            }
            else if (!Attacking && !gotYat)
            {
                if (Rand.Next(attackChance) == 0 && InBounds() && !yat.Invulnerable && yat.HitGround) //Attack if by chance and In bounds, and the yat isn't invulnerable
                {
                    Attacking = true;
                    Vector2 yatPos = yat.Vector;
                    attackAngle = (float)Math.Atan2(yatPos.Y - Position.Y, yatPos.X - Position.X);
                    Play();
#if DEBUG
                    Debugger.Log(1, "Main", string.Format("Bird has started to attack at a {0}° angle.\n", MathHelper.ToDegrees(attackAngle)));
#endif
                }
            }
            xpgen.Update(time);
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            xpgen.UpdateWindow(newWindow);
            window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            float deg = MathHelper.ToDegrees(attackAngle);
            float drawAngle = attackAngle;
            bool flip = false;
            if (deg <= 180 && deg > 90)
            {
                drawAngle = MathHelper.ToRadians(deg - 180);
                flip = true;
            }

            int x = (int)Math.Floor(index) % framesX;
            int y = (int)Math.Floor(index) / framesX;
            Rectangle dest = new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
            Rectangle source = new Rectangle((int)(x * frameSize.X), (int)(y * frameSize.Y), (int)frameSize.X, (int)frameSize.Y);
            batch.Draw(spriteSheet,
                dest,
                source,
                Color.White,
                drawAngle,
                new Vector2(frameSize.X / 2, frameSize.Y / 2),
                (flip) ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0);
            xpgen.Draw(batch);
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //hawk.Volume = Settings.Volume - soundDimmer;
                //hawk.Play();               
            }
        }

        public void Reset()
        {
            gotYat = false;
            xpgen.Reset();
        }

        public bool Dead()
        {
            return !window.Contains(GetBoundingBox()) && xpgen.Done;
        }

        public Bird(Level level, 
            Yat yat, 
            Texture2D spriteSheet, 
            SoundEffectInstance hawk, 
            Vector2 frameSize, 
            Vector2 position,
            float angle,
            XPManager xpgen,
            Rectangle window)
        {
            this.animationSpeed = 50;
            this.level = level;
            this.yat = yat;
            this.spriteSheet = spriteSheet;
            this.hawk = hawk;
            this.frameSize = frameSize;
            this.framesX = (int)(spriteSheet.Width / frameSize.X);
            this.framesY = (int)(spriteSheet.Height / frameSize.Y);
            this.Position = position;
            this.attackAngle = angle;
            this.xpgen = xpgen;
            this.window = window;
            this.Attacking = false;
            this.gotYat = false;
        }

        public static readonly Vector2 Size = new Vector2(76, 85);
    }
}
