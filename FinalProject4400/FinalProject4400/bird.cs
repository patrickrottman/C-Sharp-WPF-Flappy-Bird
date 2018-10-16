using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;

namespace FinalProject4400
{
    class bird : flappyBaseClass
    {
        BitmapImage birdBitmap;
        double minY = 0.0;
        double flappyHeight = 50;
        double gameAreaHeight = 0;
        double flapSize = 0;
        Image flappy;
        private Boolean goDown = false;
        private Thread posnThread = null;
        private bool flap = false;
        private Int32 waitTime;
        double incrementSize = 5;
        Boolean alive = true;

        public bird(Canvas gameArea, Dispatcher dispatcher, Int32 waitTime) : base(gameArea, dispatcher, waitTime)
        {

            birdBitmap = LoadBitmap(@"bird.png", flappyHeight);
            //creature.Source = leftBitmap;
            flappy = new Image();
            flappy.Source = birdBitmap;
            flappy.Height = flappyHeight;
            //waitTime = this.waitTime;
            //MessageBox.Show(waitTime.ToString());
        }


        public override void Place(double x = 100.0, double y = 200.0)
        {

            switch (goDown)
            {
                case false:
                    {
                        goDown = true;
                        break;
                    }
                case true:
                    {
                        goDown = false;
                        break;
                    }
                default:
                    {
                        goDown = true;
                        break;
                    }
            }
            this.waitTime = 30;
            this.x = x;
            this.y = y;
            gameArea.Children.Add(flappy);
            flappy.SetValue(Canvas.TopProperty, this.y);
            flappy.SetValue(Canvas.LeftProperty, this.x);

            posnThread = new Thread(Position);

            posnThread.Start();
        }

        void Position()
        {
            while (alive)
            {
                if (Paused == false)
                {
                    if (!flap)
                    {
                        y += incrementSize;
                        this.ActualHeight = flappy.ActualHeight;
                        this.ActualWidth = flappy.ActualWidth;
                        Action getInfo = delegate ()
                        {
                            this.GetLeft = Canvas.GetLeft(flappy);
                            this.GetTop = Canvas.GetTop(flappy);
                        };
                        gameArea.Dispatcher.Invoke(DispatcherPriority.Normal, getInfo);

                        if (y > gameArea.ActualHeight-flappy.ActualHeight)
                        {
                            alive = false;
                            posnThread.Abort();
                            //goDown = false;
                        }
                    }
                    else
                    {
                        y -= incrementSize * 2;

                        if (flapSize > 0)
                        {
                            flapSize = flapSize - incrementSize;
                        }
                        else
                        {
                            goDown = true;
                            flap = false;
                        }

                        if (y >= minY)
                        {
                            goDown = true;
                            flap = false;
                        }
                    }

                    UpdatePosition();
                }
                Thread.Sleep(waitTime);
            }
        }

        void UpdatePosition()
        {
            gameAreaHeight = (int)this.gameArea.ActualHeight;
            minY = gameAreaHeight - flappyHeight;

            Action action = () => { flappy.SetValue(Canvas.TopProperty, y); /*gameArea.SetValue(Canvas.TopProperty, y);*/ };
            dispatcher.BeginInvoke(action);
        }

        void SwitchBitmap(BitmapImage theBitmap)
        {
            //Random r = new Random();
            Action action = () => { flappy.Source = theBitmap; };
            dispatcher.BeginInvoke(action);
            //dispatcher.Invoke(() =>
            //{
            //    gameArea.Background = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), (byte)r.Next(1, 255), (byte)r.Next(1, 233)));
            //});
        }


        public override void Flap()
        {
            flap = true;
            flapSize = 30;
        }

        public override Boolean isAlive(bool? isAlive = null)
        {
            if (isAlive == false)
            {
                alive = false;
            }

            return alive;
        }

        public override location getLocation()
        {
            location flappyLocation = new location();
            flappyLocation.xLocation = this.x;
            flappyLocation.yLocation = this.y;
            return flappyLocation;
        }

        public override void Shutdown()
        {
            if (posnThread != null)
            {
                posnThread.Abort();
            }
        }

    }
}
