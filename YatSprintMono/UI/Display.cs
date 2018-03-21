using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using YatSprint.Global;

namespace YatSprint.UI
{
    public class Display
    {
        private List<Screen> screens;

        public void Add(Screen screen)
        {
            screens.Add(screen);
            screens.Sort(new ScreenSort());
        }

        public int CurrentScreen { get; set; }

        public void SwitchScreen(int index)
        {
#if DEBUG
            Debugger.Log(1, "Main", string.Format("Switching to screen {0}.\n", index));
#endif
            screens[CurrentScreen].Reset(screens[index]);
            CurrentScreen = index;
            screens[CurrentScreen].Begin();
        }

        public void Update(YatSprintGame game, GameTime time)
        {
            if (Actions.Gameover && CurrentScreen != 2)
                SwitchScreen(2);
            Screen current = screens[CurrentScreen];
            current.Update(time);
            Request best = null;
            int lastStr = 0;
            Request r;
            if (current.RequestScreen(out r))
            {
                if (r != null)
                {
                    if (r.Strength > lastStr)
                    {
                        best = r;
                        lastStr = r.Strength;
                    }
                }
            }
            if (best != null)
            {
                if (best.Index != -1)
                    SwitchScreen(best.Index);
                if (best.Actions != null)
                    for (int i = 0; i < best.Actions.Length; i++)
                        best.Actions[i].Invoke();
                if (Actions.Exitgame)
                    game.Exit();
                if (Actions.ToggleFullscreen)
                    game.graphics.ToggleFullScreen();
                Actions.Exitgame = false;
                Actions.ToggleFullscreen = false;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                screens[i].UpdateWindow(newWindow);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            Screen current = screens[CurrentScreen];
            current.Draw(batch);
        }

        public Display()
        {
            screens = new List<Screen>();
        }
    }
}
