using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.Objects.Potions
{
    public class HealthPotion : Potion
    {
        private float increment;
        private TimeSpan lastApplied = TimeSpan.MinValue;

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (lastApplied == TimeSpan.MinValue)
                lastApplied = time.TotalGameTime;

            if (time.TotalGameTime.Subtract(lastApplied).TotalSeconds > increment && InEffect)
            {
                lastApplied = time.TotalGameTime;
                if (yat.healthLeft + strength > Yat.health)
                    yat.healthLeft = Yat.health;
                else
                    yat.healthLeft += strength;
            }
        }

        public HealthPotion(Yat yat, Texture2D vial, SoundEffectInstance sound, Vector2 position, int strength, int duration, int timesToApply)
        {
            this.yat = yat;
            this.vial = vial;
            this.sound = sound;
            this.Position = position;
            this.strength = strength;
            this.duration = duration;
            this.increment = duration / (float)timesToApply;
            this.poisonColor = Color.Red;
        }
    }
}
