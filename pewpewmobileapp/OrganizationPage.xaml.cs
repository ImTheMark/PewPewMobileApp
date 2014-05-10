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

        private async void loadRequestList()
        {
            var list = await App.MobileService.GetTable<Asset>().ToListAsync();
            foreach (Asset asset in list)
            {
                if (asset.owner.Equals(MainPage.userID) && asset.status.Equals("ON THE WAY"))
                {
                    Button b = new Button();
                    b.Name = asset.id;
                    b.
                }
            }
        }
    }
}