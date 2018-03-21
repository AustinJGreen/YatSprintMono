using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using YatSprint.Global;

namespace YatSprint.Objects
{
    public interface IObject
    {
        void Update(GameTime time);

        void Draw(SpriteBatch batch);

        void Reset();

        void UpdateWindow(Rectangle newWindow);
    }
}