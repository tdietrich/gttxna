using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using gtt.MainC;

namespace gtt.Pages
{
    public partial class Options : PhoneApplicationPage
    {
       
        private double frictValue;
        public Options()
        {
            InitializeComponent();


        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            
            frictValue = Math.Round(frictSlider.Value, 2);
            
            textBlock4.Text = frictSlider.Value.ToString();

        }
        private void frictSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (textBlock4 != null)
            {
                textBlock4.Text = e.NewValue.ToString();
                this.frictValue = (float)e.NewValue;
            }

        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OptionsHandler.blocksFriction = (float)this.frictValue;
        }


        
    }
}