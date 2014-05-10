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

namespace pewpewmobileapp
{

    public partial class MainPage : PhoneApplicationPage
    {
        public static string userID = "";
        public static string userType = "";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationService != null)
                while (NavigationService.CanGoBack)
                    NavigationService.RemoveBackEntry();
        }
        /*
        private async void loginAccount(object sender, RoutedEventArgs e)
        {
            int count = 0;
            //btnLogin.IsEnabled = false;

            try
            {
                if (txtUsername.Text.Equals("admin") && txtPassword.Text.Equals("admin"))
                {
                    //NavigationService.Navigate(new Uri("/AdminPage.xaml", UriKind.Relative));
                    userID = "1";
                    userType = "ADMIN";
                    txtMessage.Text = "LOGIN ADMIN";
                    count++;
                }
                else
                {
                    var results = App.MobileService.GetTable<Town>();
                    List<Town> filter = await results.ToListAsync();
                    foreach (Town a in filter)
                    {
                        if (a.email.Equals(txtUsername.Text) && a.password.Equals(txtPassword.Text))
                        {
                            userID = a.id;
                            userType = "TOWN";
                            count++;
                            break;
                        }
                    }
                    if (userType == "TOWN")
                        //NavigationService.Navigate(new Uri("/TownPage.xaml", UriKind.Relative));
                        txtMessage.Text = "LOGIN TOWN";
                    else
                    {
                        var results2 = App.MobileService.GetTable<Organization>();
                        List<Organization> filter2 = await results2.ToListAsync();
                        foreach (Organization a in filter2)
                        {
                            if (a.email.Equals(txtUsername.Text) && a.password.Equals(txtPassword.Text))
                            {
                                userID = a.id;
                                userType = "ORG";
                                count++;
                                break;
                            }
                        }
                        if (userType == "ORG")
                            //NavigationService.Navigate(new Uri("/OrgPage.xaml", UriKind.Relative));
                            txtMessage.Text = "LOGIN ORG";
                    }
                }

                if (count == 0)
                    txtMessage.Text = "ERROR";

            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("NO");
            }
        }
         */ 
    }
}