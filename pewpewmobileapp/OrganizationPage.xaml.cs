using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using pewpewmobileapp.Objects;
using System.Threading.Tasks;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Windows.Devices.Geolocation;
using System.Text;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Maps.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;

namespace pewpewmobileapp
{
    public partial class OrganizationPage : PhoneApplicationPage
    {

        RouteQuery routeQuery = new RouteQuery();
        List<GeoCoordinate> waypoints;
        Geolocator geolocator;
        StringBuilder sb = new StringBuilder();

        public OrganizationPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            routeQuery.TravelMode = TravelMode.Walking;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            GetCurrentLocation();
            MyMapControl.CartographicMode = MapCartographicMode.Road;
            MyMapControl.LandmarksEnabled = true;
            MyMapControl.ZoomLevel = 16;

            int x = myAssets.Children.Count;
            for (int i = 0; i < x; i++)
                myAssets.Children.RemoveAt(0);
            x = requestedAssets.Children.Count;
            for (int i = 0; i < x; i++)
                requestedAssets.Children.RemoveAt(0);
            loadRequestList();
            loadMyAssetList();
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

        private async void loadMyAssetList()
        {
            StackPanel truckList = new StackPanel();
            StackPanel lifevestList = new StackPanel();
            StackPanel doctorList = new StackPanel();
            StackPanel rubberBoatList = new StackPanel();
            StackPanel fireTruckList = new StackPanel();
            StackPanel militaryList = new StackPanel();
            StackPanel socialWorkerList = new StackPanel();
            StackPanel tentList = new StackPanel();
            StackPanel radioList = new StackPanel();
            StackPanel constructionEquipmentsList = new StackPanel();
            StackPanel helicopterList = new StackPanel();
            StackPanel generatorsList = new StackPanel();
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                if (asset.owner.Equals(MainPage.userID))
                {
                    Button b = new Button();
                    b.Name = asset.id;
                    b.Content = asset.name + "\n" + asset.borrower + "\n" + asset.status;
                    b.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/AssetPage.xaml?param0=" + asset.id + "&param1=" + "REQUEST DETAIL", UriKind.Relative));
                    };
                    switch (asset.id)
                    {
                        case "truck":
                            truckList.Children.Add(b); break;
                        case "lifevest":
                            lifevestList.Children.Add(b); break;
                        case "doctor":
                            doctorList.Children.Add(b); break;
                        case "rubberBoat":
                            rubberBoatList.Children.Add(b); break;
                        case "fireTruck":
                            fireTruckList.Children.Add(b); break;
                        case "military":
                            militaryList.Children.Add(b); break;
                        case "socialWorker":
                            socialWorkerList.Children.Add(b); break;
                        case "tent":
                            tentList.Children.Add(b); break;
                        case "radio":
                            radioList.Children.Add(b); break;
                        case "constructionEquipments":
                            constructionEquipmentsList.Children.Add(b); break;
                        case "helicopter":
                            helicopterList.Children.Add(b); break;
                        case "generators":
                            generatorsList.Children.Add(b); break;
                    }
                }
            }
        }

        private async void loadRequestList()
        {
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                if (asset.owner.Equals(MainPage.userID) && asset.status.Equals("ON THE WAY"))
                {
                    Town borrower = await getBorrower(asset.borrower);
                    Button b = new Button();
                    b.Name = asset.id;
                    b.Content = asset.name + "\n" + asset.borrower + "\n"+ borrower.address;
                    b.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/AssetPage.xaml?param0=" + asset.id + "&param1=" + "REQUEST DETAIL", UriKind.Relative));
                    };
                    Organization owner = await getOwner(asset.owner);
                    plotPoint(owner.latitude, owner.longitude);
                    requestedAssets.Children.Add(b);
                }
            }
        }

        private void plotPoint(double p1, double p2)
        {
            GeoCoordinate geoCord = new GeoCoordinate(p1, p2);

            waypoints = new List<GeoCoordinate>();
            waypoints.Add(geoCord);
            MyMapControl.Center = geoCord;
            AddPoint(MyMapControl, geoCord);
        }

        private async Task<Town> getBorrower(string id)
        {
            var list = await App.MobileService.GetTable<Town>().ToListAsync();
            foreach (Town t in list)
            {
                if (t.id.Equals(id))
                {
                    return t;
                }
            }
            return null;
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
    }
}