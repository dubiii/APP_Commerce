using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using APP_Commerce.Models;
using APP_Commerce.Services;
using GalaSoft.MvvmLight.Command;

namespace APP_Commerce.ViewModels
{
    public class CustomerItemViewModel : Customer
    {
        private NavigationService navigationService;
        private DataService dataService;
        private ApiService apiService;
        private NetService netService;

        public ObservableCollection<DepartmentItemViewModel> Departments { get; set; }
        public ObservableCollection<CityItemViewModel> Cities { get; set; }

        public CustomerItemViewModel()
        {
            navigationService = new NavigationService();
            dataService = new DataService();
            apiService = new ApiService();
            netService = new NetService();
            Departments = new ObservableCollection<DepartmentItemViewModel>();
            Cities = new ObservableCollection<CityItemViewModel>();
            LoadDepartments();
            LoadCities();
        }

        private async void LoadCities()
        {
            var cities = new List<City>();

            if (netService.IsConnected())
            {
                cities = await apiService.GetCities();
                dataService.Save(cities);
            }
            else
            {
                cities = dataService.Get<City>(true);
            }

            reloadCities(cities);
        }


        private async void LoadDepartments()
        {
            var departments = new List<Department>();

            if (netService.IsConnected())
            {
                departments = await apiService.GetDepartments();
                dataService.Save(departments);
            }
            else
            {
                departments = dataService.Get<Department>(true);
            }

            reloadDepartments(departments);
        }

        private void reloadDepartments(List<Department> departments)
        {
            Departments.Clear();
            foreach (var department in departments.OrderBy(p => p.Name))
            {
                Departments.Add(new DepartmentItemViewModel
                {
                    DepartmentId = department.DepartmentId,
                    Name = department.Name,
                    Customers = department.Customers,
                    Cities = department.Cities,

                });
            }
        }

        private void reloadCities(List<City> cities)
        {
            Cities.Clear();
            foreach (var city in cities.OrderBy(p => p.Name))
            {
                Cities.Add(new CityItemViewModel
                {
                    CityId = city.CityId,
                    DepartmentId = city.DepartmentId,
                    Department = city.Department,
                    Name = city.Name,
                    Customers = city.Customers,
                });
            }
        }

        public ICommand CustomerDetailCommand { get { return new RelayCommand(CustomerDetail); } }
        private async void CustomerDetail()
        {
            var customerItem = new CustomerItemViewModel
            {
                CustomerId = CustomerId,
                FirstName = FirstName,
                City = City,
                Department = Department,
                CityId = CityId,
                DepartmentId = DepartmentId,
                LastName = LastName,
                Address = Address,
                Latitude = Latitude,
                Longitude = Longitude,
                Photo = Photo,
                Phone = Phone,
                UserName = UserName,
            };

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.setCurrentCustomer(customerItem);
            await navigationService.Navigate("CustomerDetailPage");
        }
    }

}

