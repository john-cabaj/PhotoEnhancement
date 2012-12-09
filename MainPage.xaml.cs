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
        IList<Image> imagesList = new List<Image>();

        public MainPage()
        {
            InitializeComponent();
            imagesList.Add(image1);
            imagesList.Add(image2);
            imagesList.Add(image3);
            imagesList.Add(image4);
            imagesList.Add(image5);
            imagesList.Add(image6);
        }

        private void textBlock_takePhoto_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Camera.xaml", UriKind.Relative));
        }

        private void textBlock_viewPhotos_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AllPhotos.xaml", UriKind.Relative));
        }

        private void image_Tap(object sender, GestureEventArgs e)
        {
            Image image = (Image)sender;
            string file = image.Tag.ToString();
            file = file.Substring(0, file.LastIndexOf('_'));
            file = file + ".jpg";
            NavigationService.Navigate(new Uri("/FullView.xaml?msg=" + file, UriKind.Relative));
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
            for (int i = 0; i < filenames.Length; i++)
            {
                imagesList[i].Tag = filenames[i];
            }
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
            IList<string> thumbnails = findThumbnails(files);
            Random rand = new Random();
            IList<int> index = new List<int>();
            int temp;

            for (int i = 0; i < thumbnails.Count && i < filenames.Length; i++)
            {
                temp = rand.Next(thumbnails.Count);

                while (index.Contains(temp))
                    temp = rand.Next(thumbnails.Count);

                filenames[i] = thumbnails[temp];
                index.Add(temp);
            }

            return filenames;
        }

        private IList<string> findThumbnails(string[] files)
        {
            IList<string> thumbnails = new List<string>();

            foreach (string file in files)
            {
                if (file.Contains("_th.jpg"))
                    thumbnails.Add(file);
            }

            return thumbnails;
        }

    }
}