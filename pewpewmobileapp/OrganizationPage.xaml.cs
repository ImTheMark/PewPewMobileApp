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
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                //if (asset.owner.Equals(MainPage.userID) && asset.status.Equals("ON THE WAY"))
                //{ 
                //}
            }
        }

        private async void loadRequestList()
        {
            loadMaps();
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                /*if (asset.owner.Equals(MainPage.userID) && asset.status.Equals("ON THE WAY"))
                {
                    Town borrower = await getBorrowerAddress(asset.borrower);
                    Button b = new Button();
                    b.Name = asset.id;
                    b.Content = asset.name + "\n" + asset.borrower + "\n"+ borrower.address;
                    b.Click += (s, e) =>
                    {
                        NavigationService.Navigate(new Uri("/AssetPage.xaml?param0=" + asset.id + "&param1=" + "REQUEST DETAIL", UriKind.Relative));
                    };
                    requestedAssets.Children.Add(b);
                }*/
            }
        }

        private void loadMaps()
        {
            throw new NotImplementedException();
        }

        private async Task<Town> getBorrowerAddress(string id)
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
    }
}