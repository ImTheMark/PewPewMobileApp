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

        private readonly IMobileServiceTable<Asset> assetTable = App.MobileService.GetTable<Asset>();
        private readonly IMobileServiceTable<Organization> orgTable = App.MobileService.GetTable<Organization>();
        private readonly IMobileServiceTable<Pending> pendingTable = App.MobileService.GetTable<Pending>();
        private readonly IMobileServiceTable<Town> townTable = App.MobileService.GetTable<Town>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            geolocator = new Geolocator();
           // populate();
            routeQuery.TravelMode = TravelMode.Walking;
            routeQuery.QueryCompleted += routeQuery_QueryCompleted;
            GetCurrentLocation();
            MyMapControl.CartographicMode = MapCartographicMode.Road;
            MyMapControl.LandmarksEnabled = true;
            MyMapControl.ZoomLevel = 16;

            loadRequestList();
        }


        private async void populate()
        {

            Organization org = new Organization();
            org.name = "oliver";
            org.contactperson = "alvin";
            org.phone = "5225";
            org.email = "o@yahoo.com";
            org.fax = "1241";
            org.username = "giver1";
            org.password = "111";
            org.address = "manila";
            org.longitude = 121.019205031916;
            org.latitude = 14.5596795435995;
            org.isverified = "true";

            await orgTable.InsertAsync(org);

            org = new Organization();
            org.name = "vil";
            org.contactperson = "john";
            org.phone = "657812";
            org.email = "v@yahoo.com";
            org.fax = "112111";
            org.username = "giver2";
            org.password = "111";
            org.address = "quezon";
            org.longitude = 121.016954993829;
            org.latitude = 14.5508508011699;
            org.isverified = "true";

            await orgTable.InsertAsync(org);

            org = new Organization();
            org.name = "joy";
            org.contactperson = "cassandra";
            org.phone = "123456765";
            org.email = "j@yahoo.com";
            org.fax = "4444444";
            org.username = "giver3";
            org.password = "111";
            org.address = "quezon city";
            org.longitude = 121.023305878043;
            org.latitude = 14.5516221038997;
            org.isverified = "true";

            await orgTable.InsertAsync(org);

            org = new Organization();
            org.name = "mark";
            org.contactperson = "selene";
            org.phone = "544444";
            org.email = "m@yahoo.com";
            org.fax = "34565";
            org.username = "giver4";
            org.password = "111";
            org.address = "luzon";
            org.longitude = 121.012187702581;
            org.latitude = 14.5663466770202;
            org.isverified = "true";

            await orgTable.InsertAsync(org);

            org = new Organization();
            org.name = "ral";
            org.contactperson = "archie";
            org.phone = "6455454";
            org.email = "r@yahoo.com";
            org.fax = "95464526";
            org.username = "giver5";
            org.password = "111";
            org.address = "visayas";
            org.longitude = 121.008334541693;
            org.latitude = 14.5574766118079;
            org.isverified = "true";

            await orgTable.InsertAsync(org);

            //

            Town town = new Town();
            town.name = "barangay1";
            town.contactperson = "mayor";
            town.phone = "85565225";
            town.email = "bar1@yahoo.com";
            town.username = "barangay1";
            town.password = "111";
            town.address = "bar 676";
            town.longitude = 121.021518521011;
            town.latitude = 14.5495645143092;

            await townTable.InsertAsync(town);

            town = new Town();
            town.name = "barangay2";
            town.contactperson = "bishop";
            town.phone = "899002125";
            town.email = "bar2@yahoo.com";
            town.username = "barangay2";
            town.password = "111";
            town.address = "bar 543";
            town.longitude = 121.02594207041;
            town.latitude = 14.5575703214854;

            await townTable.InsertAsync(town);

            town = new Town();
            town.name = "barangay3";
            town.contactperson = "duterte";
            town.phone = "4356789087654";
            town.email = "bar3@yahoo.com";
            town.username = "barangay3";
            town.password = "111";
            town.address = "bar 321";
            town.longitude = 121.025511743501;
            town.latitude = 14.5530046150088;

            await townTable.InsertAsync(town);

            town = new Town();
            town.name = "barangay4";
            town.contactperson = "gloria";
            town.phone = "437834783";
            town.email = "bar4@yahoo.com";
            town.username = "barangay4";
            town.password = "111";
            town.address = "bar 908";
            town.longitude = 121.019065640867;
            town.latitude = 14.5622836332768;

            await townTable.InsertAsync(town);

            town = new Town();
            town.name = "barangay5";
            town.contactperson = "ninoy";
            town.phone = "7342312";
            town.email = "bar5@yahoo.com";
            town.username = "barangay5";
            town.password = "111";
            town.address = "bar 777";
            town.longitude = 121.014296589419;
            town.latitude = 14.5614915434271;

            await townTable.InsertAsync(town);

            //

            Pending pend = new Pending();
            pend.townID = "";
            pend.ownerID = "";
            pend.assetName = "";

            await pendingTable.InsertAsync(pend);

            pend = new Pending();
            pend.townID = "";
            pend.ownerID = "";
            pend.assetName = "";

            await pendingTable.InsertAsync(pend);

            pend = new Pending();
            pend.townID = "";
            pend.ownerID = "";
            pend.assetName = "";

            await pendingTable.InsertAsync(pend);

            pend = new Pending();
            pend.townID = "";
            pend.ownerID = "";
            pend.assetName = "";

            await pendingTable.InsertAsync(pend);

            pend = new Pending();
            pend.townID = "";
            pend.ownerID = "";
            pend.assetName = "";

            await pendingTable.InsertAsync(pend);

            //

            Asset asset = new Asset();
            asset.name = "truck";
            asset.owner = "16A880E2-2C06-4220-8FCB-44606511A82D";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "truck";
            asset.owner = "16A880E2-2C06-4220-8FCB-44606511A82D";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "life vest";
            asset.owner = "1CC4C6CB-B47C-4F7E-A9C1-111210AD15CB";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "life vest";
            asset.owner = "1CC4C6CB-B47C-4F7E-A9C1-111210AD15CB";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "life vest";
            asset.owner = "6102DC97-18BC-4028-ADC6-B0B914FF493A";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "rubber boat";
            asset.owner = "1CC4C6CB-B47C-4F7E-A9C1-111210AD15CB";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);

            asset = new Asset();
            asset.name = "rubber boat";
            asset.owner = "6102DC97-18BC-4028-ADC6-B0B914FF493A";
            asset.borrower = "";
            asset.status = "AVAILABLE";

            await assetTable.InsertAsync(asset);
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
                if (asset.status.Equals("AVAILABLE"))
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
                Debug.WriteLine("o.id: " + o.id + "string id: " + id);
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