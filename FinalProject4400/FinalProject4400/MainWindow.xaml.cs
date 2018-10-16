using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Drawing;

namespace FinalProject4400
{
    public class location
    {
        public double xLocation { get; set; }
        public double yLocation { get; set; }
    }

    public partial class MainWindow : Window
    {
        public flappyBaseClass flappy;
        Thread pipeThread;
        Thread lifeThread;

        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += delegate (object sender, KeyEventArgs e)
            {
                if (flappy != null) { flappy.Flap(); }
            };
        }

        private void PlayMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //pipe.Paused = false;
            flappy.Paused = false;

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (flappy != null)
            {
                flappy.Shutdown();
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            start.Visibility = System.Windows.Visibility.Hidden;
            rectangle.Visibility = System.Windows.Visibility.Hidden;
            flappyText.Visibility = System.Windows.Visibility.Hidden;
            birdText.Visibility = System.Windows.Visibility.Hidden;

            flappy = new bird(gameArea, Dispatcher, 3);
            flappy.Paused = false;
            flappy.Place(50, 300);

            pipeThread = new Thread(delegate ()
            {
                while (true)
                {
                    this.Dispatcher.Invoke(
                               DispatcherPriority.SystemIdle,
                               new Action(() => addPipes()));
                    Thread.Sleep(2300);
                }
            });
            pipeThread.Start();
        }

        public void addPipes()
        {
            Random rnd = new Random();
            double random = rnd.Next(-180, 180);


            flappyBaseClass bottomPipe = new bottomPipe(gameArea, Dispatcher, 3);
            bottomPipe.Paused = false;
            bottomPipe.Place(788, random);

            flappyBaseClass topPipe = new topPipe(gameArea, Dispatcher, 3);
            topPipe.Paused = false;
            topPipe.Place(788, random);

            lifeThread = new Thread(delegate ()
            {
                while (true)
                {
                    this.Dispatcher.Invoke(
                               DispatcherPriority.SystemIdle,
                               new Action(() => life(bottomPipe, topPipe)));
                    Thread.Sleep(30);
                }
            });
            lifeThread.Start();
            if (!flappy.isAlive())
            {
                this.pipeThread.Abort();
            }

        }

        public void life(flappyBaseClass bottomPipe, flappyBaseClass topPipe)
        {

            try
            {
                Rect bottom = new Rect(bottomPipe.GetLeft, bottomPipe.GetTop, bottomPipe.ActualWidth, bottomPipe.ActualHeight);
                Rect top = new Rect(topPipe.GetLeft, topPipe.GetTop, topPipe.ActualWidth, topPipe.ActualHeight);
                Rect flappybird = new Rect(flappy.GetLeft, flappy.GetTop, flappy.ActualWidth - 10, flappy.ActualHeight - 10);

                Rect addScore = new Rect(bottomPipe.GetLeft, topPipe.GetTop, 50, 600);

                if (addScore.IntersectsWith(flappybird))
                {
                    if (!score.HasContent)
                    {
                        score.Content = '1';
                    }
                    else
                    {
                        if (flappy.isAlive())
                        {
                            score.Content = int.Parse(score.Content.ToString()) + 1;
                        }
                    }
                }

                if (bottom.IntersectsWith(flappybird) || top.IntersectsWith(flappybird) || !flappy.isAlive())
                {
                    topPipe.Shutdown();
                    bottomPipe.Shutdown();

                    flappy.isAlive(false);
                    flappy.Shutdown();
                    this.lifeThread.Abort();
                    close.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception e)
            {
                //throw e;
            }
        }
        private void close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
