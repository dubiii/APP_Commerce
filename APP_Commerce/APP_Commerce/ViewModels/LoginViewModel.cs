using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APP_Commerce.Models;
using APP_Commerce.Services;
using GalaSoft.MvvmLight.Command;

namespace APP_Commerce.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private NavigationService navigationService;
        private MessageService messageService;
        private ApiService apiService;
        private bool isRunning;
        private DataService dataService;
        private NetService netService;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Props
        public string User { get; set; }
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
        public bool IsRunning
        {
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
            get
            {
                return isRunning;
            }
        }
        #endregion

        #region ctors
        public LoginViewModel()
        {
            navigationService = new NavigationService();
            messageService = new MessageService();
            apiService = new ApiService();
            IsRemembered = true;
            IsRunning = false;
            dataService = new DataService();
            netService = new NetService();
        }
        #endregion

        #region commandos y metodos
        public ICommand LoginCommand { get { return new RelayCommand(Login); } }

        private async void Login()
        {
            if (string.IsNullOrEmpty(User))
            {
                await messageService.Message("Error", "Necesitas ingresar un usuario");
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await messageService.Message("Error", "Necesitas ingresar una contraseña");
                return;
            }

            IsRunning = true;
            var response = new Response();
            if (netService.IsConnected())
            {
                response = await apiService.Login(User, Password);
            }
            else
            {
                response = dataService.Login(User, Password);
            }

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await messageService.Message("Error", response.Message);
                return;
            }

            var user = (User)response.Result;
            user.IsRemembered = IsRemembered;
            user.Password = Password;
            dataService.InsertUser(user);

            navigationService.SetMainPage(user);
        }
        #endregion
    }
}
