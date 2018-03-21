using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using YatSprint.Objects.Terrains;

namespace YatSprint.Objects.Blocks
{
    public class Plains : ITerrain
    {
        public string Name { get { return "Grassy Hills"; } }
        public bool Ending { get; private set; }
        public int Length = - 1;

        private Rectangle window;
        private BlockChunk lastChunk;
        private Texture2D bottom, middle, top, ladder;
        private int[][] BlockLengths, SpaceLengths;
        private int index = -1;
        private int minHeight;
        private int maxHeight;

        private int hills = 1;

        public void Generate(int length, int height)
        {
            this.Length = length;
            int amountOfSections = Rand.NextExp(1, 4, 1);
            int H = height;
            int currentHill = 1;
            int[][] blocklengths = new int[Length][];
            int[][] spacelengths = new int[Length][];
            for (int i = 0; i < Length; i++)
            {
                if (i < Length / 2)
                {
                    int e = Rand.Next(0, 2);
                    if (H + e > maxHeight)
                        e = 0;
                    H += e;
                }
                else //if (i < (Length / hills) * (currentHill + 1))
                {
                    int e = Rand.Next(-1, 1);
                    if (H + e <= minHeight)
                        e = 0;
                    H += e;
                }
                //else
                //{
                //    currentHill++;
                //}

                //if (false)
                //{
                //    blocklengths[i] = new int[2];
                //    int[] parts = Rand.Interpolate(2, H - 4, false);
                //    for (int j = 0; j < parts.Length; j++)
                //        blocklengths[i][j] = parts[j]; 
                //    spacelengths[i] = new int[1] { H - 4 };
                //}
                //else
                //{
                blocklengths[i] = new int[1];
                blocklengths[i][0] = H;
                spacelengths[i] = new int[0];
                //}

                
                //int decrease = Rand.NextExp(1, 10, 1);
                //if (decrease == 8 && amountOfSections > 1)
                //    amountOfSections--;
                //if (decrease == 9 && amountOfSections < 4 && H > 5)
                //    amountOfSections++;
            }
            this.BlockLengths = blocklengths;
            this.SpaceLengths = spacelengths;
        }

        public void ChangeWindow(Rectangle newWindow)
        {
            this.window = newWindow;
        }

        public BlockChunk Last()
        {
            if (lastChunk == null)
                throw new Exception("No chunks generated.");
            return lastChunk;
        }

        public BlockChunk Construct(int x)
        {
            index++;
            if (index >= Length)
            {
                index = 0;
                Ending = true;
            }

            BlockChunk chunk = new BlockChunk(bottom, middle, top, ladder, BlockLengths[index], SpaceLengths[index], x, 50, Level.moveSpeed, false, window);
            lastChunk = chunk;
            return chunk;
        }

        public Plains(Texture2D bottom, Texture2D middle, Texture2D top, Texture2D ladder, int minHeight, int maxHeight, int hills, Rectangle window)
        {
            this.bottom = bottom;
            this.middle = middle;
            this.top = top;
            this.ladder = ladder;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.hills = hills;
            this.window = window;
            //Please
        }
    }
}
