//Imports necessary libraries and namespaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using YatSprint.Factories;
using YatSprint.Global;
using YatSprint.Managers;
using YatSprint.Objects;
using YatSprint.UI;
using YatSprint.Objects.Terrains;
using YatSprint.Objects.Blocks;
using System.IO;
using System.Reflection;

namespace YatSprint
{
    //DOING:
    //
    // -Pause Screen
    //
    //TODO:
    //
    // -change Fullscreen to Options, and put fullscreen and other stuff in that screen
    // -Add scrollbar class to put in Options screen, inherits button?
    // -Organize code by adding to IOBject loadTextures()? or something that uses content manager
    //
    //IDEAS:
    //
    // - Tunnels
    // - Rat traps
    // - Replace coins with cheese
    // - 
    //

    /// <summary>
    /// YatSprint Game Component for XNA 4.0
    /// </summary>
    public class YatSprintGame : Game
    {
        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Texture2D[] yatRunning = new Texture2D[4];
        private Texture2D[] yatSmashing = new Texture2D[4];
        private Texture2D[] yatClimbing = new Texture2D[4];
        private Texture2D[] yatFalling = new Texture2D[2];
        private Texture2D skyTex,
            ground1Tex,
            ground2Tex,
            ground3Tex,
            ladder,
            coin,
            soundLoud,
            soundMute,
            mushroomSheet,
            fireballSheet,
            birdSheet,
            tint,
            cage,
            healthBar,
            xpOrb,
            xpBar,
            potionGreen,
            potionYellow,
            potionRed,
            potionBlue,
            pauseButton,
            playButton;
        private SpriteFont button14, 
            button36, 
            button72;
        private Song mainmenuSong,
            gameplaySong;
        private SoundEffect coinEffect, 
            flameEffect,
            squashEffect,
            hawkEffect,
            xpEffect,
            snapEffect,
            potionEffect;
        private Display display;
        private Screen titleScreen, 
            gameplay,
            gameover,
            paused;
        private Yat yat; 
        private Sky sky;
        private Level level;
        private Button  gameoverLabel,
            mainMenuLabel;
        private ImageButton soundButton,
            playPauseButton;
        private Progressbar healthbar,
            experience;
        private Clock clock;
        private Score score;
        private FPSCounter fpsCounter;
        private Thread loadThread;
        private string loadMessage;
        private string KeysPressed = string.Empty;
        private string[] Messages = new string[]
        {
            "Just like Minecraft!",
            "Not copyrighted!",
            "Lame puns!",
            "Don't rage, Seriously.",
            "So very original!",
            "Clean UI!",
            "Inspired by a 6-Year old kid!",
            "Play the game already.",
            "LAgGgGgGgGg... so real.",
            "Almost over 9000...FPS!",
            "Compiled with 64-bit love!",
            "Why are you reaading these?",
            "You better hurry.",
            "What are you running from???!",
            "Sesquipedality specialtists!",
            "With a font size 16!",
            "Who cares.",
            "Some random texts!",
            "A _____ of time?",
            "Vertaald, to french!",
            "Good texture!",
            "Powered by Yats!"
        };

        public YatSprintGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        private bool AllowEditing(string password)
        {
            Keys[] input = Keyboard.GetState().GetPressedKeys();
            if (input.Length > 0)
            {
                char current = (char)input[0];
                if (KeysPressed.Length > 0)
                {
                    if ((char)KeysPressed[KeysPressed.Length - 1] != current)
                        KeysPressed += current;
                }
                else
                    KeysPressed += current;              
            }
            if (KeysPressed.Contains(password.ToUpper()))
            {
                KeysPressed = string.Empty;
                return true;
            }
            return false;
        }

        private Texture2D MonoLoadTexture2D(string path)
        {
            string curDir = Environment.CurrentDirectory;
            using (FileStream fs = File.Open(Path.Combine(curDir, "Content", string.Concat(path, ".png")), FileMode.Open))
            {
                return Texture2D.FromStream(GraphicsDevice, fs);
            }
        }

        private Song MonoLoadSong(string path)
        {
            var ctor = typeof(Song).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance, null,
                new[] { typeof(string), typeof(string), typeof(int) }, null);

            string filename = Path.GetFileNameWithoutExtension(path);
            return (Song)ctor.Invoke(new object[] { filename, path, 0 });
        }

