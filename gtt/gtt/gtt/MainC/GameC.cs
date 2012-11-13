﻿using System;
using System.Linq;
using System.Windows;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using FarseerPhysics.Factories;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace gtt.MainC
{
    /// <summary>
    /// Klasa, zarządza całą grą, światem fizycznym itp.
    /// 
    /// autor: Tomasz Dietrich, Michał Czwarnowski
    /// 
    /// TODO:
    ///     - Blok jeszcze nie dotknął innego - kolizje puste a juz leci następny...?
    ///     
    /// 
    /// 
    /// W tej chwili opadający klocek znajduje się w zmiennej CurrentBlock
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
        protected Texture2D tex;
        protected Texture2D tex2;
        protected Texture2D tex3;

        /// <summary>
        /// Widok
        /// </summary>
        public DebugViewXNA debugView;

        /// <summary>
        /// Ciało - podłoga
        /// </summary>
        protected Body _floor;

        /// <summary>
        /// Ciało platforma
        /// </summary>
        protected Body _platform;

        /// <summary>
        /// Sprite Podłogi
        /// </summary>
        protected Sprite _floorS;

        /// <summary>
        /// Sprite platformy
        /// </summary>
        protected Sprite _platformS;

        /// <summary>
        /// Licznik do 'wypluwania' klocków
        /// </summary>
        protected float counter;

        private SpriteBatch spriteBatch;

        /// <summary>
        /// Struktura trzymające settingsy gry
        /// </summary>
        public static GameSettings Settings;

        /// <summary>
        /// Zmienna mówiąca czy blok wylosowany 'leci', czy został postawiony.
        /// </summary>
        protected bool blockOnHisWay;

        /// <summary>
        /// Blok który w tym momencie spada.
        /// </summary>
        protected Block CurrentBlock;

        /// <summary>
        /// Linie pokazujące wysokość wieży
        /// </summary>
        public List<LevelLine> LevelLines;
        #endregion

        #region Methods
       
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="game"></param>
        public GameC()
        {
            // Ustawienie defaultowych danych, w calej grze dane są z tej zmiennej brane
            Settings = new GameSettings(3.0f, 0.0f, 0.2f, Vector2.UnitY,
                                        new Vector2(SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Width/ 2,40));

        }

        /// <summary>
        /// Metoda  wywolywana przed rysowaniem
        /// </summary>
        public void Initialize()
        {

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            LevelLines = new List<LevelLine>();

            // Dodanie linii levelu.
            LevelLines.Add(new LevelLine(250, SharedGraphicsDeviceManager.Current.GraphicsDevice));
            LevelLines.Add(new LevelLine(500, SharedGraphicsDeviceManager.Current.GraphicsDevice));



            // Inicjalizacja świata
            if (world == null)
            {
                world = new World(Settings.gravity);
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
            _floor.Restitution = 0.0f;
            _floor.Friction = 2.5f;

            // Tworzenie platformy
            _platform = BodyFactory.CreateRectangle(world,
                                                ConvertUnits.ToSimUnits(250),
                                                ConvertUnits.ToSimUnits(50),
                                                10f);
            _platform.Position = ConvertUnits.ToSimUnits(240, 725);
            _platform.BodyType = BodyType.Static;
            _platform.IsStatic = true;
            _platform.Restitution = 0.0f;
            _platform.Friction = 3.0f;

            // Żaden blok nie spada
            blockOnHisWay = false;

            //Aktywacja gestów - hold do obracania i horizontaldrag do ruszania klockiem
            TouchPanel.EnabledGestures = GestureType.HorizontalDrag | GestureType.VerticalDrag;
            
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
        /// Update logiki, sterowanie itp
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Jeżeli poprzedni blok został usytuowany
            if (!blockOnHisWay)
            {
                // Losowanie randomowego typu bloku
                var random = new Random();
                var type = random.Next(1,7);

                // Losowanie randomowego obrotu klocka, od 0 do 360
                float rot = (float)random.Next(0,360);
                
                // Stworzenie bloku = jednoznacze z dodaniem go do świata
                CurrentBlock = new Block(ref world, tex, (BLOCKTYPES)type, rot);

                blockOnHisWay = true;
            }
            
            // Aktualizacja fizyki
            //world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds*0.001f);
              world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 3f)));


            // Usunięcie Boxów które wypadły za świat
            foreach (var box in from box in world.BodyList
                                let pos = ConvertUnits.ToDisplayUnits(box.Position)
                                where pos.Y > SharedGraphicsDeviceManager.Current.GraphicsDevice.Viewport.Height
                                select box)
            {
                world.RemoveBody(box);
            }

            // Sprawdzanie kolizji spadającego klocka
            UpdateBlockCollision();

            //Pętla odczytujaca gesty z ekranu
            while (TouchPanel.IsGestureAvailable)
            {
                //odczyt gestu z ekranu
                GestureSample gesture = TouchPanel.ReadGesture();

                //switch sprawdzający który z gestów z Initialize jest wykonywany
                switch (gesture.GestureType)
                {
                    case GestureType.VerticalDrag:
                        if (CurrentBlock != null) CurrentBlock.myBody.Rotation += 0.01f * gesture.Delta.Y;
                        break;
                    case GestureType.HorizontalDrag:
                        if (CurrentBlock != null) CurrentBlock.myBody.Position += 0.01f * gesture.Delta;
                        break;
                }

            }

        }


        /// <summary>
        /// Sprawdzanie kolizji spadającego klocka
        /// </summary>
        private void UpdateBlockCollision()
        {
            // Jeżeli nastąpił kontakt spadającego klocka z jakimś
            // Innym ciałem, zmien flagę, że można puścić następny kloc.

            // TODO: Ogarnąć czy to jest dokladny sposob bo chyba nie.
            if (CurrentBlock.myBody.ContactList != null)
            {
                // Zmien flage.
                blockOnHisWay = false;

                // Zapomnij tego klocka jako currenta.
                CurrentBlock = null;
            }
        }
        
        /// <summary>
        /// Rysowanie, jeszcze burdel tu jest na razie i tak rysowanie w klasie GAmePage
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
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
        /// <param name="_blockSize">Rozmair klocka</param>
        /// <param name="_blocksSpawnInterval">Odstęp między spawnowaniem następnych klocków</param>
        /// <param name="_gravity">Grawitacja swiata</param>
        public GameSettings(float friction,float restitution,float _blockSize,Vector2 _gravity,
                            Vector2 _spawnPoint)
        {
            blocksFriction = friction;
            blocksRestitution = restitution;
            blockSize = _blockSize;
            gravity = _gravity;
            spawnPoint = _spawnPoint;
        }



        public float blocksFriction;
        public float blocksRestitution;
        public float blockSize;
        public Vector2 gravity;
        public Vector2 spawnPoint;
        
    }

    #endregion
}
