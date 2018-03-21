using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public class Experiencebar : Progressbar
    {
        public override void Update(GameTime time)
        {
            if (yat != null)
            {
                Value = yat.ExperiencePoints;
                maxValue = yat.ExperienceGoal;
            }
        }

        public Experiencebar(Yat yat,
            Texture2D cage, 
            Texture2D health, 
            SpriteFont font, 
            Vector2 position, 
            Resize resize,
            int startValue,
            int maxValue,
            AmountType amountType)
        {
            this.yat = yat;
            this.cage = cage;
            this.health = health;
            this.font = font;
            this.Position = position;
            this.resize = resize;
            this.startValue = startValue;
            this.maxValue = maxValue;
            this.Value = maxValue;
            this.amountType = amountType;
            this.margin = (cage.Width - health.Width) / 2;
        }
    }
}
