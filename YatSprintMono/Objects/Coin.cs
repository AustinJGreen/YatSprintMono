using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using YatSprint.Objects.Blocks;

namespace YatSprint.Objects
{
    public class Coin : SpriteSheet, ILevelObject
    {
        private const float soundDimmer = 0.4F;

        private Yat yat;

        private Level level;

        private SoundEffectInstance coinSound;

        private Rectangle Window;

        public bool Visible { get; set; }

        public bool BlockBelow(out Rectangle block)
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, !Dead());
            for (int i = 0; i < blocks.Length; i++)
            {
                Rectangle b = blocks[i].GetBoundingBox();
                Rectangle thiscoin = GetBoundingBox();
                thiscoin.Y++;
                if (b.Y >= thiscoin.Y && thiscoin.Intersects(b))
                {
                    block = b;
                    return true;
                }
            }
            block = Rectangle.Empty;
            return false;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
        }

        public void ShiftX(float amount)
        {
            Position.X += amount;
        }

        public void Update(GameTime time)
        {
            UpdateFrame(time);
            Rectangle y = yat.GetBoundingBox();
            Rectangle c = GetBoundingBox();
            if (c.Intersects(y) && Visible)
            {
#if DEBUG
                Debugger.Log(1, "Main", "Yat got a coin.\n");
#endif
                Play();
                Visible = false;
                Score.Points += 1000;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            Rectangle rect = Rectangle.Empty;
            if (!BlockBelow(out rect))
            {
                int changeHeight = newWindow.Height - Window.Height;
                Position.Y += changeHeight;
            }
            Window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            if (Visible)
                DrawFrame(batch);
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //coinSound.Volume = Settings.Volume - soundDimmer;
                //coinSound.Play();
            }
        }

        public void Reset()
        {
        }

        public bool Dead()
        {
            Rectangle bb = GetBoundingBox();
            return bb.X + bb.Width < 0 || (!Visible);
        }

        public Coin(Yat yat, Level level, Texture2D spriteSheet, SoundEffectInstance coinSound, Vector2 frameSize, Vector2 position, Rectangle window)
        {
            this.animationSpeed = 50;
            this.yat = yat;
            this.level = level;
            this.spriteSheet = spriteSheet;
            this.coinSound = coinSound;
            this.frameSize = frameSize;
            this.framesX = (int)(spriteSheet.Width / frameSize.X);
            this.framesY = (int)(spriteSheet.Height / frameSize.Y);
            this.Position = position;
            this.Visible = true;
            this.Window = window;
        }

        public static readonly Vector2 Size = new Vector2(30, 30);
    }
}
