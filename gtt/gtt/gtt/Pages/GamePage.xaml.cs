using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Controllers;
using FarseerPhysics.Common;

namespace gtt
{
    public partial class GamePage : PhoneApplicationPage
    {
        private ContentManager contentManager;
  
        private GameTimer timer;
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private World world;
        private Rectangle rect;
        private DebugViewXNA debugView;
        private Body _floor;
        private float counter;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: use this.content to load your game content here
            tex = contentManager.Load<Texture2D>("ApplicationIcon");
            
            // Inicjalizacja świata
            if (world == null)
            {
                world = new World(Vector2.UnitY*10);
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
                                                


            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Allows the page to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: Add your update logic here

            counter += (float)e.ElapsedTime.TotalSeconds;

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
                box.Restitution = 0.2f;
                box.Friction = 0.2f;
  
                // Randomly pick a location along the top to drop it from
                box.Position = ConvertUnits.ToSimUnits(random.Next(50, 400), 0);
                                                      



            }
            world.Step(Math.Min((float)e.ElapsedTime.TotalMilliseconds * 0.001f, (1f / 30f)));

            foreach(var box in from box in world.BodyList
                    let pos = ConvertUnits.ToDisplayUnits(box.Position)
                    where pos.Y >  SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height
                    select box)
            {
                world.RemoveBody(box);
            }

        }

        private void UpdateBlock(GameTimerEventArgs e)
        {
            
        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

         spriteBatch.Begin();
 
         var projection = Matrix.CreateOrthographicOffCenter(0f,
             ConvertUnits.ToSimUnits(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width),
             ConvertUnits.ToSimUnits(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height), 0f, 0f,
             1f);

         debugView.RenderDebugData(ref projection);
         spriteBatch.End();

        }
    }
}