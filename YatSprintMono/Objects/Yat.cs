using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using YatSprint.Global;
using YatSprint.Managers;
using YatSprint.Objects.Blocks;
using YatSprint.UI;

namespace YatSprint.Objects
{
    public enum Direction
    {
        Right, 
        Left
    }
    public enum Animation
    {
        Climbing = 0,
        Running,
        Smashing,
        Falling
    }

    public class Yat : MarshalByRefObject, IObject
    {
        public const int animationSpeed = 10;
        public const int health = 300;
        public const float jumpVelocity = 7;
        public const float normalJumpSpeed = 1.05F;
        public const float soundDimmer = 0.0F;
    

        private Texture2D[][] yatSprites;
        private Vector2 Position = new Vector2(0);
        private Level level;
        private Animation current = Animation.Running;
        private Direction faceDirection = Direction.Right;
        //private SoundEffectInstance snap;
        private Rectangle window;
        private PotionManager potionManager;
        private float? JumpClimax = null;
        public float moveSpeedR = 3;
        public float moveSpeedL = 4.5F;
        public float climbSpeedU = 3;
        public float climbSpeedD = 3;
        private float Velocity = jumpVelocity;
        private double index = 0;
        private bool falling = false;

        public Color? poison;
        public bool HitGround = false;
        public bool HasJumpPotion = false;
        public bool Invulnerable = false;
        public bool Climbing = false;
        public bool Jumping = false;
        public bool Attacking = false; //Used by mushroom
        public bool BeingCarriedByBird = false; //Used by Bird
        public float jumpSpeed = normalJumpSpeed; //Used by jump potion
        public float BirdAngle = 0; //Used by Bird
        public int healthLeft = health; //Used by healthbar
        public int ExperiencePoints = 0;
        public int ExperienceGoal = 100;
        public int ExperienceIncrement = 100;

        public Vector2 Vector
        {
            get
            {
                return Position;
            }
            set
            {
                Position = value;
            }
        }

        public int Height
        {
            get
            {
                return yatSprites[(int)current][(int)Math.Floor(index)].Height;
            }
        }

        public Rectangle GetBoundingBox()
        {
            const int offsetX = 26;
            const int offsetY = 26;
            const int offsetZ = 14;
            if (faceDirection == Direction.Right && current != Animation.Smashing)
                return new Rectangle((int)Position.X, (int)Position.Y + offsetZ, offsetX, offsetY);
            return new Rectangle((int)Position.X + offsetX, (int)Position.Y + offsetZ, offsetX, offsetY);
        }

        public void ChangeAnimation(Animation desired)
        {
            if (index >= yatSprites[(int)desired].Length)
                index = 0;
            current = desired;
        }

