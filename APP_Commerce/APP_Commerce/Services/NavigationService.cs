using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_Commerce.Models;
using APP_Commerce.Pages;
using APP_Commerce.ViewModels;

namespace APP_Commerce.Services
{
    public class NavigationService
    {
        private DataService dataService;

        public NavigationService()
        {
            dataService = new DataService();
        }

        public async Task Navigate(string pageName)
        {
            App.Master.IsPresented = false;

            switch (pageName)
            {
                case "ProductsPage":
                    await App.Navigator.PushAsync(new ProductsPage());
                    break;
                case "ClientsPage":
                    await App.Navigator.PushAsync(new CustomerPage());
                    break;
                case "OrdersPage":
                    await App.Navigator.PushAsync(new OrdersPage());
                    break;
                case "DeliveriesPage":
                    await App.Navigator.PushAsync(new DeliveriesPage());
                    break;
                case "SyncsPage":
                    await App.Navigator.PushAsync(new SyncsPage());
                    break;
                case "SetupPage":
                    await App.Navigator.PushAsync(new SetupPage());
                    break;
                case "UserPage":
                    await App.Navigator.PushAsync(new UserPage());
                    break;
                case "CustomerDetailPage":
                    await App.Navigator.PushAsync(new CustomerDetailPage());
                    break;
                case "LogoutPage":
                    Logout();
                    break;
                default:
                    break;
            }

        }

        private void Logout()
        {
            App.CurrentUser.IsRemembered = false;
            dataService.UpdateUser(App.CurrentUser);
            App.Current.MainPage = new LoginPage();

        }

        internal void SetMainPage(User user)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.LoadUser(user);
            App.CurrentUser = user;
            App.Current.MainPage = new MasterPage();
        }

        public User GetCurrentUser()
        {
            return App.CurrentUser;
        }
    }
}
