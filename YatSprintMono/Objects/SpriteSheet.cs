using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace YatSprint.Objects
{
    public abstract class SpriteSheet
    {
        public void UpdateFrame(GameTime time)
        {
            if (index >= (framesX * framesY) - 1)
                index = 0;
            index += animationSpeed * time.ElapsedGameTime.TotalSeconds * Settings.gameSpeed;
        }
        public void DrawFrame(SpriteBatch batch)
        {
            int x = (int)Math.Floor(index) % framesX;
            int y = (int)Math.Floor(index) / framesX;
            Rectangle dest = new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
            Rectangle source = new Rectangle((int)(x * frameSize.X), (int)(y * frameSize.Y), (int)frameSize.X, (int)frameSize.Y);
            if (faceDirection == Direction.Right)
            {
                batch.Draw(spriteSheet,
                    dest,
                    source,
                    Color.White);
            }
            else
            {
                batch.Draw(spriteSheet,
                    dest,
                    source,
                    Color.White,
                    0.0F,
                    new Vector2(0),
                    SpriteEffects.FlipHorizontally,
                    0);
            }
        }
        public void DrawFrame(SpriteBatch batch, float angle)
        {
            int x = (int)Math.Floor(index) % framesX;
            int y = (int)Math.Floor(index) / framesX;
            Rectangle dest = new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
            Rectangle source = new Rectangle((int)(x * frameSize.X), (int)(y * frameSize.Y), (int)frameSize.X, (int)frameSize.Y);
            if (faceDirection == Direction.Right)
            {
                batch.Draw(spriteSheet,
                    dest,
                    source,
                    Color.White,
                    angle,
                    new Vector2(frameSize.X / 2, frameSize.Y / 2),
                    SpriteEffects.None,
                    0);
            }
            else
            {
                batch.Draw(spriteSheet,
                    dest,
                    source,
                    Color.White,
                    angle,
                    new Vector2(frameSize.X / 2, frameSize.Y / 2),
                    SpriteEffects.FlipHorizontally,
                    0);
            }
        }
        public Direction faceDirection = Direction.Right;
        public Vector2 Position;
        protected int animationSpeed = 10;
        protected double index = 0;
        protected int framesX, framesY;
        protected Vector2 frameSize;
        protected Texture2D spriteSheet;
    }
}
