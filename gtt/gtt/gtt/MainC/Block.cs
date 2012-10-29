using System.Windows.Controls;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
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
        /// Typ klocka - określany przez BlockTypesEnum
        /// </summary>
        private BLOCKTYPES type;

        #endregion Fields
        

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Typ klocka, pobrany z BlockTypesEnum</param>
        public Block(BLOCKTYPES _type)
        {
            type = _type;

            InitializeBlock();
        }


        /// <summary>
        /// Funkcja inicjalizucje block w zaleznosci od typu podanego do konstruktora
        /// </summary>
        private void InitializeBlock()
        {
            switch (type)
            {
                case BLOCKTYPES.I_SHAPE:
                    break;
                case BLOCKTYPES.J_SHAPE:
                    break;
                case BLOCKTYPES.L_SHAPE:
                    break;
                case BLOCKTYPES.O_SHAPE:
                    break;
                case BLOCKTYPES.S_SHAPE:
                    break;
                case BLOCKTYPES.T_SHAPE:
                    break;
                case BLOCKTYPES.Z_SHAPE:
                    break;
            }

        }

        #endregion Methods
    }
}
