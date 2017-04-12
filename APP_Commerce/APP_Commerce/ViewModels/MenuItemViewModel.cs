using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APP_Commerce.Services;
using GalaSoft.MvvmLight.Command;

namespace APP_Commerce.ViewModels
{
    public class MenuItemViewModel
    {
        #region propiedades y atributos
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }

        public NavigationService navigationService { get; set; }
        #endregion

        #region Constructores
        public MenuItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commandos
        public ICommand NavigateCommand { get { return new RelayCommand(Navigate); } }

        private async void Navigate()
        {
            await navigationService.Navigate(PageName);
        }
        #endregion
    }
}
