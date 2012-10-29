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

        // Zmiene ponizej tylko na chwilę, ogarnąć strukturę
        private Texture2D tex;
        private Rectangle rect;
        public DebugViewXNA debugView;
        private Body _floor;
        private Body _platform;
        private float counter;

        #endregion

        #region Methods
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game"></param>
        public GameC()
        {

        }



        /// <summary>
        /// Metoda  wywolywana przed rysowaniem
        /// </summary>
        public void Initialize()
        {


            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;


            // Inicjalizacja świata
            if (world == null)
            {
                world = new World(Vector2.UnitY * 2);
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

            _floor = BodyFactory.CreateRectangle(world,
                                                ConvertUnits.ToSimUnits(480),
                                                ConvertUnits.ToSimUnits(50),
                                                10f);
            _floor.Position = ConvertUnits.ToSimUnits(240, 775);
            _floor.IsStatic = true;
            _floor.Restitution = 0.2f;
            _floor.Friction = 0.2f;

            _platform = BodyFactory.CreateRectangle(world,
                                                ConvertUnits.ToSimUnits(250),
                                                ConvertUnits.ToSimUnits(50),
                                                10f);
            _platform.Position = ConvertUnits.ToSimUnits(240, 725);
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
            tex = contentManager.Load<Texture2D>("ApplicationIcon");

        }

        /// <summary>
        /// Update logiki itp
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            counter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (counter >= 1.0f)
            {
                counter = 0f;

                var random = new Random();
                var width = random.Next(20, 100);
                var height = random.Next(20, 100);

                // Create it and store the size in the user data
                var box = BodyFactory.CreateRectangle(
                         world,
                         ConvertUnits.ToSimUnits(width),
                         ConvertUnits.ToSimUnits(height),
                         10f,
                         new Microsoft.Xna.Framework.Point(width, height));

                box.BodyType = BodyType.Dynamic;
                box.Inertia = 0.5f;
                box.Restitution = 0.0f;
                box.Friction = 0.2f;

                // Randomly pick a location along the top to drop it from
                box.Position = ConvertUnits.ToSimUnits(random.Next(50, 400), 0);




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
        /// Rysowanie
        /// </summary>
        /// <param name="gameTime"></param>
        public  void Draw(GameTime gameTime)
        {
         
            // Rysowanie odbywa się w klasie GamePage.xaml.cs wlasciwie
        }

        #endregion


    }
}
