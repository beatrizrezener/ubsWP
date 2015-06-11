using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
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

        public  Geoposition currentPosition { get; set; }

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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MapService.ServiceToken = "9sS3k8A_lN-OP2NWhVxW5g";
            ShowRouteOnMap();
            //ShowMyPosition();
        }

        private async void ShowRouteOnMap()
        {
            Geolocator geolocator = new Geolocator();
            currentPosition = null;
            try
            {
                currentPosition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
            }
            catch (Exception)
            {
                // Handle errors like unauthorized access to location
                // services or no Internet access.
            }

            // Start
            BasicGeoposition startLocation = new BasicGeoposition();
            startLocation.Latitude = currentPosition.Coordinate.Point.Position.Latitude;
            startLocation.Longitude = currentPosition.Coordinate.Point.Position.Longitude;
            Geopoint startPoint = new Geopoint(startLocation);

            // End
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = -15.890103578567;
            endLocation.Longitude = -48.142154216765;

            Geopoint endPoint = new Geopoint(endLocation);

            // Get the route between the points.
            MapRouteFinderResult routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                startPoint,
                endPoint,
                MapRouteOptimization.Time,
                MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                myMapControl.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await myMapControl.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }

        private async void ShowMyPosition()
        {
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
            catch (Exception)
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
