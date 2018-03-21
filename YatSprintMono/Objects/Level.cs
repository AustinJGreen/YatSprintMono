using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using YatSprint.Factories;
using YatSprint.Global;
using YatSprint.Objects.Terrains;

namespace YatSprint.Objects
{
    /// <summary>
    /// Level class for managing factories, mobs, and terrains
    /// </summary>
    public class Level : MarshalByRefObject, IObject
    {
        public static float moveSpeed = 2.0F;

        public BlockManager Manager { get { return blockManager; } }

        private int waveNumber = 1;

        private Rectangle window;

        private BlockManager blockManager;

        private ITerrain[] terrains;

        private IFactory<IEntity>[] Egenerators;

        private IFactory<ILevelObject>[] Ogenerators;

        private IEntity[][] entities;

        private ILevelObject[][] objects;

        public void Update(GameTime time)
        {
            if (!blockManager.Initialized)
            {
                int i = Rand.Next(0, terrains.Length);
                blockManager.SetTerrain(terrains[i]);
                blockManager.Initialize();
            }

            blockManager.Update(time);

            for (int i = 0; i < entities.Length; i++)
                if (entities[i] == null && !Actions.Gameover)
                    entities[i] = Egenerators[i].Generate();

            for (int j = 0; j < objects.Length; j++)
            {
                if (objects[j] == null && !Actions.Gameover)
                    objects[j] = Ogenerators[j].Generate();
            }

            bool spawnmobs = true;
            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i] != null)
                {
                    for (int j = 0; j < entities[i].Length; j++)
                    {
                        entities[i][j].Update(time);
                        entities[i][j].ShiftX(-moveSpeed * Settings.gameSpeed);
                        if (!entities[i][j].Dead())
                            spawnmobs = false;
                    }
                }
            }

            for (int j = 0; j < objects.Length; j++)
            {
                if (objects[j] != null)
                {
                    for (int k = 0; k < objects[j].Length; k++)
                    {
                        objects[j][k].Update(time);
                        objects[j][k].ShiftX(-moveSpeed * Settings.gameSpeed);
                        if (!objects[j][k].Dead())
                            spawnmobs = false;
                    }
                }
            }

            if (spawnmobs)
            {
                if (!Actions.Gameover)
                {
                    for (int i = 0; i < entities.Length; i++)
                        entities[i] = Egenerators[i].Generate();
                    for (int j = 0; j < objects.Length; j++)
                        objects[j] = Ogenerators[j].Generate();
                    waveNumber++;
                }
                else
                {
                    for (int i = 0; i < entities.Length; i++)
                        entities[i] = null;
                    for (int j = 0; j < objects.Length; j++)
                        objects[j] = null;
                }

            }

            if (blockManager.CurrentTerrain.Ending)
            {
                int i = Rand.Next(0, terrains.Length);
                blockManager.SetTerrain(terrains[i]);
            }
        }

        public void UpdateWindow(Rectangle newWindow)
        {
            blockManager.UpdateWindow(newWindow);
            for (int a = 0; a < Ogenerators.Length; a++)
                Ogenerators[a].UpdateWindow(newWindow);
            for (int b = 0; b < Egenerators.Length; b++)
                Egenerators[b].UpdateWindow(newWindow);
            for (int i = 0; i < entities.Length; i++)
                if (entities[i] != null)
                    for (int j = 0; j < entities[i].Length; j++)
                        entities[i][j].UpdateWindow(newWindow);
            for (int j = 0; j < objects.Length; j++)
                if (objects[j] != null)
                    for (int k = 0; k < objects[j].Length; k++)
                        objects[j][k].UpdateWindow(newWindow);
        }

        public void Draw(SpriteBatch batch)
        {
            blockManager.Draw(batch);
            for (int i = 0; i < entities.Length; i++)
                if (entities[i] != null)
                    for (int j = 0; j < entities[i].Length; j++)
                        entities[i][j].Draw(batch);
            for (int j = 0; j < objects.Length; j++)
                if (objects[j] != null)
                    for (int k = 0; k < objects[j].Length; k++)
                        objects[j][k].Draw(batch);
        }

        public void Reset()
        {
            blockManager.Reset();
            for (int i = 0; i < entities.Length; i++)
                entities[i] = null; //Dont't call reset, delete them
            for (int j = 0; j < objects.Length; j++)
                objects[j] = null; //Dont't call reset, delete them
        }

        /// <summary>
        /// Initialize the level with factories and terrains
        /// </summary>
        /// <param name="Ogenerators">Level object factories</param>
        /// <param name="Egenerators">Level entity factories</param>
        /// <param name="terrains">Different terrains for level</param>
        public void Init(IFactory<ILevelObject>[] Ogenerators, IFactory<IEntity>[] Egenerators, ITerrain[] terrains)
        {
            this.Ogenerators = Ogenerators;
            this.Egenerators = Egenerators;
            this.objects = new ILevelObject[Ogenerators.Length][];
            this.entities = new IEntity[Egenerators.Length][];
            this.terrains = terrains;
        }

        /// <summary>
        /// Creates a level
        /// </summary>
        /// <param name="blockManager">Block manager for creating blokcs</param>
        /// <param name="window">Window rectangle of the game</param>
        public Level(BlockManager blockManager, Rectangle window)
        {
            this.blockManager = blockManager;
            this.window = window;
        }
    }
}
