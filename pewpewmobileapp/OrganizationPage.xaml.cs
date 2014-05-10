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

namespace pewpewmobileapp
{
    public partial class OrganizationPage : PhoneApplicationPage
    {
        public OrganizationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            int x = myAssets.Children.Count;
            for (int i = 0; i < x; i++)
                myAssets.Children.RemoveAt(0);
            x = requestedAssets.Children.Count;
            for (int i = 0; i < x; i++)
                requestedAssets.Children.RemoveAt(0);
            loadRequestList();
            loadMyAssetList();
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
            myAssets.Children.Add(truckList);
            myAssets.Children.Add(lifevestList);
            myAssets.Children.Add(doctorList);
            myAssets.Children.Add(rubberBoatList);
            myAssets.Children.Add(fireTruckList);
            myAssets.Children.Add(militaryList);
            myAssets.Children.Add(socialWorkerList);
            myAssets.Children.Add(tentList);
            myAssets.Children.Add(radioList);
            myAssets.Children.Add(constructionEquipmentsList);
            myAssets.Children.Add(helicopterList);
            myAssets.Children.Add(generatorsList);
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
            throw new NotImplementedException();
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