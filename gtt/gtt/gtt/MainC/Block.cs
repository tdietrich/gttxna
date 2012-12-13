﻿using System.Windows.Controls;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace gtt.MainC
{
    /// <summary>
    /// Klasa przedstawiające tetrisowy klocek.
    /// Używane w klasie też - Typy Bloków - <see cref="BlockTypesEnum"/> i struktura settings <see cref="Settings"/>
    ///
    /// 
    /// autor: Tomasz Dietrich
    /// 
    /// TODO:
    ///     - Jeżeli klocek się obraca to nie obraca się względem środka swojego, trzeba dostosować pozycje
    ///       Respawna w takim razie.
    /// </summary>
    /// 
    public class Block
    {

        #region Fields

        private Border border;
        private Sprite rectangleSprite;

        /// <summary>
        /// Fizyczne przedstawienie klocka
        /// </summary>
        public Body myBody { private set; get; }

        public bool heightChecked;

        private Vector2 offset;

        /// <summary>
        /// Początkowa rotacja klocka, klocek nie zawsze znajduje się w 'standardowym' polozeniu na poczatku.
        /// Aby dodać rotacje w czasie gry, dobrac się do myBody.
        /// </summary>
        public float initialRotation { private set; get; }

        /// <summary>
        /// 
        /// Referencja do świata fizycznego
        /// </summary>
        private World worldRef;

        /// <summary>
        /// SpriteBacth do rysowania
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Tekstura
        /// </summary>
        private Texture2D tex;

        /// <summary>
        /// Typ klocka - określany przez BlockTypesEnum
        /// </summary>
        private BLOCKTYPES type;

        #endregion Fields
        

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Typ klocka, pobrany z BlockTypesEnum</param>
        public Block(ref World _worldRef, Texture2D texture, BLOCKTYPES _type, float rotation)
        {
            // Przypisania
            type = _type;
            worldRef = _worldRef;
            tex = texture;

            // Stworzenie zmiennej do rysowania
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // Stworzenie nowego ciała
            myBody = new Body(worldRef);

            initialRotation= rotation;

            heightChecked = false;
            // Odpal funkcję inicjalizującą mnie
            InitializeBlock();
        }

        /// <summary>
        /// Funkcja inicjalizucje block w zaleznosci od typu podanego do konstruktora
        /// </summary>
        private void InitializeBlock()
        {
            // Funnkcja Tworzy cialo w zmiennej myBody, skladjace sie z wielu kwadratow w zaleznosci od typu tetrisowego klocka
            MakeMe(type);

            myBody.BodyType = BodyType.Dynamic;

            // Pozycja klocka pobrana ze struktury trzymającej ustawienia gry
            myBody.Position = ConvertUnits.ToSimUnits(GameC.Settings.spawnPoint);

            // Offset drugiego klocka
            offset = new Vector2(ConvertUnits.ToDisplayUnits(GameC.Settings.blockSize), 0f);

            // Rotacja klocka
            myBody.Rotation = initialRotation;
            myBody.Restitution = GameC.Settings.blocksRestitution;
            myBody.Friction = GameC.Settings.blocksFriction;
            myBody.Mass = 10;

            // Handler for collision
            myBody.OnCollision += new OnCollisionEventHandler(myBody_OnCollision);

        }

        /// <summary>
        /// W momencie gdy ciało skoliduje z jakims innym, ma prędkość ustawiona na 0, i zaczyna na niego wplywac grawitacja
        /// Takie rozwiązanie umożliwia ukladanie klocków jakies sensowne
        /// </summary>
        /// <param name="fixtureA"></param>
        /// <param name="fixtureB"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        bool myBody_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            myBody.IgnoreGravity = false;
            myBody.LinearVelocity = Vector2.Zero;
            return true;
        }



        /// <summary>
        /// Funkcja w zaleznosci od typu klocka tetrisowego, tworzy ciało skłądające się z kwadratów
        /// o odpowiednim ułożeniu
        /// </summary>
        /// <param name="type">Typ Bloku</param>
        private void MakeMe(BLOCKTYPES type)
        {
            // Lista wierzchołków kwadratów
            List<Vertices> rects = new List<Vertices>();
            
            
            for (int i = 1; i < 5; i++)
            {
                rects.Add(PolygonTools.CreateRectangle(GameC.Settings.blockSize, GameC.Settings.blockSize));
                
            }


            // Główny switch po typach
            switch (type)
             {
                 case BLOCKTYPES.I_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         rects[i-1].Translate(new Vector2(0, i *2*GameC.Settings.blockSize));
                     }

                     break;



                 case BLOCKTYPES.J_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 4)
                         {
                             rects[i - 1].Translate(new Vector2(-2 * GameC.Settings.blockSize, i * GameC.Settings.blockSize + (2 * GameC.Settings.blockSize)));
                         }
                         else
                             rects[i - 1].Translate(new Vector2(0, i * 2 * GameC.Settings.blockSize));
                         
                     }

                     



                     break;
                 case BLOCKTYPES.L_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 4)
                         {
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, i * GameC.Settings.blockSize + (2 * GameC.Settings.blockSize)));
                         }
                         else
                             rects[i - 1].Translate(new Vector2(0, i * 2 * GameC.Settings.blockSize));
                         
                     }




                     break;
                 case BLOCKTYPES.O_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, 0));

                         else if(i == 3)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.Settings.blockSize));  
                        
                         else if(i == 4)
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, 2 * GameC.Settings.blockSize));  

                     }
                     break;
                 case BLOCKTYPES.S_SHAPE:


                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, 0));

                         else if(i == 3)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.Settings.blockSize));  
                        
                         else if(i == 4)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.Settings.blockSize, 2 * GameC.Settings.blockSize));  

                     }

                     break;
                 case BLOCKTYPES.T_SHAPE:

                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.Settings.blockSize));

                         else if (i == 3)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.Settings.blockSize, 2 * GameC.Settings.blockSize));

                         else if (i == 4)
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, 2 * GameC.Settings.blockSize));

                     }

                     break;
                 case BLOCKTYPES.Z_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.Settings.blockSize));

                         else if (i == 3)
                             rects[i - 1].Translate(new Vector2(2 * GameC.Settings.blockSize, 2 * GameC.Settings.blockSize));

                         else if (i == 4)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.Settings.blockSize, 0));

                     }


                     break;
             }
            myBody = BodyFactory.CreateCompoundPolygon(worldRef, rects, 1.0f);
            
        }


        /// <summary>
        /// Rysuj
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                spriteBatch.Draw(tex, ConvertUnits.ToDisplayUnits(myBody.Position), null,
                                       Color.White, myBody.Rotation,
                                       rectangleSprite.Origin + offset, 1f, SpriteEffects.None, 0f);
                    // draw second rectangle
                spriteBatch.Draw(tex, ConvertUnits.ToDisplayUnits(myBody.Position), null, Color.White,
                                 myBody.Rotation, rectangleSprite.Origin - offset, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }


        #endregion Methods
    }
}
