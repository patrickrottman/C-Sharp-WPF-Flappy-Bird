using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FinalProject4400
{
    public class flappyBaseClass
    {
        protected Canvas gameArea;
        protected Dispatcher dispatcher;
        protected double x, y, actualHeight, actualWidth, getLeft, getTop;
        Int32 waitTime;
        Boolean paused = true;

        public flappyBaseClass(Canvas gameArea, Dispatcher dispatcher, Int32 waitTime = 100)
        {
            this.gameArea = gameArea;
            this.dispatcher = dispatcher;
            this.waitTime = waitTime;
        }

        public Int32 WaitTime
        {
            get
            {
                return waitTime;
            }
            set
            {
                waitTime = value;
            }
        }

        public Boolean Paused
        {
            get
            {
                return paused;
            }

            set
            {
                if (value) paused = true;
                else paused = false;
            }
        }

        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public double ActualHeight
        {
            get
            {
                return actualHeight;
            }
            set
            {
                actualHeight = value;
            }
        }


        public double ActualWidth
        {
            get
            {
                return actualWidth;
            }
            set
            {
                actualWidth = value;
            }
        }

        public double GetLeft
        {
            get
            {
                return getLeft;
            }
            set
            {
                getLeft = value;
            }
        }


        public double GetTop
        {
            get
            {
                return getTop;
            }
            set
            {
                getTop = value;
            }
        }

        public virtual void Place(double x = 100, double y = 200)
        {
            this.x = x;
            this.y = y;
        }

        protected virtual BitmapImage LoadBitmap(String assetsRelativePath, double decodeHeight)
        {
            BitmapImage theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            String basePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"assets\");
            String path = System.IO.Path.Combine(basePath, assetsRelativePath);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelHeight = (int)decodeHeight;
            theBitmap.EndInit();

            return theBitmap;
        }

        protected virtual BitmapImage LoadBitmap(String assetsRelativePath, double decodeHeight, double decodeWidth)
        {
            BitmapImage theBitmap = new BitmapImage();
            theBitmap.BeginInit();
            String basePath = System.IO.Path.Combine(Environment.CurrentDirectory, @"assets\");
            String path = System.IO.Path.Combine(basePath, assetsRelativePath);
            theBitmap.UriSource = new Uri(path, UriKind.Absolute);
            theBitmap.DecodePixelHeight = (int)decodeHeight;
            theBitmap.DecodePixelWidth = (int)decodeWidth;
            theBitmap.EndInit();

            return theBitmap;
        }

        public virtual void Shutdown()
        {

        }

        public virtual void Flap()
        {

        }

        public virtual location getLocation()
        {
            location Location = new location();
            Location.xLocation = 0;
            Location.yLocation = 0;
            return Location;
        }

        public virtual Boolean isAlive(Boolean? bird = null)
        {
            return false;
        }
    }
}