        public bool BlockBelow(out Rectangle block)
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, true);
            for (int i = 0; i < blocks.Length; i++)
            {
                Rectangle b = blocks[i].GetBoundingBox();
                Rectangle thisyat = GetBoundingBox();
                thisyat.Y++;
                if (b.Y >= thisyat.Y && thisyat.Intersects(b))
                {
                    block = b;
                    return true;
                }
            }
            block = Rectangle.Empty;
            return false;          
        }

        public bool LadderCheck(out Rectangle block)
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, true);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (!blocks[i].HasLadder)
                {
                    Rectangle b = blocks[i].GetBoundingBox();
                    Rectangle thisyat = GetBoundingBox();
                    thisyat.Y++;
                    if (b.Y >= thisyat.Y && thisyat.Intersects(b))
                    {
                        block = b;
                        return true;
                    }
                }
            }
            block = Rectangle.Empty;
            return false;
        }

        public bool IntersectsLadder()
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, true);
            for (int i = 0; i < blocks.Length; i++)
            {
                if (blocks[i].HasLadder)
                {
                    Rectangle b = blocks[i].GetBoundingBox();
                    Rectangle thisyat = GetBoundingBox();
                    thisyat.Y++;
                    if (thisyat.Intersects(b))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void ShiftX(float amount)
        {
            Position.X += amount;
        }

        public void Update(GameTime time)
        {
            Score.Points++;
            KeyboardState state = Keyboard.GetState();
            index += animationSpeed * time.ElapsedGameTime.TotalSeconds * Settings.gameSpeed;
            if (index > yatSprites[(int)current].Length)
            {
                index = 0;
                if (Attacking)
                    Attacking = false;
                if (current == Animation.Smashing)
                    ChangeAnimation(Animation.Running);
            }
            else if (index > yatSprites[(int)current].Length - 2 && current == Animation.Smashing)
                Attacking = true;

            if ((state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift)) && current != Animation.Smashing)
            {
                if (current == Animation.Running)
                {
                    ChangeAnimation(Animation.Smashing);
                }
            }
            if ((state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D)) && !Climbing && !BeingCarriedByBird)
            {
                Position.X += moveSpeedR * Settings.gameSpeed;
                faceDirection = Direction.Right;
            }
            if ((state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A)) && !Climbing && !BeingCarriedByBird)
            {
                Position.X -= moveSpeedL * Settings.gameSpeed;
                faceDirection = Direction.Left;
            }
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                if (Climbing)
                {
                    Position.Y += climbSpeedD * Settings.gameSpeed;
                    Rectangle rect = Rectangle.Empty;
                    if (LadderCheck(out rect))
                    {
                        ChangeAnimation(Animation.Running);
                        Climbing = false;
                    }
                }
                else
                {
                    Rectangle rect = Rectangle.Empty;
                    if (IntersectsLadder())
                    {
                        ChangeAnimation(Animation.Climbing);
                        Climbing = true;
                    }
                }
            }
            else if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                bool intersecting = false;
                Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.All, true);
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i].HasLadder)
                    {
                        Rectangle yat = GetBoundingBox();
                        Rectangle block = blocks[i].GetBoundingBox();
                        if (yat.Intersects(block))
                        {
                            ChangeAnimation(Animation.Climbing);
                            intersecting = true;
                            Climbing = true;
                        }
                    }
                }
                if (!intersecting)
                {
                    ChangeAnimation(Animation.Running);
                    Climbing = false;
                }
                if (Climbing)
                {
                    Position.Y -= climbSpeedU * Settings.gameSpeed;
                }
            }
            if (state.IsKeyDown(Keys.Space) && !BeingCarriedByBird)
            {
                Jumping = true;
            }

            if (Jumping && !BeingCarriedByBird)
            {
                if (Velocity > 0 && Velocity < 2)//falling
                {
                    ChangeAnimation(Animation.Falling);
                    if (JumpClimax == null)
                        JumpClimax = Position.Y;
                    Velocity *= -1;
                    falling = true;
                }
                Position.Y -= Velocity;
                if (falling)
                    Velocity *= jumpSpeed;
                else
                    Velocity /= jumpSpeed;
                Rectangle layer;
                if (BlockBelow(out layer) && Velocity < 0) //Checks only if the yat is falling (aka negative velocity)
                {
                    HitGround = true;
                    if (JumpClimax.Value - Position.Y < -200 && !Invulnerable && !HasJumpPotion) //3 blocks
                    {
#if DEBUG
                        Debugger.Log(1, "Main", "Yat fell too high and lost damage.\n");
#endif
                        healthLeft -= 50;
                        Play();
                    }
                    if (layer != Rectangle.Empty)
                        Position.Y = layer.Y - Height;
                    Jumping = false;           
                    ChangeAnimation(Animation.Running);
                    Velocity = jumpVelocity;
                    JumpClimax = null;
                    falling = false;
                }
            }


            if (Climbing)
            {
                Position.X -= Level.moveSpeed * Settings.gameSpeed;
            }
            else if (!Jumping)
            {
                Rectangle rect = Rectangle.Empty;
                if (!BlockBelow(out rect))
                {
                    ChangeAnimation(Animation.Falling);
                    Position.Y += 4 * Settings.gameSpeed;
                }
                else 
                {
                    HitGround = true;
                    if (current != Animation.Smashing)    
                        ChangeAnimation(Animation.Running);
                }

                if (rect != Rectangle.Empty)
                {
                    Position.Y = rect.Y - Height;
                }
            }

            if (healthLeft <= 0)
                Actions.Gameover = true;
            if (ExperiencePoints >= ExperienceGoal)
            {
                ExperienceGoal += ExperienceIncrement;
                ExperiencePoints = 0;
                //Award potion....
#if DEBUG
                Debugger.Log(1, "Main", "Awarding potion(s)...\n");
#endif
                potionManager.Generate();


            }
            potionManager.Update(time);
            if (Position.Y > window.Height) //Last check5
                Actions.Gameover = true;
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            potionManager.UpdateWindow(newWindow);
            window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {      
            int absl = (int)Math.Floor(index);
            if (absl >= yatSprites[(int)current].Length)
                absl = 0;
            Texture2D texture = yatSprites[(int)current][absl];
            Color tintyat = poison ?? Color.White;
            if (faceDirection == Direction.Left)
            {
                if (BeingCarriedByBird)
                    batch.Draw(texture, Position, texture.Bounds, tintyat, BirdAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.FlipHorizontally, 0);
                else
                    batch.Draw(texture, Position, texture.Bounds, tintyat, 0, new Vector2(0), 1, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                if (BeingCarriedByBird)
                    batch.Draw(texture, Position, texture.Bounds, tintyat, BirdAngle, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
                else
                    batch.Draw(texture, Position, tintyat);
            }
            potionManager.Draw(batch);
        }

        public void Play()
        {
            if (Settings.PlaySounds)
            {
                //snap.Volume = Settings.Volume - soundDimmer;
                //snap.Play();
            }
        }

        public void Init(Level level, PotionManager pm)
        {
            this.level = level;
            this.potionManager = pm.New();     
        }

        public void Reset()
        {
            BeingCarriedByBird = false;
            Position = new Vector2(0);
            faceDirection = Direction.Right;
            healthLeft = health;
            ExperiencePoints = 0;
            ExperienceGoal = ExperienceIncrement;
            Jumping = false;
            falling = false;
            Climbing = false;
            HitGround = false;
        }

        public Yat(Texture2D[] yatClimbing, Texture2D[] yatRunning, Texture2D[] yatSmashing, Texture2D[] yatFalling, SoundEffect snap, Rectangle bounds)
        {
            this.yatSprites = new Texture2D[4][];
            this.yatSprites[0] = yatClimbing;
            this.yatSprites[1] = yatRunning;
            this.yatSprites[2] = yatSmashing;
            this.yatSprites[3] = yatFalling;
            //this.snap = snap.CreateInstance();
            this.window = bounds;
        }
    }
}