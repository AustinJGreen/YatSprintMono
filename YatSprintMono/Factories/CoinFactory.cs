using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YatSprint.Objects;
using YatSprint.Objects.Blocks;

namespace YatSprint.Factories
{
    public class CoinFactory : IFactory<ILevelObject>
    {
        private Texture2D spriteSheet;
        private SoundEffect coinEffect;
        private Level level;
        private Yat yat;
        private Rectangle window;

        public ILevelObject[] Generate()
        {
            Block[] blocks = level.Manager.GetAllBlocks(BlockFilter.Top, false);
            List<Coin> coins = new List<Coin>();
            for (int i = 0; i < blocks.Length; i++)
            {
                if (Rand.Next(5) == 0)
                {
                    Coin current = new Coin(yat, level, spriteSheet, null, Coin.Size, new Vector2(blocks[i].X + (Coin.Size.X / 2), blocks[i].Y - Coin.Size.Y), window);
                    coins.Add(current);
                }
            }
            return coins.ToArray();
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            window = newWindow;
        }

        public CoinFactory(Level level, Yat yat, Texture2D spriteSheet, SoundEffect coinEffect, Rectangle window)
        {
            this.level = level;
            this.yat = yat;
            this.spriteSheet = spriteSheet;
            this.coinEffect = coinEffect;
            this.window = window;
        }
    }
}
