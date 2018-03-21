using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace YatSprint.Objects.Potions
{
    public class SpeedPotion : Potion
    {
        private bool applied = false;
        public override void Update(GameTime time)
        {
            base.Update(time);

            if (InEffect && !applied)
            {
                yat.moveSpeedL += strength;
                yat.moveSpeedR += strength;
                yat.climbSpeedU += strength;
                yat.climbSpeedD += strength;
                applied = true;
            }
        }

        public override void End()
        {
            yat.moveSpeedL -= strength;
            yat.moveSpeedR -= strength;
            yat.climbSpeedU -= strength;
            yat.climbSpeedD -= strength;
            base.End();
        }

        public SpeedPotion(Yat yat, Texture2D vial, SoundEffectInstance sound, Vector2 position, int duration, int strength)
        {
            this.yat = yat;
            this.vial = vial;
            this.sound = sound;
            this.Position = position;
            this.strength = strength;
            this.duration = duration;
            this.strength = strength;
            this.poisonColor = Color.Yellow;
        }
    }
}
