using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using YatSprint.Objects;
using YatSprint.Objects.Blocks;
using YatSprint.Objects.Terrains;

namespace YatSprint.Factories
{
    public class BlockManager : IObject
    {
        public bool Initialized { get; set; }

        public float LevelMoveSpeed { get; set; }

        public int TowersCreated { get; set; }

        private const int maxHeight = 7;

        private const int minHeight = 2;

        private const int groundWidth = 50;

        private int extraLength = 10;

        private int lastHeight = 2;

        private Rectangle window;

        private Texture2D bottom, middle, top, ladder;

        private List<BlockChunk> chunks;

        public ITerrain CurrentTerrain;

        /// <summary>
        /// Get certain blocks based on a filter
        /// </summary>
        /// <param name="filter">Filter to get blocks</param>
        /// <param name="onscreen">Should the blocks be visible on the screen</param>
        /// <returns>Array of filtered blocks</returns>
        public Block[] GetAllBlocks(BlockFilter filter, bool onscreen)
        {
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < chunks.Count; i++)
                blocks.AddRange(chunks[i].GetBlocks(filter, onscreen));            
            return blocks.ToArray();
        }

        public void Initialize()
        {
#if DEBUG
            Debugger.Log(1, "Main", "Initializing level block manager.");
#endif
            Initialized = true;
            chunks.Clear();
            int x = 0;
            int amountOfTowers = (window.Width / groundWidth) + extraLength + 1;
            TowersCreated = amountOfTowers;
            CurrentTerrain.Generate(Rand.Next(30, 50), lastHeight);
            for (int i = 0; i < amountOfTowers; i++)
            {
                BlockChunk tower = CurrentTerrain.Construct(x);
                tower.Generate();
                chunks.Add(tower);
                x += groundWidth;
            }
        }

        public void Update(GameTime time)
        {
            List<int> towersDone = new List<int>();
            for (int i = 0; i < chunks.Count; i++)
            {
                chunks[i].Update(time);
                if (chunks[i].TowerDone)
                {
                    towersDone.Add(i);
                }
            }
            for (int j = towersDone.Count - 1; j >= 0; j--)          
                chunks.RemoveAt(towersDone[j]);
            for (int k = 0; k < towersDone.Count; k++)
            {
                int newHeight = lastHeight + (Rand.Next(-1, 2));
                if (newHeight > maxHeight)
                    newHeight = maxHeight;
                if (newHeight <= minHeight)
                    newHeight = minHeight;
                int x = (int)CurrentTerrain.Last().CurrentX + groundWidth;
                BlockChunk next = CurrentTerrain.Construct(x);
                next.Generate();
                chunks.Add(next);
                lastHeight = newHeight;
                TowersCreated++;
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            if (CurrentTerrain != null)
                CurrentTerrain.ChangeWindow(newWindow);
            if (chunks.Count > 0)
            {
                for (int i = 0; i < chunks.Count; i++)
                    chunks[i].UpdateWindow(newWindow);
                int oldamountOfTowers = (window.Width / groundWidth) + extraLength + 1;
                int newamountOfTowers = (newWindow.Width / groundWidth) + extraLength + 1;
                int towersToCreate = Math.Abs(newamountOfTowers - oldamountOfTowers) + 1;
                float x = CurrentTerrain.Last().CurrentX + groundWidth;
                for (int i = 0; i < towersToCreate; i++)
                {
                    BlockChunk current = CurrentTerrain.Construct((int)x);
                    current.Generate();
                    chunks.Add(current);
                    x += groundWidth;
                    TowersCreated++;
                }
            }
            window = newWindow;
        }

        public void Draw(SpriteBatch batch)
        {
            for (int i = 0; i < chunks.Count; i++)
                chunks[i].Draw(batch);
        }

        public void Reset()
        {
            Initialized = false;
            chunks.Clear();
            TowersCreated = 0;
        }

        /// <summary>
        /// Sync the level generation to a terrain
        /// </summary>
        /// <param name="terrain"></param>
        public void SetTerrain(ITerrain terrain)
        {
            this.CurrentTerrain = terrain;
        }

        /// <summary>
        /// Create a blockmanager which holds textures and block chunk data
        /// </summary>
        /// <param name="bottom">Bottom texture</param>
        /// <param name="middle">Middle texture</param>
        /// <param name="top">Top texture</param>
        /// <param name="ladder">Ladder texture</param>
        /// <param name="window">Game window</param>
        public BlockManager(Texture2D bottom, Texture2D middle, Texture2D top, Texture2D ladder, Rectangle window)
        {
            this.bottom = bottom;
            this.middle = middle;
            this.top = top;
            this.ladder = ladder;
            this.LevelMoveSpeed = Level.moveSpeed;
            this.window = window;
            this.chunks = new List<BlockChunk>();
        }
    }
}
