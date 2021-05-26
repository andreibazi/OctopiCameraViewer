using AForge.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace PrinterCam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MJPEGStream stream = new AForge.Video.MJPEGStream("http://octopi.local/webcam/?action=stream");

            stream.Start();

            stream.NewFrame += new NewFrameEventHandler(UseFrame);

            void UseFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
            {
                Bitmap FrameData = new Bitmap(eventArgs.Frame);

                Dispatcher.BeginInvoke(new ThreadStart(() => cam.Source = (ImageSource)Convert(FrameData)));
               
            }

            object Convert(object value)
            {
                MemoryStream ms = new MemoryStream();
                ((System.Drawing.Bitmap)value).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                return image;
            }

        }

        private void Window_SourceInitialized(object sender, EventArgs ea)
        {
            WindowAspectRatio.Register((Window)sender);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
