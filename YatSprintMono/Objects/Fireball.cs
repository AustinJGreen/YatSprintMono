using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace YatSprint.Objects
{
    public class Fireball : SpriteSheet, IObject
    {
        public int power;

        private const float soundDimmer = 0.1F;

        private SoundEffectInstance sound;

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
        }

        public void Update(GameTime time)
        {
            UpdateFrame(time);
        }

        public void UpdateWindow(Rectangle newWindow)
        {
        }

        public void Draw(SpriteBatch batch)
        {
            DrawFrame(batch);
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //sound.Volume = Settings.Volume - soundDimmer;
                //sound.Play();
            }
        }

        public bool Ready()
        {
            return Rand.Next(20) == 0;//return sound.State != SoundState.Playing;
        }

        public void Reset()
        {
        }

        public Fireball(Texture2D spriteSheet, SoundEffectInstance sound, Vector2 frameSize, Vector2 position, int power)
        {
            this.spriteSheet = spriteSheet;
            this.sound = sound;
            this.frameSize = frameSize;
            this.framesX = (int)(spriteSheet.Width / frameSize.X);
            this.framesY = (int)(spriteSheet.Height / frameSize.Y);
            this.Position = position;
            this.power = power;
        }

        public static readonly Vector2 Size = new Vector2(14, 10);
    }
}
