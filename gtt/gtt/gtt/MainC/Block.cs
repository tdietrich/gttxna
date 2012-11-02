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
    /// </summary>
    /// 
    public class Block
    {

        #region Fields

        private Border border;
        private Sprite rectangleSprite;
        private Body rectangles;
        private Vector2 offset;

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
        public Block(BLOCKTYPES _type, ref World _worldRef,Texture2D texture)
        {
            type = _type;
            worldRef = _worldRef;
            tex = texture;
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);
            rectangles = new Body(worldRef);
            InitializeBlock();
        }

        /// <summary>
        /// Funkcja inicjalizucje block w zaleznosci od typu podanego do konstruktora
        /// </summary>
        private void InitializeBlock()
        {
            // Funnkcja Tworzy cialo skladjace sie z wielu kwadratow w zaleznosci od typu tetrisowego klocka
            MakeMe(type);
            
            rectangles.BodyType = BodyType.Dynamic;
            var rand = new Random();
            int y = rand.Next(120, 200);

            rectangles.Position = ConvertUnits.ToSimUnits(y, 200);
            //rectangles.Inertia = 0.5f;
            rectangles.Restitution = 0.0f;
            rectangles.Friction = 0.2f;

            // Offset drugiego klocka
            offset = new Vector2(ConvertUnits.ToDisplayUnits(GameC.BlockSize), 0f);

            

        }

        private void MakeMe(BLOCKTYPES type)
        {
            // Lista wierzchołków kwadratów
            List<Vertices> rects = new List<Vertices>();

            // Translacja
            Vector2 trans = new Vector2();
            
            
            for (int i = 1; i < 5; i++)
            {
                rects.Add(PolygonTools.CreateRectangle(GameC.BlockSize, GameC.BlockSize));
            }


            // Główny switch po typach
            switch (type)
             {
                 case BLOCKTYPES.I_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         rects[i-1].Translate(new Vector2(0, i *2*GameC.BlockSize));
                     }

                     break;



                 case BLOCKTYPES.J_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 4)
                         {
                             rects[i-1].Translate(new Vector2(-2 * GameC.BlockSize,i* GameC.BlockSize + (2*GameC.BlockSize)));
                         }
                         else
                            rects[i-1].Translate(new Vector2(0, i *2*GameC.BlockSize));
                         
                     }

                     



                     break;
                 case BLOCKTYPES.L_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 4)
                         {
                             rects[i-1].Translate(new Vector2(2 * GameC.BlockSize,i* GameC.BlockSize + (2*GameC.BlockSize)));
                         }
                         else
                            rects[i-1].Translate(new Vector2(0, i *2*GameC.BlockSize));
                         
                     }




                     break;
                 case BLOCKTYPES.O_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                            rects[i-1].Translate(new Vector2(2 * GameC.BlockSize,0));

                         else if(i == 3)
                            rects[i-1].Translate(new Vector2(0, 2*GameC.BlockSize));  
                        
                         else if(i == 4)
                             rects[i - 1].Translate(new Vector2(2 * GameC.BlockSize,2 * GameC.BlockSize));  

                     }
                     break;
                 case BLOCKTYPES.S_SHAPE:


                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                            rects[i-1].Translate(new Vector2(2 * GameC.BlockSize,0));

                         else if(i == 3)
                            rects[i-1].Translate(new Vector2(0, 2*GameC.BlockSize));  
                        
                         else if(i == 4)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.BlockSize,2 * GameC.BlockSize));  

                     }

                     break;
                 case BLOCKTYPES.T_SHAPE:

                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.BlockSize));

                         else if (i == 3)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.BlockSize, 2 * GameC.BlockSize));

                         else if (i == 4)
                             rects[i - 1].Translate(new Vector2(2 * GameC.BlockSize, 2 * GameC.BlockSize));

                     }

                     break;
                 case BLOCKTYPES.Z_SHAPE:
                     for (int i = 1; i < 5; i++)
                     {
                         if (i == 2)
                             rects[i - 1].Translate(new Vector2(0, 2 * GameC.BlockSize));

                         else if (i == 3)
                             rects[i - 1].Translate(new Vector2(2 * GameC.BlockSize, 2 * GameC.BlockSize));

                         else if (i == 4)
                             rects[i - 1].Translate(new Vector2(-2 * GameC.BlockSize,0));

                     }


                     break;
             }
            rectangles = BodyFactory.CreateCompoundPolygon(worldRef, rects, 1f);
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                spriteBatch.Draw(tex, ConvertUnits.ToDisplayUnits(rectangles.Position), null,
                                       Color.White, rectangles.Rotation,
                                       rectangleSprite.Origin + offset, 1f, SpriteEffects.None, 0f);
                    // draw second rectangle
                spriteBatch.Draw(tex, ConvertUnits.ToDisplayUnits(rectangles.Position), null, Color.White,
                                 rectangles.Rotation, rectangleSprite.Origin - offset, 1f, SpriteEffects.None, 0f);
            spriteBatch.End();
        }


        #endregion Methods
    }
}
