using Microsoft.Xna.Framework;
using YatSprint.Objects.Blocks;

namespace YatSprint.Objects.Terrains
{
    public interface ITerrain
    {
        string Name { get; }

        bool Ending { get; }

        void Generate(int length, int height);

        void ChangeWindow(Rectangle window);

        BlockChunk Last();

        BlockChunk Construct(int x);
    }
}
