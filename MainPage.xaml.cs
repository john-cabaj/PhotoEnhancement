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
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;

namespace PhotoEnhancement
{
    public partial class MainPage : PhoneApplicationPage
    {
        string[] filenames;

        public MainPage()
        {
            InitializeComponent();
        }

        private void textBlock_takePhoto_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Camera.xaml", UriKind.Relative));
        }

        private void textBlock_viewPhotos_Tap(object sender, GestureEventArgs e)
        {

        }

        private void image1_Tap(object sender, GestureEventArgs e)
        {
            
        }

        private void image2_Tap(object sender, GestureEventArgs e)
        {
            
        }

        private void image3_Tap(object sender, GestureEventArgs e)
        {

        }

        private void image4_Tap(object sender, GestureEventArgs e)
        {

        }

        private void image5_Tap(object sender, GestureEventArgs e)
        {

        }

        private void image6_Tap(object sender, GestureEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IList<BitmapImage> isoImages = getImages();
            
            image1.Source = isoImages[0];
            image2.Source = isoImages[1];
            image3.Source = isoImages[2];
            image4.Source = isoImages[3];
            image5.Source = isoImages[4];
            image6.Source = isoImages[5];
        }

        private IList<BitmapImage> getImages()
        {
            ImageBrush temp = new ImageBrush();
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            filenames = pickImages(iso.GetFileNames());
            IList<BitmapImage> images = new List<BitmapImage>();

            foreach (string file in filenames)
            {
                BitmapImage bimg = new BitmapImage();
                if (iso.FileExists(file))
                {
                    using (IsolatedStorageFileStream stream = iso.OpenFile(file, FileMode.Open, FileAccess.Read))
                    {
                        bimg.SetSource(stream);
                    }
                }
                else
                    bimg = null;

                images.Add(bimg);
            }

            return images;
        }

        private string[] pickImages(string[] files)
        {
            string[] filenames = new string[6];
            Random rand = new Random();
            IList<int> index = new List<int>();
            int temp;

            for (int i = 0; i < files.Length && i < filenames.Length; i++)
            {
                temp = rand.Next(files.Length - 1);

                while (index.Contains(temp) || !files[temp].Contains("_th.jpg"))
                    temp = rand.Next(files.Length - 1);

                filenames[i] = files[temp];
                index.Add(temp);
            }

            return filenames;
        }
    }
}