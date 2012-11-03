using System.Windows.Controls;
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
    /// Używane w klasie też - <seealso cref="BlockTypesEnum"/>
    /// 
    /// autor: Tomasz Dietrich
    /// </summary>
    /// 
    public class Block
    {

        #region Fields

        private Border border;
        private Sprite rectangleSprite;
        private Body myBody;
        private Vector2 offset;

        /// <summary>
        /// Początkowa rotacja klocka, klocek nie zawsze znajduje się w 'standardowym' polozeniu na poczatku.
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
            myBody.Restitution = GameC.Settings.blocksRestitution;
            myBody.Friction = GameC.Settings.blocksFriction;
            initialRotation= rotation;
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
            var rand = new Random();
            int y = rand.Next(120, 200);

            myBody.Position = ConvertUnits.ToSimUnits(y, 200);

            // Offset drugiego klocka
            offset = new Vector2(ConvertUnits.ToDisplayUnits(GameC.Settings.blockSize), 0f);

            // Rotacja klocka
            myBody.Rotation = initialRotation;

        }

        private void MakeMe(BLOCKTYPES type)
        {
            // Lista wierzchołków kwadratów
            List<Vertices> rects = new List<Vertices>();

            // Translacja
            Vector2 trans = new Vector2();
            
            
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
            myBody = BodyFactory.CreateCompoundPolygon(worldRef, rects, 1f);
        }

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
