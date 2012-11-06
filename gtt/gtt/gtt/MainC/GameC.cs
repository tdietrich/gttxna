using System;
using System.Linq;
using System.Windows;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using FarseerPhysics.Factories;

namespace gtt.MainC
{
    /// <summary>
    /// Klasa, zarządza całą grą, światem fizycznym itp.
    /// 
    /// autor: Tomasz Dietrich
    /// </summary>
    public class GameC
    {
        # region Fields

        /// <summary>
        /// Świat fizyczny
        /// </summary>
        public World world;

        /// <summary>
        /// Grafika
        /// </summary>
        public GraphicsDevice graphicDevice;

        /// <summary>
        /// Manager tresci - do ladowania np tekstur itp
        /// </summary>
        public ContentManager contentManager;

        /// <summary>
        /// Tekstury temporalne
        /// </summary>
        private Texture2D tex;
        private Texture2D tex2;
        private Texture2D tex3;

        /// <summary>
        /// Widok
        /// </summary>
        public DebugViewXNA debugView;

        /// <summary>
        /// Ciało - podłoga
        /// </summary>
        private Body _floor;

        /// <summary>
        /// Ciało platforma
        /// </summary>
        private Body _platform;

        /// <summary>
        /// Sprite Podłogi
        /// </summary>
        private Sprite _floorS;

        /// <summary>
        /// Sprite platformy
        /// </summary>
        private Sprite _platformS;

        /// <summary>
        /// Licznik do 'wypluwania' klocków
        /// </summary>
        private float counter;

        private SpriteBatch spriteBatch;

        /// <summary>
        /// Struktura trzymające settingsy gry
        /// </summary>
        public static GameSettings Settings;

        #endregion

        #region Methods
       
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game"></param>
        public GameC()
        {
            // Ustawienie defaultowych danych, w calej grze dane są z tej zmiennej brane
            Settings = new GameSettings(0.5f, 0.0f,0.2f);
        }

        /// <summary>
        /// Metoda  wywolywana przed rysowaniem
        /// </summary>
        public void Initialize()
        {

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // Inicjalizacja świata
            if (world == null)
            {
                world = new World(Vector2.UnitY * 3);
            }
            else
            {
                world.Clear();
            }

            if (debugView == null)
            {
                debugView = new DebugViewXNA(world);
                debugView.RemoveFlags(FarseerPhysics.DebugViewFlags.Controllers);
                debugView.RemoveFlags(FarseerPhysics.DebugViewFlags.Joint);

                debugView.LoadContent(SharedGraphicsDeviceManager.Current.GraphicsDevice, contentManager);
            }

            // Tworzenie podlogi
            _floor = BodyFactory.CreateRectangle(world,
                                                ConvertUnits.ToSimUnits(480),
                                                ConvertUnits.ToSimUnits(50),
                                                10f);
            _floor.Position = ConvertUnits.ToSimUnits(240, 775);
            _floor.BodyType = BodyType.Static;
            _floor.IsStatic = true;
            _floor.Restitution = 0.2f;
            _floor.Friction = 0.2f;

            // Tworzenie platformy
            _platform = BodyFactory.CreateRectangle(world,
                                                ConvertUnits.ToSimUnits(250),
                                                ConvertUnits.ToSimUnits(50),
                                                10f);
            _platform.Position = ConvertUnits.ToSimUnits(240, 725);
            _platform.BodyType = BodyType.Static;
            _platform.IsStatic = true;
            _platform.Restitution = 0.2f;
            _platform.Friction = 1.0f;

            
            LoadContent();


        }

        /// <summary>
        /// Tu Load contentu grafizcnego etc
        /// </summary>
        protected void LoadContent()
        {
            // TODO: use this.content to load your game content here
            // Tymczasowe textury, syf i mogiła, na razie nie używane, ale nie wywalać.
            tex = contentManager.Load<Texture2D>("ApplicationIcon");
            tex2 = contentManager.Load<Texture2D>("asdawdas");
            tex3 = contentManager.Load<Texture2D>("floor");

            //OurBlock = new Block(BLOCKTYPES.Z_SHAPE, ref world, tex);
            _floorS = new Sprite(tex3);
            _platformS = new Sprite(tex2);

        }

        /// <summary>
        /// Update logiki itp
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= 2.0f)
            {
                counter = 0f;

                // Losowanie randomowego typu bloku
                var random = new Random();
                var type = random.Next(1,7);

                // Losowanie randomowego obrotu klocka, od 0 do 360
                float rot = (float)random.Next(0,360);

                // Stworzenie bloku = jednoznacze z dodaniem go do świata
                var block = new Block(ref world, tex, (BLOCKTYPES)type, rot);

            }
            //world.Step(0.5f);
            world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 2f)));

            foreach (var box in from box in world.BodyList
                                let pos = ConvertUnits.ToDisplayUnits(box.Position)
                                where pos.Y > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height
                                select box)
            {
                world.RemoveBody(box);
            }

        }
        
        /// <summary>
        /// Rysowanie, jeszcze burdel tu jest na razie i tak rysowanie w klasie GAmePage
        /// </summary>
        /// <param name="gameTime"></param>
        public  void Draw(GameTime gameTime)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

                spriteBatch.Draw(_floorS.Texture, ConvertUnits.ToDisplayUnits(_floor.Position), null,
                                               Color.White, _floor.Rotation,
                                               _floorS.Origin, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(_platformS.Texture, ConvertUnits.ToDisplayUnits(_platform.Position), null,
                                           Color.White, _platform.Rotation,
                                           _platformS.Origin, 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(tex, ConvertUnits.ToDisplayUnits(rectangles.Position), null,
            //                           Color.White, rectangles.Rotation,
            //                           rectangleSprite.Origin + offset, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();

            //OurBlock.Draw(gameTime);
            // Rysowanie odbywa się w klasie GamePage.xaml.cs wlasciwie
        }

        #endregion

    }


    #region Game Setting Struct

    /// <summary>
    /// Struktura trzymająca dane ustawien gry
    /// </summary>
    public struct GameSettings
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="friction">tarcie klocków</param>
        /// <param name="restitution">odbijalnosc klockos</param>
        public GameSettings(float friction,float restitution,float _blockSize)
        {
            blocksFriction = friction;
            blocksRestitution = restitution;
            blockSize = _blockSize;
        }
        public float blocksFriction;
        public float blocksRestitution;
        public float blockSize;
    }

    #endregion
}
