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
using Microsoft.Phone.Tasks;
using ImageTools;
using ImageTools.Filtering;

namespace PhotoEnhancement
{
    public partial class FullView : PhoneApplicationPage
    {
        BitmapImage image = new BitmapImage();

        public FullView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                string msg = NavigationContext.QueryString["msg"];

                IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();

                if (iso.FileExists(msg))
                {
                    using (IsolatedStorageFileStream stream = iso.OpenFile(msg, FileMode.Open, FileAccess.Read))
                    {
                        image.SetSource(stream);
                    }
                }
                else
                    image = null;

                imageView.Source = image;

            }
            catch (KeyNotFoundException ex)
            {
                //do nothing
            }
        }

        private void ApplicationBarSaveButton_Click(object sender, EventArgs e)
        {

        }

        private void invert_Click(object sender, EventArgs e)
        {
            ExtendedImage loadedImage;
            ExtendedImage filteredImage;
            Inverter filter;

            WriteableBitmap wb = new WriteableBitmap(image);
            loadedImage = new ExtendedImage();
            loadedImage.SetPixels(image.PixelWidth, image.PixelHeight, wb.ToByteArray());
            filter = new Inverter();
            filteredImage = new ExtendedImage(loadedImage);
            filter.Apply(filteredImage, loadedImage, new ImageTools.Rectangle(0, 0, loadedImage.PixelWidth, loadedImage.PixelHeight));
            var bitmap = filteredImage.ToBitmap();
            imageView.Source = bitmap;
        }
    }
}