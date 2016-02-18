//Name:                       MovingCircles class
//Date of creation:           15th Dec 2012
//Created by:                 Ganesh Moholkar 
//                            9975953531
//                            ganeshmoholkar@gmail.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hotel_POS.User_Controls
{
    /// <summary>
    /// Interaction logic for MovingCircles.xaml
    /// </summary>
    public partial class MovingCircles : UserControl
    {
        private TimeSpan timeInterval;

        public MovingCircles()
        {
            InitializeComponent();
        }

        public double CirclesHeight
        {
            get
            {
                return ellipse.Height;
            }

            set
            {
                ellipse.Height = value;
            }
        }

        public double CirclesWidth
        {
            get
            {
                return ellipse.Width;
            }

            set
            {
                ellipse.Width = value;
            }
        }

        public Brush CirclesFill
        {
            get
            {
                return ellipse.Fill;
            }

            set
            {
                ellipse.Fill = value;
            }
        }

        public TimeSpan TimeInterval
        {
            get
            {
                return timeInterval;
            }

            set
            {
                timeInterval = value;

                keyFrame1.KeyTime = keyFrame1.KeyTime.TimeSpan.Add(timeInterval);
                keyFrame2.KeyTime = keyFrame2.KeyTime.TimeSpan.Add(timeInterval);
                keyFrame3.KeyTime = keyFrame3.KeyTime.TimeSpan.Add(timeInterval);
                keyFrame4.KeyTime = keyFrame4.KeyTime.TimeSpan.Add(timeInterval);
                keyFrame5.KeyTime = new TimeSpan(0, 0, 6);
            }
        }

        private void movingCircles_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.WidthChanged)
            {
                keyFrame1.Value = 0;
                keyFrame2.Value = this.ActualWidth / 3;
                keyFrame3.Value = this.ActualWidth * 2 / 3;
                keyFrame4.Value = this.ActualWidth;
                keyFrame5.Value = this.ActualWidth;
            }
        }
    }
}