        private void Load()
        {
#if DEBUG
            Debugger.Log(1, "Main", "Started loading in background...\n");
            Debugger.Log(1, "Main", "Started loading sounds...\n");
#endif 
            loadMessage = "Loading sounds...";
            coinEffect = Content.Load<SoundEffect>("sounds//coin");
            flameEffect = Content.Load<SoundEffect>("sounds//flame");
            squashEffect = Content.Load<SoundEffect>("sounds//squash");
            hawkEffect = Content.Load<SoundEffect>("sounds//hawk");
            xpEffect = Content.Load<SoundEffect>("sounds//xp");
            snapEffect = Content.Load<SoundEffect>("sounds//snap");
            potionEffect = Content.Load<SoundEffect>("sounds//potionUse");

            mainmenuSong = Content.Load<Song>("songs//mainmenuSong");
            gameplaySong = Content.Load<Song>("songs//gameplaySong");
#if DEBUG
            Debugger.Log(1, "Main", "Started loading fonts...\n");
#endif
            loadMessage = "Loading fonts...";
            button14 = Content.Load<SpriteFont>("fonts//button14");
            button72 = Content.Load<SpriteFont>("fonts//button72");
#if DEBUG
            Debugger.Log(1, "Main", "Started loading textures...\n");
#endif 
            loadMessage = "Loading textures...";
            yatClimbing[0] = MonoLoadTexture2D("yatSprites//yatClimb//yatClimbing1");
            yatClimbing[1] = MonoLoadTexture2D("yatSprites//yatClimb//yatClimbing2");
            yatClimbing[2] = MonoLoadTexture2D("yatSprites//yatClimb//yatClimbing3");
            yatClimbing[3] = MonoLoadTexture2D("yatSprites//yatClimb//yatClimbing4");
            yatFalling[0] = MonoLoadTexture2D("yatSprites//yatFall//yatFalling1");
            yatFalling[1] = MonoLoadTexture2D("yatSprites//yatFall//yatFalling2");
            yatRunning[0] = MonoLoadTexture2D("yatSprites//yatRun//yatRunning1");
            yatRunning[1] = MonoLoadTexture2D("yatSprites//yatRun//yatRunning2");
            yatRunning[2] = MonoLoadTexture2D("yatSprites//yatRun//yatRunning3");
            yatRunning[3] = MonoLoadTexture2D("yatSprites//yatRun//yatRunning4");
            yatSmashing[0] = MonoLoadTexture2D("yatSprites//yatSmash//yatSmash1");
            yatSmashing[1] = MonoLoadTexture2D("yatSprites//yatSmash//yatSmash2");
            yatSmashing[2] = MonoLoadTexture2D("yatSprites//yatSmash//yatSmash3");
            yatSmashing[3] = MonoLoadTexture2D("yatSprites//yatSmash//yatSmash4");
            skyTex = MonoLoadTexture2D("background//sky");
            ground1Tex = MonoLoadTexture2D("background//ground1");
            ground2Tex = MonoLoadTexture2D("background//ground2");
            ground3Tex = MonoLoadTexture2D("background//ground3");
            ladder = MonoLoadTexture2D("background//Ladder");
            coin = MonoLoadTexture2D("coin");
            soundLoud = MonoLoadTexture2D("UI//soundLoud");
            soundMute = MonoLoadTexture2D("UI//soundMute");
            mushroomSheet = MonoLoadTexture2D("mushroomSprites//mushrooms");
            fireballSheet = MonoLoadTexture2D("Fireballs");
            birdSheet = MonoLoadTexture2D("bird");
            tint = MonoLoadTexture2D("tint");
            cage = MonoLoadTexture2D("UI//Cage");
            healthBar = MonoLoadTexture2D("UI//Healthbar");
            xpBar = MonoLoadTexture2D("UI//Xpbar");
            xpOrb = MonoLoadTexture2D("xp");
            potionGreen = MonoLoadTexture2D("potions//potion1");
            potionYellow = MonoLoadTexture2D("potions//potion2");
            potionRed = MonoLoadTexture2D("potions//potion3");
            potionBlue = MonoLoadTexture2D("potions//potion4");
            pauseButton = MonoLoadTexture2D("UI//Pause");
            playButton = MonoLoadTexture2D("UI//Play");
#if DEBUG
            Debugger.Log(1, "Main", "Finished loading content.\n");
#endif
            MediaPlayer.IsRepeating = true;
            Create();
        }

