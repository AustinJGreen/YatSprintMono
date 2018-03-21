using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.Objects.Potions
{
    public class JumpPotion : Potion
    {
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (InEffect)
            {
                yat.HasJumpPotion = true;
                yat.jumpSpeed = 1.01F;
            }
            else
            {
                yat.HasJumpPotion = false;
                yat.jumpSpeed = Yat.normalJumpSpeed;
            }
        }

        public override void End()
        {
            yat.HasJumpPotion = false;
            yat.jumpSpeed = Yat.normalJumpSpeed;
            base.End();
        }

        public JumpPotion(Yat yat, Texture2D vial, SoundEffectInstance sound, Vector2 position, int duration)
        {
            this.yat = yat;
            this.vial = vial;
            this.sound = sound;
            this.Position = position;
            this.strength = strength;
            this.duration = duration;
            this.poisonColor = Color.Blue;
        }
    }
}
