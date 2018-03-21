using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using YatSprint.Global;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public class Screen
    {
        public int Index { get; set; }

        public bool RequireResets { get; set; }

        public IButton[] Buttons { get; set; }

        public IObject[] Objects { get; set; }

        public Song Song { get; set; }

        public void Update(GameTime time)
        {
            for (int i = 0; i < Objects.Length; i++)
                Objects[i].Update(time);
            for (int i = 0; i < Buttons.Length; i++)
                Buttons[i].Update(time);
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < Objects.Length; i++)
                Objects[i].Draw(batch);
            for (int i = 0; i < Buttons.Length; i++)
                Buttons[i].Draw(batch);
        }

        public bool RequestScreen(out Request screen)
        {
            Request request;
            for (int i = 0; i < Buttons.Length; i++)
            {
                if (Buttons[i].RequestScreen(out request))
                {
                    screen = request;
                    return true;
                }
            }
            screen = null;
            return false;
        }

        public void Begin()
        {
            if (Song != null && MediaPlayer.Queue.ActiveSong != Song)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(Song);   
            }
        }

        public void Reset(Screen destination)
        {
            if (destination.RequireResets)
            {
                for (int i = 0; i < Objects.Length; i++)
                    Objects[i].Reset();
                for (int i = 0; i < Buttons.Length; i++)
                    Buttons[i].Reset();
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            for (int i = 0; i < Buttons.Length; i++)
                Buttons[i].UpdateWindow(newWindow);
            for (int j = 0; j < Objects.Length; j++)
                Objects[j].UpdateWindow(newWindow);
        }

        public Screen(int index, IButton[] buttons, IObject[] objects, bool requireResets)
        {
            this.Index = index;
            this.Buttons = buttons;
            this.Objects = objects;
            this.RequireResets = requireResets;
        }

        public Screen(int index, IButton[] buttons, IObject[] objects, Song song, bool requireResets) : this(index, buttons, objects, requireResets)
        {
            this.Song = song;
        }
    }
}