        private void Create()
        {
#if DEBUG
            Debugger.Log(1, "Main", "Started initialization phase.\n");
#endif
            Rectangle window = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            sky = new Sky(skyTex, window);
            yat = new Yat(yatClimbing, yatRunning, yatSmashing, yatFalling, snapEffect, window);
            BlockManager blockManager = new BlockManager(ground3Tex, ground2Tex, ground1Tex, ladder, window);
            level = new Level(blockManager, window);
            experience = new Experiencebar(yat, cage, xpBar, button14, new Vector2(50, window.Height - 50), new Resize(DockType.LowerLeft, new Vector2(cage.Width, cage.Height), new Vector2(25)), 0, 100, AmountType.AmountAndTotal);
            XPManager xpManager = new XPManager(yat, experience, xpOrb, xpEffect);
            MushroomFactory mgen = new MushroomFactory(mushroomSheet, fireballSheet, Mushroom.Size, level, yat, flameEffect, squashEffect, xpManager, window);
            CoinFactory cgen = new CoinFactory(level, yat, coin, coinEffect, window);
            BirdFactory bgen = new BirdFactory(level, yat, birdSheet, hawkEffect, xpManager, window);
            PotionManager pmanager = new PotionManager(yat, (Experiencebar)experience, new Texture2D[] { potionRed, potionGreen, potionBlue, potionYellow }, potionEffect, 20);
            yat.Init(level, pmanager);
            Plains hillzone = new Plains(ground3Tex, ground2Tex, ground1Tex, ladder, 2, 7, Rand.Next(10, 20), window);
            level.Init(new IFactory<ILevelObject>[] { cgen }, new IFactory<IEntity>[] { mgen, bgen }, new ITerrain[] { hillzone });
            fpsCounter = new FPSCounter(new Vector2(10), new Resize(new Vector2(10)), window, button14, Color.Black);
            score = new Score(new Vector2(0, 10), new Resize(new Vector2(2, 0), new Vector2(0, 35)), window, button36, Color.Black);
            gameoverLabel = new Button(new Vector2(0, window.Height / 3), new Resize(new Vector2(2, 3), new Vector2(0, 0)), window, "Game over", button72, true, Color.Black, Color.Black, false, null, tint);
            mainMenuLabel = new Button(new Vector2(0, window.Height / 3 + 100), new Resize(new Vector2(2, 3), new Vector2(0, 100)), window, "Main menu", button36, true, Color.Black, Color.Red, true, new Request(0, 2,
                delegate()
                {
                    Actions.Gameover = false;
                    Score.Points = 0;
                }));
            soundButton = new ImageButton(new Vector2(window.Width - 50, 20), new Resize(DockType.UpperRight, new Vector2(soundLoud.Width, soundLoud.Height), new Vector2(50, 20)),  new Action[] { 
            delegate() 
            {
                Settings.PlaySounds = false;
                MediaPlayer.IsMuted = true;
            },
            delegate()
            {
                Settings.PlaySounds = true;
                MediaPlayer.IsMuted = false;
            }},
            window,
            null,
            false,
            soundLoud, 
            soundMute);

            playPauseButton = new ImageButton(new Vector2(window.Width - 120, 20), new Resize(DockType.UpperRight, new Vector2(pauseButton.Width, pauseButton.Height), new Vector2(120, 20)), new Request[] {
                new Request(1, 2),
                new Request(3, 2)}, 
                window, 
                'P',
                true,
                pauseButton, 
                playButton);
            healthbar = new Healthbar(yat, cage, healthBar, button14, new Vector2(window.Width - 200, window.Height - 50), new Resize(DockType.LowerRight, new Vector2(cage.Width, cage.Height), new Vector2(25)), Yat.health, Yat.health, AmountType.Percentage);
            clock = new Clock(button36, new Vector2(window.Width / 2, 75), new Resize(new Vector2(2, 0), new Vector2(0, 75)), Color.Black, window);
            gameplay = new Screen(1, new IButton[] { fpsCounter, score, clock, soundButton, playPauseButton, healthbar, experience }, new IObject[] { sky, level, yat }, gameplaySong, true);
            Button title = new Button(new Vector2(0, window.Height / 3), new Resize(new Vector2(2, 3), new Vector2(0)), window, "Yat Sprint", button72, true, Color.Black, Color.DimGray, false, null);
            Button play = new Button(new Vector2(0, window.Height / 3 + 100), new Resize(new Vector2(2, 3), new Vector2(0, 100)), window, "Play", button36, true, Color.Black, Color.DimGray, true, new Request(1, 1));
            Button fullscreen = new Button(new Vector2(0, window.Height / 3 + 150), new Resize(new Vector2(2, 3), new Vector2(0, 150)), window, "Fullscreen", button36, true, Color.Black, Color.DimGray, true, new Request(-1, 1, delegate()
            {
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = 800;
                    graphics.PreferredBackBufferHeight = 600;
                    graphics.ApplyChanges();
                }
                else
                {
                    graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    graphics.ApplyChanges();
                }
                display.UpdateWindow(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
                Actions.ToggleFullscreen = true;
            }));
            Button exit = new Button(new Vector2(0, window.Height / 3 + 200), new Resize(new Vector2(2, 3), new Vector2(0, 200)), window, "Exit", button36, true, Color.Black, Color.DimGray, true, new Request(-1, 1, delegate()
            {
                Actions.Exitgame = true;
            }));

            Button motd = new Button(new Vector2(window.Width / 2 + 250, window.Width / 3 + 10), new Resize(new Vector2(2, 3), new Vector2(250, 25)), window, Messages[Rand.Next(Messages.Length)], button14, false, Color.Black, Color.Black, false, null);
            motd.SetRotation(Rand.Next(-45, -20));
            titleScreen = new Screen(0, new IButton[] { fpsCounter, soundButton, title, motd, play, fullscreen, exit }, new IObject[] { sky }, mainmenuSong, true);
            gameover = new Screen(2, new IButton[] { fpsCounter, score, soundButton, gameoverLabel, mainMenuLabel }, new IObject[] { sky }, gameplaySong, true);
            Button pauseTitle = new Button(new Vector2(0, window.Height / 3), new Resize(new Vector2(2, 3), new Vector2(0)), window, "Paused", button72, true, Color.Black, Color.Black, false, null, tint);
            Button resumeButton = new Button(new Vector2(0, window.Height / 3 + 100), new Resize(new Vector2(2, 3), new Vector2(0, 100)), window, "Resume", button36, true, Color.Black, Color.Gray, true, new Request(1, 2));
            Button mainMenuPause = new Button(new Vector2(0, window.Height / 3 + 150), new Resize(new Vector2(2, 3), new Vector2(0, 150)), window, "Main Menu", button36, true, Color.Black, Color.Gray, true, new Request(0, 2));
            paused = new Screen(3, new IButton[] { fpsCounter, score, soundButton, playPauseButton, pauseTitle, resumeButton, mainMenuPause }, new IObject[] { sky }, gameplaySong, false); 
            display = new Display();
            display.Add(gameplay);
            display.Add(titleScreen);
            display.Add(gameover);
            display.Add(paused);

            MediaPlayer.Play(mainmenuSong);

#if DEBUG
            Debugger.Log(1, "Main", "Finished initialization phase.\n");
#endif
        }

