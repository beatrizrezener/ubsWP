using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace saudf
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;

            //try
            //{
            //    Geoposition geoposition = await geolocator.GetGeopositionAsync(
            //         maximumAge: TimeSpan.FromMinutes(5),
            //         timeout: TimeSpan.FromSeconds(10)
            //        );

            //    //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
            //    geolocation.Text = "GPS:" + geoposition.Coordinate.Latitude.ToString("0.0000000") + ", " + geoposition.Coordinate.Longitude.ToString("0.0000000");
            //}
            ////If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
            ////The second is that the user doesn't turned on the Location Services
            //catch (UnauthorizedAccessException)
            //{
            //    //print exception: "location is disabled in phone settings"
            //}

            Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;
            Geoposition geoposition = null;
            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
  
                geolocation.Text = "GPS:" + geoposition.Coordinate.Point.Position.Latitude.ToString("0.0000000") + ", " + geoposition.Coordinate.Point.Position.Longitude.ToString("0.0000000");
            }
            catch (Exception ex)
            {
                // Handle errors like unauthorized access to location
                // services or no Internet access.
            }
            myMapControl.Center = geoposition.Coordinate.Point;
            myMapControl.ZoomLevel = 15;

            MapIcon mapIcon = new MapIcon();
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinkPushPin.png"));
            mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
            mapIcon.Location = geoposition.Coordinate.Point;
            mapIcon.Title = "You are here";
            myMapControl.MapElements.Add(mapIcon);
        }
    }
}
