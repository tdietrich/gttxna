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

namespace gtt.MainC
{
    public static class OptionsHandler
    {
        public static float blocksBounciness = 0.01f;
        public static float blocksFriction = 3.0f;

        public static void SetDefault(float friction, float bounc)
        {
            blocksBounciness = bounc;
            blocksFriction = friction;
        }
    }
}