        protected override void Initialize()
        {
            //Allow mouse to be visible
            this.IsMouseVisible = true;

            //Set mediaplayer volume
            //MediaPlayer.Volume = Settings.Volume;

            //Set mediaplayer to repeat mode
            //MediaPlayer.IsRepeating = true;

            //Create a thread for the content to be loaded in the background
            loadThread = new Thread(() => Load());
            loadThread.Name = "Background_Loader";

#if DEBUG
            if (!Debugger.IsLogging())
                Debugger.Launch();
            Debugger.Log(1, "Main", "Attached debugger to process.\n");
#endif

            //Continue to loading content
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region RequiredContentToLoadFirst

            //Loads biggest font for loading screen
            button36 = Content.Load<SpriteFont>("fonts//button36");

            #endregion

            loadThread.Start();
        }

        protected override void Update(GameTime gameTime)
        {
            //Exits game if escape key is pressed at any time
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
           
#if DEBUG
            float increment = 0.01F;
            if (Keyboard.GetState().IsKeyDown(Keys.Add))
                Settings.gameSpeed += increment;
            else if (Keyboard.GetState().IsKeyDown(Keys.Subtract) && Settings.gameSpeed > increment)
                Settings.gameSpeed -= increment;
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                Settings.gameSpeed = 1.0F;
            else if (AllowEditing("edit"))
                Debugger.Break();
#endif
            //Only draw once all objects have been loaded
            if (!loadThread.IsAlive)
            {
                //Update the display
                display.Update(this, gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            if (loadThread.IsAlive)
            {
                Rectangle window = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                string message = loadMessage ?? "Loading...";
                Vector2 size = button36.MeasureString(message);
                spriteBatch.DrawString(button36, message, new Vector2((window.Width / 2) - (size.X / 2), (window.Height / 2) - (size.Y / 2)), Color.White);
            }
            else
            {
                display.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}