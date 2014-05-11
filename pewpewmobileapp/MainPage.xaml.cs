using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using pewpewmobileapp.Resources;
using pewpewmobileapp.Objects;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Windows.Devices.Geolocation;
using System.Text;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;

namespace pewpewmobileapp
{

    public partial class MainPage : PhoneApplicationPage
    {
        public static string userID = "";
        public static string userType = "";

        RouteQuery routeQuery = new RouteQuery();
        List<GeoCoordinate> waypoints;
        Geolocator geolocator;
        StringBuilder sb = new StringBuilder();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();

            routeQuery.TravelMode = TravelMode.Walking;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            GetCurrentLocation();
            MyMapControl.CartographicMode = MapCartographicMode.Road;
            MyMapControl.LandmarksEnabled = true;
            MyMapControl.ZoomLevel = 16;

            loadRequestList();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationService != null)
                while (NavigationService.CanGoBack)
                    NavigationService.RemoveBackEntry();

  
        }

        private async void loadRequestList()
        {
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                if (!asset.status.Equals("AVAILABLE"))
                {
                    Organization owner = await getOwner(asset.owner);
                    plotPoint(owner.latitude, owner.longitude);
                }
            }
        }

        private async Task<Organization> getOwner(string id)
        {
            var list = await App.MobileService.GetTable<Organization>().ToListAsync();
            foreach (Organization o in list)
            {
                if (o.id.Equals(id))
                {
                    return o;
                }
            }
            return null;
        }

        private void plotPoint(double p1, double p2)
        {
            GeoCoordinate geoCord = new GeoCoordinate(p1, p2);

            waypoints = new List<GeoCoordinate>();
            waypoints.Add(geoCord);
            MyMapControl.Center = geoCord;
            AddPoint(MyMapControl, geoCord);
        }

        private async void GetCurrentLocation()
        {
            try
            {
                geolocator.DesiredAccuracyInMeters = 50;
                geolocator.DesiredAccuracy = Windows.Devices.Geolocation.PositionAccuracy.High;
                // If the cached location is over 30 minutes old, get a new one
                TimeSpan maximumAge = new TimeSpan(0, 30, 0);
                // If we're trying for more than 45 seconds, abandon the operation
                TimeSpan timeout = new TimeSpan(0, 0, 15);
                Geoposition myLocation = await geolocator.GetGeopositionAsync(maximumAge, timeout);

                Debug.WriteLine("Latitude1: " + myLocation.Coordinate.Latitude + " Longitude1: " + myLocation.Coordinate.Longitude);

                //  GeoCoordinate geoCord = new GeoCoordinate(myLocation.Coordinate.Latitude, myLocation.Coordinate.Longitude);
                GeoCoordinate geoCord = new GeoCoordinate(14.5585541, 121.0181178);

                waypoints = new List<GeoCoordinate>();
                waypoints.Add(geoCord);
                MyMapControl.Center = geoCord;
                AddPoint(MyMapControl, geoCord);

                geolocator.PositionChanged += newPosition;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        void newPosition(Geolocator sender, PositionChangedEventArgs args)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                GeoCoordinate geoCord =
                    new GeoCoordinate(args.Position.Coordinate.Latitude,
                                        args.Position.Coordinate.Longitude);
                MyMapControl.Center = geoCord;
            });
        }

        bool isMappingRoute = false;

        private void AddRoute(object sender, System.EventArgs e)
        {
            isMappingRoute = true;
        }

        private void GetGeolocationOfTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (isMappingRoute)
            {
                Point tapPosition = e.GetPosition((UIElement)sender);
                GeoCoordinate tapLocation = MyMapControl.ConvertViewportPointToGeoCoordinate(tapPosition);
                waypoints.Add(tapLocation);
                AddPoint(MyMapControl, tapLocation);
            }

        }

        private void AddPoint(Map controlMap, GeoCoordinate geo)
        {
            MapLayer ml = new MapLayer();
            MapOverlay mo = new MapOverlay();

            // Add an Ellipse UI
            Ellipse r = new Ellipse();
            r.Fill = new SolidColorBrush(Color.FromArgb(255, 240, 5, 5));
            r.Width = r.Height = 12;
            r.Margin = new Thickness(-6, -6, 0, 0);
            mo.Content = r;
            mo.GeoCoordinate = geo;
            ml.Add(mo);
            controlMap.Layers.Add(ml);

            Debug.WriteLine("Latitude: " + geo.Latitude + " Longitude: " + geo.Longitude);
        }

        private void MappingRoute(object sender, System.EventArgs e)
        {
            isMappingRoute = false;
            routeQuery.Waypoints = waypoints;
            routeQuery.QueryAsync();
        }

        private void Direction(object sender, System.EventArgs e)
        {
            MessageBox.Show(sb.ToString());
        }

        void routeQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (null == e.Error)
            {
                Route MyRoute = e.Result;
                MapRoute MyMapRoute = new MapRoute(MyRoute);
                MyMapControl.AddRoute(MyMapRoute);
                MyMapControl.SetView(MyMapRoute.Route.BoundingBox);

                var i = 0;
                sb.AppendFormat("Estimated time: {0} minutes\n", e.Result.EstimatedDuration.TotalMinutes.ToString("#,###.##"));
                foreach (var leg in e.Result.Legs)
                    foreach (var maneuver in leg.Maneuvers)
                    {
                        sb.AppendFormat("{0}. {1}: {2}\n", ++i, maneuver.InstructionKind.ToString(), maneuver.InstructionText);
                    }
            }

            else
                MessageBox.Show("Error occured:\n" + e.Error.Message);

        }

        private void DownloadMapsForOffline(object sender, System.EventArgs e)
        {
            MapDownloaderTask mdt = new MapDownloaderTask();
            mdt.Show();
        }

        private async void loginAccount(object sender, RoutedEventArgs e)
        {
            int count = 0;
            //btnLogin.IsEnabled = false;

            try
            {
                if (txtUsername.Text.Equals("admin") && txtPassword.Password.Equals("admin"))
                {
                    NavigationService.Navigate(new Uri("/AdminPage.xaml", UriKind.Relative));
                    userID = "1";
                    userType = "ADMIN";
                    //txtMessage.Text = "LOGIN ADMIN";
                    count++;
                }
                else
                {
                    var results = App.MobileService.GetTable<Town>();
                    List<Town> filter = await results.ToListAsync();
                    foreach (Town a in filter)
                    {
                        if (a.email.Equals(txtUsername.Text) && a.password.Equals(txtPassword.Password))
                        {
                            userID = a.id;
                            userType = "TOWN";
                            count++;
                            break;
                        }
                    }
                    if (userType == "TOWN")
                        NavigationService.Navigate(new Uri("/BarangayPage.xaml", UriKind.Relative));
                        //txtMessage.Text = "LOGIN TOWN";
                    else
                    {
                        var results2 = App.MobileService.GetTable<Organization>();
                        List<Organization> filter2 = await results2.ToListAsync();
                        foreach (Organization a in filter2)
                        {
                            if (a.email.Equals(txtUsername.Text) && a.password.Equals(txtPassword.Password))
                            {
                                userID = a.id;
                                userType = "ORG";
                                count++;
                                break;
                            }
                        }
                        if (userType == "ORG")
                            NavigationService.Navigate(new Uri("/OrganizationPage.xaml", UriKind.Relative));
                            //txtMessage.Text = "LOGIN ORG";
                    }
                }

                //if (count == 0)
                //    txtMessage.Text = "ERROR";

            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("NO");
            }
        }
        
    }
}