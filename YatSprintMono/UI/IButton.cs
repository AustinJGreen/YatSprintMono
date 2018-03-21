using YatSprint.Global;
using YatSprint.Objects;

namespace YatSprint.UI
{
    public interface IButton : IObject
    {
        bool RequestScreen(out Request screen);
    }
}
