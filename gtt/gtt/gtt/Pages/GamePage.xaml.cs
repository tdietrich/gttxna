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
using gtt.MainC;

namespace gtt
{
    public partial class GamePage : PhoneApplicationPage
    {
        private ContentManager contentManager;
  
        private GameTimer timer;
        private SpriteBatch spriteBatch;

        /// <summary>
        ///  Klasa Wrapper ogólna dla gry - tu jest wszystko
        /// </summary>
        public GameC game;

        public GamePage()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            //game1 = new Game1();

            game = new GameC();
            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
        }

        /// <summary>
        /// Inicjalizacja
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            
            // Odpalenie gry
           // game1.Run();
            game.Initialize();

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
            GameTime gt = new GameTime(e.TotalTime, e.ElapsedTime);
            game.Update(gt);


        }

        /// <summary>
        /// Allows the page to draw itself.
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            //GameTime gt = new GameTime(e.TotalTime,e.ElapsedTime);
            //game.Draw(gt);
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            /*
             * RYSOWANIE
             */
           spriteBatch.Begin();
                
                var projection = Matrix.CreateOrthographicOffCenter(0f,
                    ConvertUnits.ToSimUnits(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width),
                    ConvertUnits.ToSimUnits(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height), 0f, 0f,
                    1f);

               game.debugView.RenderDebugData(ref projection);
               
            spriteBatch.End();



            // Przygotowanie do rysowania w klasie Game
            var gt = new GameTime(e.TotalTime, e.ElapsedTime);
            //game.Draw(gt);
            

        }
    }
}