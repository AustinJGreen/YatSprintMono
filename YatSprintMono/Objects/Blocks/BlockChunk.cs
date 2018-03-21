using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Collections.Generic;

namespace YatSprint.Objects.Blocks
{
    /// <summary>
    /// Filters for different blocks
    /// </summary>
    public enum BlockFilter
    {
        Bottom,
        Middle,
        Top,
        All,
        BottomAndMiddle,
        MiddleAndTop,
        TopAndBottom
    }

    /// <summary>
    /// A Chunk of blocks
    /// </summary>
    public class BlockChunk : IObject
    {
        public bool HasLadder, TowerDone;

        public float StartX { get { return startx; } }

        public float CurrentX { get { return currentx; } }

        private Block[] blocks;

        private int[] blockLengths, spaceLengths;

        private int width, height;

        private float moveSpeed, startx, currentx;

        private bool hasLadder;

        private Texture2D bottom, middle, top, ladder;

        private Rectangle window;

        public List<Block> GetBlocks(BlockFilter type, bool onscreen)
        {
            List<Block> filtered = new List<Block>();
            for (int i = 0; i < blocks.Length; i++)
            {
                switch (type)
                {
                    case BlockFilter.All: default:
                        if (onscreen)
                            filtered.Add(blocks[i]);
                        else if (blocks[i].Dead())
                            filtered.Add(blocks[i]);
                        break;
                    case BlockFilter.Bottom:
                        if (blocks[i].Type == BlockType.Bottom)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                    case BlockFilter.Middle:
                        if (blocks[i].Type == BlockType.Middle)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                    case BlockFilter.Top:
                        if (blocks[i].Type == BlockType.Top)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                    case BlockFilter.BottomAndMiddle:
                        if (blocks[i].Type == BlockType.Bottom || blocks[i].Type == BlockType.Middle)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                    case BlockFilter.MiddleAndTop:
                        if (blocks[i].Type == BlockType.Middle || blocks[i].Type == BlockType.Top)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                    case BlockFilter.TopAndBottom:
                        if (blocks[i].Type == BlockType.Top || blocks[i].Type == BlockType.Bottom)
                        {
                            if (onscreen)
                                filtered.Add(blocks[i]);
                            else if (blocks[i].Dead())
                                filtered.Add(blocks[i]);
                        }
                        break;
                }
            }
            return filtered;
        }

        public void Generate()
        {
            int spaces = 0;
            blocks = new Block[height];
            int blockIndex = 0;
            int blockLengthIndex = 0;
            for (int i = 0; i < height; i++)
            {
                Texture2D texture = middle;
                BlockType type = BlockType.Middle;
                if (blockIndex == 0 && blockLengths[blockLengthIndex] != 1)
                {
                    texture = bottom;
                    type = BlockType.Bottom;
                }
                else if (blockIndex == blockLengths[blockLengthIndex] - 1)
                {
                    texture = top;
                    type = BlockType.Top;
                }

                blocks[i] = new Block(texture,
                                        ladder,
                                        startx,
                                        window.Height - (texture.Height * (i + spaces + 1)),
                                        type,
                                        HasLadder,
                                        window);

                blockIndex++;
                if (blockIndex >= blockLengths[blockLengthIndex])
                {
                    if (blockLengthIndex < spaceLengths.Length)
                        spaces += spaceLengths[blockLengthIndex];
                    blockIndex = 0;
                    blockLengthIndex++;

                }
            }
        }

        public void Update(GameTime time)
        {
            currentx += -moveSpeed;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].ShiftX(-moveSpeed * Settings.gameSpeed);
                if (blocks[i].X <= -width)
                    TowerDone = true;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            window = newWindow;
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Y = window.Height - (blocks[i].Size.Y * (i + 1));
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < blocks.Length; i++)
                blocks[i].Draw(batch);
        }

        public void Reset()
        {
            blocks = null;
        }

        /// <summary>
        /// Creates a chunk of blocks
        /// </summary>
        /// <param name="textures">Textures to use</param>
        /// <param name="ladder">Texture for ladder</param>
        /// <param name="spaceLengths">Indices to insert no blocks</param>
        /// <param name="x">Starting X Position of the chunk</param>
        /// <param name="width">Width of the whole chunk</param>
        /// <param name="moveSpeed">Speed to move the chunk</param>
        /// <param name="hasLadder">Create a ladder on this chunk</param>
        /// <param name="window">The game window</param>
        public BlockChunk(Texture2D bottom,
            Texture2D middle,
            Texture2D top,
            Texture2D ladder, 
            int[] blockLengths,
            int[] spaceLengths, 
            int x, 
            int width, 
            float moveSpeed, 
            bool hasLadder, 
            Rectangle window)
        {
            this.bottom = bottom;
            this.middle = middle;
            this.top = top;
            this.ladder = ladder;
            this.blockLengths = blockLengths;
            this.spaceLengths = spaceLengths;
            this.startx = x;
            this.currentx = x;
            this.width = width;
            this.height = blockLengths.Sum();
            this.moveSpeed = moveSpeed;
            this.hasLadder = hasLadder;
            this.window = window;
        }
    }
}
