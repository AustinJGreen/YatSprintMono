using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YatSprint.UI;

namespace YatSprint
{
    public class Score : Button
    {
        public override void Update(GameTime time)
        {
            ChangeText(Score.Points.ToString("n0"));
            base.Update(time);        
        }

        public Score(Vector2 position, Resize resize, Rectangle window, SpriteFont font, Color textColor) : base(position, resize, window, "0", font, true, textColor, Color.Black, false, null) { }

        public static int Points = 0;
        public static int Multiplier = 1;
    }
}
