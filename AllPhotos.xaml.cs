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
    public partial class AllPhotos : PhoneApplicationPage
    {
        IList<Image> images = new List<Image>();
        public AllPhotos()
        {
            InitializeComponent();
            progressBar.Visibility = Visibility.Visible;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            IList<string> thumbnails = findThumbnails(iso.GetFileNames());
            int margin = 5;
            foreach (Image img in images)
            {
                ContentPanel.Children.Remove(img);
            }

            for (int i = 0; i < thumbnails.Count; i = i + 2)
            {
                Image image1 = new Image();
                image1.Height = 150;
                image1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                image1.Margin = new Thickness(15, margin, 0, 0);
                image1.Stretch = Stretch.Fill;
                image1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                image1.Width = 230;
                image1.Tap +=new EventHandler<GestureEventArgs>(image_Tap);
                image1.Tag = thumbnails[i];
                image1.Source = new WriteableBitmap(getThumbnail(thumbnails[i], iso)).Rotate(90);
                images.Add(image1);
                ContentPanel.Children.Add(image1);

                if (i + 1 < thumbnails.Count)
                {
                    Image image2 = new Image();
                    image2.Height = 150;
                    image2.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    image2.Margin = new Thickness(255, margin, 0, 0);
                    image2.Stretch = Stretch.Fill;
                    image2.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    image2.Width = 240;
                    image2.Tap += new EventHandler<GestureEventArgs>(image_Tap);
                    image2.Tag = thumbnails[i+1];
                    image2.Source = new WriteableBitmap(getThumbnail(thumbnails[i + 1], iso)).Rotate(90);
                    images.Add(image2);
                    ContentPanel.Children.Add(image2);
                }

                margin += 159;
            }

            progressBar.Visibility = Visibility.Collapsed;
        }

        private BitmapImage getThumbnail(string file, IsolatedStorageFile iso)
        {
            BitmapImage bmp = new BitmapImage();

            if (iso.FileExists(file))
            {
                using (IsolatedStorageFileStream stream = iso.OpenFile(file, FileMode.Open, FileAccess.Read))
                {
                    bmp.SetSource(stream);
                }
            }
            else
                bmp = null;

            return bmp;
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

        private void image_Tap(object sender, GestureEventArgs e)
        {
            Image image = (Image)sender;
            string file = image.Tag.ToString();
            file = file.Substring(0, file.LastIndexOf('_'));
            file = file + ".jpg";
            NavigationService.Navigate(new Uri("/FullView.xaml?msg=" + file, UriKind.Relative));
        }
    }
}