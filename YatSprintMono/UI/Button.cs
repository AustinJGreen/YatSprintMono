using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using YatSprint.Global;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public class Button : MarshalByRefObject, IButton
    {
        private Texture2D background;
        private Color currentColor;
        private bool MouseDown = false, Rotating;
        private float degreeRot;

        public Rectangle BoundingBox { get; private set; }
        public bool Center { get; private set; }
        public bool Clicked { get; private set; }
        public SpriteFont Font { get; private set; }
        public bool Highlight { get; private set; }
        public Color HighlightColor { get; private set; }
        public Vector2 Position { get; private set; }
        public Resize Resize { get; private set; }
        public Request Request { get; private set; }
        public string Text { get; private set; }
        public Color TextColor { get; private set; }
        public Rectangle Window { get; private set; }

        public void ChangeText(string text)
        {
            this.Text = text;
            Vector2 textSize = Font.MeasureString(Text);
            if (Center)
                Position = new Vector2((Window.Width / 2) - (textSize.X / 2), Position.Y);
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, (int)textSize.X, (int)textSize.Y);
        }

        public void SetRotation(float degrees)
        {
            this.Rotating = true;
            this.degreeRot = degrees;
        }

        public virtual void Update(GameTime time)
        {
            if (Clicked)
                Clicked = false;
            int x = Mouse.GetState().X;
            int y = Mouse.GetState().Y;
            if (BoundingBox.Contains(x, y))
            {
                if (Highlight)
                    currentColor = HighlightColor;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    MouseDown = true;
                else if (MouseDown)
                {
                    Clicked = true;
                    MouseDown = false;
                }
            }
            else
            {
                currentColor = TextColor;
                MouseDown = false;
            }
        }

        public virtual void UpdateWindow(Rectangle newWindow)
        {
            Position = Resize.GetPosition(newWindow);
            Vector2 size = Font.MeasureString(Text);
            if (Center)
                Position = new Vector2(Position.X - (size.X / 2), Position.Y - (size.Y / 2));
            BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y);
            Window = newWindow;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (background != null)
                batch.Draw(background, Window, Color.White);
            if (Rotating)
            {
                float rad = MathHelper.ToRadians(degreeRot);
                Vector2 size = Font.MeasureString(Text);
                batch.DrawString(Font, Text, Position, currentColor, rad, new Vector2(size.X / 2, size.Y / 2), 1, SpriteEffects.None, 0);
            }
            else
                batch.DrawString(Font, Text, Position, currentColor);
        }

        public virtual bool RequestScreen(out Request screen)
        {
            screen = Request;
            return Clicked;
        }

        public virtual void Reset()
        {
        }

        public Button(Vector2 position, 
            Resize resize,
            Rectangle window, 
            string text, 
            SpriteFont font, 
            bool center, 
            Color textColor, 
            Color highlightColor,
            bool highlight,
            Request makeRequest)
        {
            this.Position = position;
            this.Resize = resize;
            this.Window = window;
            this.Text = text;
            this.Font = font;
            this.Center = center;
            this.TextColor = textColor;
            this.Highlight = highlight;
            this.HighlightColor = highlightColor;
            this.currentColor = textColor;
            this.Request = makeRequest;
            Vector2 textSize = Font.MeasureString(Text);
            if (center)
                Position = new Vector2((window.Width / 2) - (textSize.X / 2), Position.Y);
            BoundingBox = new Rectangle((int)Position.X, (int)position.Y, (int)textSize.X, (int)textSize.Y);
        }

        public Button(Vector2 position,
            Resize resize,
            Rectangle window,
            string text,
            SpriteFont font,
            bool center,
            Color textColor,
            Color highlightColor,
            bool highlight,
            Request makeRequest,
            Texture2D background) : this(position,
            resize,
            window,
            text,
            font,
            center,
            textColor,
            highlightColor,
            highlight,
            makeRequest)
        {
            this.background = background;
        }
    }
}
