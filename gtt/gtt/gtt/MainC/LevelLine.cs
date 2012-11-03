using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace gtt.MainC
{

    /// <summary>
    /// Klasa przedstawiająca linię - wysokosc do osiągnięcia - w którym zaliczony jest tak jakby level
    /// Na razie brak widocznej linii na ekranie
    /// 
    /// autor: Tomasz Dietrich
    /// </summary>
    public class LevelLine
    {
        /// <summary>
        /// Wysokosc danej linii. Należy ją zmieniać w miare jak idziemy do góry, i wywolywac rysowanie.
        /// Co kazdy draw gry. Wtedy linia się bedzie rysowac w takiej wysokosci jak ta.
        /// </summary>
        public int height;

        /// <summary>
        /// SpriteBatch do rysowania
        /// </summary>
        private PrimitiveBatch primitiveBatch;

        /// <summary>
        /// Linia
        /// </summary>
        private Line myLine;

        /// <summary>
        /// Tekst informujacy o wysokosci
        /// </summary>
        private TextBlock heightText;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public LevelLine(int _height)
        {
            // Przypisania, tworzenie obiektow
            height = _height;
            myLine = new Line();
            heightText = new TextBlock();
            // Wypelnienie linii textem
            heightText.Text = height.ToString() + "m";


        }

    }
}
