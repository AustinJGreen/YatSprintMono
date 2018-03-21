using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace YatSprint.UI
{
    public class FPSCounter : Button
    {
        private TimeSpan? lastFPSCount;
        private double frames;
        private double fps = 60;

        public override void Update(GameTime time)
        {
            //Checks to see if lastFPSCount has been set yet
            if (!lastFPSCount.HasValue)
                lastFPSCount = time.TotalGameTime;

            //If has been counting for more than a second
            if (time.TotalGameTime.Subtract(lastFPSCount.Value).TotalSeconds > 1)
            {
#if DEBUG
                if (frames < fps)
                    Debugger.Log(1, "Main", string.Format("FPS has decresed by a value of {0}!\n", fps - frames));
                if (frames > fps)
                    Debugger.Log(1, "Main", string.Format("FPS has increased by a value of {0}!\n", frames - fps));
#endif
                //Set fps to frames
                fps = frames;
                //Reset frames back to 0
                frames = 0;
                //Reset lastFPSCount to current time
                lastFPSCount = time.TotalGameTime;
                //Change text to updated fps count
                ChangeText(string.Format("FPS: {0}", Math.Round(fps, 2)));

            }
            else
            {
                //If hasn't met limit, continue counting
                frames++;
            }

            base.Update(time);
        }

        public FPSCounter(Vector2 position, Resize resize, Rectangle window, SpriteFont font, Color textColor) : base(position, resize, window, "FPS: 60", font, false, textColor, Color.Black, false, null) { }
    }
}
