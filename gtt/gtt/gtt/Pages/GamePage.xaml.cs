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

        public UIElementRenderer elementRenderer;

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

            LayoutUpdated += new EventHandler(GamePage_LayoutUpdated);
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
        /// UpdateLayout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GamePage_LayoutUpdated(object sender, EventArgs e)
        {
            if ((ActualWidth > 0) && (ActualHeight > 0))
            {
                SharedGraphicsDeviceManager.Current.PreferredBackBufferWidth = (int)ActualWidth;
                SharedGraphicsDeviceManager.Current.PreferredBackBufferHeight = (int)ActualHeight;
            }

            if (null == elementRenderer)
            {
                elementRenderer = new UIElementRenderer(this, (int)ActualWidth, (int)ActualHeight);
            }


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
            // REnderowanie UI, jako tekstury xna
            elementRenderer.Render();



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

               // Draws user hud
               DrawHud();

               // Tutaj rysujemy interfejs silverlightowy, ale zamieniony/przerenderowany na 
               // Xna texture.
               spriteBatch.Draw(elementRenderer.Texture, Vector2.Zero, Color.White);
                


            spriteBatch.End();


            /*
             * 
             * Renderowanie UI w grze
             * */



            // Przygotowanie do rysowania w klasie Game
            //ar gt = new GameTime(e.TotalTime, e.ElapsedTime);
            //game.Draw(gt);
            

        }

        /// <summary>
        /// Funkcja rysująca interfejs uzytkownika w grze
        /// </summary>
        private void DrawHud()
        {
            ScoreText.Text = "Score:";
            scoreAmount.Text = "0";
            LevelText.Text = "Level:";
            LevelAmount.Text = "1";
        }
    }
}