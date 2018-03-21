using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using YatSprint.Global;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public class ImageButton : IButton
    {
        private enum InvokeType
        {
            Action,
            Request
        }

        public Texture2D[] Images;

        public Vector2 Position;

        public Action[] Actions;

        public Request[] Requests;

        public int Index = 0;

        public int ActionIndex = 0;

        private Rectangle Window;

        private Resize resize;

        private bool MouseDown, KeyDown;

        private bool Clicked, KeyPressed;

        private bool ResetIndices;

        private char? hotkey;

        private InvokeType invokeType;

        private Rectangle BoundingBox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Images[Index].Bounds.Width, Images[Index].Bounds.Height);
        }

        public void Update(GameTime time)
        {
            if (Clicked)
                Clicked = false;
            if (KeyDown)
            {
                KeyDown = false;
                if (Keyboard.GetState().IsKeyUp((Keys)hotkey.Value))
                    KeyPressed = true;
            }
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;
            KeyDown = (hotkey.HasValue) ? Keyboard.GetState().IsKeyDown((Keys)hotkey.Value) : false;
            if (BoundingBox().Contains(x, y) || KeyPressed)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    MouseDown = true;
                else if (MouseDown || KeyPressed)
                {
                    Clicked = true;
                    MouseDown = false;
                    KeyPressed = false;
                    if (invokeType == InvokeType.Action)
                        Actions[ActionIndex].Invoke();
                    Index++;
                    if (Index >= Images.Length)
                        Index = 0;
                    resize.ChangeSize(new Vector2(Images[Index].Width, Images[Index].Height));
                    ActionIndex++;
                    if (invokeType == InvokeType.Action)
                    {
                        if (ActionIndex >= Actions.Length)
                            ActionIndex = 0;
                    }
                    else
                    {
                        if (ActionIndex >= Requests.Length)
                            ActionIndex = 0;
                    }
                }
            }
            else
            {
                MouseDown = false;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            Position = resize.GetPosition(newWindow);
            Window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            Texture2D current = Images[Index];
            batch.Draw(current, Position, Color.White);
        }

        public bool RequestScreen(out Request screen)
        {
            screen = null;
            if (invokeType == InvokeType.Request && Requests.Length > 0)
            {
                if (Clicked)
                {
                    screen = Requests[ActionIndex];
                    return true;
                }
            }
            return false;
        }

        public void Reset()
        {
            if (ResetIndices)
            {
                Index = 0;
                ActionIndex = 0;
                MouseDown = false;
                Clicked = false;
                KeyDown = false;
                KeyPressed = false;
            }
        }

        public ImageButton(Vector2 position, Resize resize, Action[] requests, Rectangle window, char? hotkey, bool resetIndices, params Texture2D[] images)
        {
            this.Position = position;
            this.resize = resize;
            this.Actions = requests;
            this.Requests = new Request[0];
            this.Window = window;
            this.hotkey = hotkey;
            this.ResetIndices = resetIndices;
            this.Images = images;
            this.invokeType = InvokeType.Action;
        }

        public ImageButton(Vector2 position, Resize resize, Request[] requests, Rectangle window, char? hotkey, bool resetIndices, params Texture2D[] images)
        {
            this.Position = position;
            this.resize = resize;
            this.Actions = new Action[0];
            this.Requests = requests;
            this.Window = window;
            this.hotkey = hotkey;
            this.ResetIndices = resetIndices;
            this.Images = images;
            this.invokeType = InvokeType.Request;
        }
    }
}
