using Microsoft.Xna.Framework;
using YatSprint.Objects;

namespace YatSprint.Factories
{
    public interface IFactory<T>
    {
        T[] Generate();

        void UpdateWindow(Rectangle newWindow);
    }
}
