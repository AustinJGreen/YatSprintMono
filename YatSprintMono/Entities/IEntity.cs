using YatSprint.Managers;

namespace YatSprint.Objects
{
    public interface IEntity : ILevelObject
    {
        XPManager Manager { get; }       
    }
}
