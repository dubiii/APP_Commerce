using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APP_Commerce.Models;
using APP_Commerce.Pages;
using APP_Commerce.Services;
using APP_Commerce.ViewModels;
using Xamarin.Forms;

namespace APP_Commerce
{
    public partial class App : Application
    {
        private DataService dataService;
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }
        public static User CurrentUser { get; internal set; }
        public App()
        {
            InitializeComponent();
            dataService = new DataService();



            var user = dataService.GetUser();
            if (user != null && user.IsRemembered)
            {
                var mainViewModel = MainViewModel.GetInstance(); //modo remember
                mainViewModel.LoadUser(user);
                App.CurrentUser = user;
                MainPage = new MasterPage();
            }
            else
            {
                MainPage = new LoginPage();
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
