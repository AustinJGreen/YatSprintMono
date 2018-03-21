using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using YatSprint.Managers;
using YatSprint.Objects;
using YatSprint.Objects.Blocks;

namespace YatSprint.Objects
{
    public class Mushroom : SpriteSheet, IEntity
    {
        public const int attackDistance = 200;

        private const float soundDimmer = 0.2F;

        private const float fireballSpeed = 4;

        private Level level;
        private Yat yat;
        private Rectangle Window;
        private SoundEffectInstance sound;
        private XPManager xpgen;
        public XPManager Manager { get { return xpgen; } }
        public Fireball fireball;

        public bool Attacking = false;
        public bool Shooting = false;
        public bool Visible { get; set; }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)frameSize.X, (int)frameSize.Y);
        }

        public bool BlockBelow(out Rectangle block)
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, !Dead());
            for (int i = 0; i < blocks.Length; i++)
            {
                Rectangle b = blocks[i].GetBoundingBox();
                Rectangle thisshroom = GetBoundingBox();
                thisshroom.Y++;
                if (b.Y >= thisshroom.Y && thisshroom.Intersects(b))
                {
                    block = b;
                    return true;
                }
            }
            block = Rectangle.Empty;
            return false;
        }

        public bool CanMove(Direction d)
        {
            if (d == Direction.Right)
            {
                Position.X += frameSize.X;
                Rectangle b;
                if (BlockBelow(out b))
                {
                    Position.X -= frameSize.X;
                    return true;
                }
                Position.X -= frameSize.X;
            }
            else
            {
                Position.X -= frameSize.X;
                Rectangle b;
                if (BlockBelow(out b))
                {
                    Position.X += frameSize.X;
                    return true;
                }
                Position.X += frameSize.X;
            }
            return false;
        }

        public void ShiftX(float amount)
        {
            Position.X += amount;
        }

        public void Update(GameTime time)
        {
            if (Visible)
            {
                UpdateFrame(time);
                if (!Shooting)
                {
                    if (CanMove(faceDirection))
                    {
                        if (faceDirection == Direction.Right)
                            Position.X += 1 * Settings.gameSpeed;
                        else
                            Position.X -= 1 * Settings.gameSpeed;
                    }
                    else
                    {
                        faceDirection = (faceDirection == Direction.Right) ? Direction.Left : Direction.Right;
                    }
                }

                if (yat != null)
                {
                    Vector2 yatPos = yat.Vector;
                    if (yatPos.Y + yat.Height == Position.Y + frameSize.Y)
                    {
                        float distance = Vector2.Distance(yatPos, Position);
                        if (distance < attackDistance && !Shooting && fireball.Ready())
                        {

                            Attacking = true;
                            Shooting = true;
                            fireball.Position = Position;
                            fireball.Play();
                            if (yatPos.X < Position.X)
                            {
                                faceDirection = Direction.Left;
                                fireball.faceDirection = Direction.Right;
                            }
                            else
                            {
                                faceDirection = Direction.Right;
                                fireball.faceDirection = Direction.Left;
                            }
                        }

                    }
                    else
                    {
                        Attacking = false;
                    }

                    if (yat.Attacking)
                    {
                        Rectangle hammer = yat.GetBoundingBox();
                        Rectangle bb = GetBoundingBox();
                        if (hammer.Intersects(bb))
                        {
#if DEBUG
                            Debugger.Log(1, "Main", string.Format("Yat killed mushroom.\n", GetHashCode()));
#endif
                            Score.Points += 1500;
                            Visible = false;
                            Play();
                            xpgen.Generate(Rand.Next(1, 5), Position);
                        }
                    }
                }
            }
            if (Shooting)
            {
                fireball.Update(time);
                bool hitYat = false;
                Rectangle fb = fireball.GetBoundingBox();
                Rectangle theyat = yat.GetBoundingBox();
                if (fb.Intersects(theyat) && !yat.Invulnerable)
                {
                    yat.healthLeft -= fireball.power;
                    hitYat = true;
#if DEBUG
                    Debugger.Log(1, "Main", string.Format("Fireball inflicted {0} damage on yat. Yat's health left {1}\n", fireball.power, yat.healthLeft));
#endif
                }

                if (fireball.faceDirection == Direction.Left)
                    fireball.Position.X += fireballSpeed * Settings.gameSpeed;
                else
                    fireball.Position.X -= fireballSpeed * Settings.gameSpeed;

                float distanceTraveled = Vector2.Distance(Position, fireball.Position);
                if (distanceTraveled > attackDistance || hitYat)
                {
                    fireball.Position = Position;
                    Shooting = false;
                    Attacking = false;
                }
            }
            xpgen.Update(time);
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            xpgen.UpdateWindow(newWindow);
            Rectangle rect = Rectangle.Empty;
            if (!BlockBelow(out rect))
            {
                int changeHeight = newWindow.Height - Window.Height;
                Position.Y += changeHeight;
                fireball.Position.Y += changeHeight;
            }
            Window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            if (Visible)
                DrawFrame(batch);
            if (Shooting)
                fireball.Draw(batch);
            xpgen.Draw(batch);
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //sound.Volume = Settings.Volume - soundDimmer;
                //sound.Play();
            }
        }

        public void Reset()
        {
            xpgen.Reset();
        }

        public bool Dead()
        {
            Rectangle bb = GetBoundingBox();
            return (bb.X + bb.Width < 0 || !Visible) && xpgen.Done;
        }

        public Mushroom(Texture2D spriteSheet, 
            Vector2 frameSize, 
            Vector2 position,
            Level level,
            Yat yat, 
            Fireball fireball, 
            SoundEffectInstance sound, 
            XPManager xpgen,
            Rectangle window)
        {
            this.animationSpeed = 10;
            this.spriteSheet = spriteSheet;
            this.frameSize = frameSize;
            this.Position = position;
            this.framesX = (int)(spriteSheet.Width / frameSize.X);
            this.framesY = (int)(spriteSheet.Height / frameSize.Y);
            this.level = level;
            this.yat = yat;
            this.fireball = fireball;
            this.sound = sound;
            this.xpgen = xpgen;
            this.Window = window;
            this.Visible = true;
        }

        public static readonly Vector2 Size = new Vector2(32, 29);
    }
}
