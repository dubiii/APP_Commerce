using APP_Commerce.Models;
using APP_Commerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace APP_Commerce.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Propiedades y atributos

        private DataService dataService;
        private ApiService apiService;
        private NetService netService;
        private string productsFilter;
        private string customerFilter;
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }

        public ObservableCollection<ProductItemViewModel> Products { get; set; }
        public ObservableCollection<CustomerItemViewModel> Customers { get; set; }

        public LoginViewModel newLogin { get; set; }

        public UserViewModel userLogged { get; set; }
        public string ProductsFilter
        {
            set
            {
                if (productsFilter != value)
                {
                    productsFilter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductsFilter"));
                    if (string.IsNullOrEmpty(productsFilter))
                    {
                        LoadLocalProducts();
                    }


                }
            }
            get
            {
                return productsFilter;
            }
        }

        public string CustomerFilter
        {
            set
            {
                if (customerFilter != value)
                {
                    customerFilter = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CustomerFilter"));
                    if (string.IsNullOrEmpty(customerFilter))
                    {
                        LoadLocalCustomers();
                    }


                }
            }
            get
            {
                return customerFilter;
            }
        }

        #endregion

        #region Constructores


        static MainViewModel instance;
        public event PropertyChangedEventHandler PropertyChanged;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }


        public MainViewModel()
        {
            instance = this; // singleton
            Menu = new ObservableCollection<MenuItemViewModel>();
            Products = new ObservableCollection<ProductItemViewModel>();
            Customers = new ObservableCollection<CustomerItemViewModel>();
            newLogin = new LoginViewModel();
            userLogged = new UserViewModel();
            dataService = new DataService();
            apiService = new ApiService();
            netService = new NetService();
            LoadMenu();
            LoadProducts();
            LoadCustomers();
        }







        #endregion

        #region Metodos
        private void LoadMenu()
        {
            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_shopping_cart.png",
                PageName = "ProductsPage",
                Title = "Productos",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_contacts.png",
                PageName = "ClientsPage",
                Title = "Clientes",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_event_available.png",
                PageName = "OrdersPage",
                Title = "Pedidos",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_folder_open.png",
                PageName = "DeliveriesPage",
                Title = "Entregas",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_restore.png",
                PageName = "SyncsPage",
                Title = "Sincronizar",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_settings.png",
                PageName = "SetupPage",
                Title = "Configuracion",
            });

            Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_action_directions_run.png",
                PageName = "LogoutPage",
                Title = "Logout",
            });

        }

        public void LoadUser(User user)
        {

            userLogged.FullName = user.FullName;
            userLogged.Photo = user.PhotoFullPath;
        }

        private async void LoadLocalProducts()
        {
            var products = await apiService.GetProducts();
            reloadProducts(products);
        }

        private async void LoadProducts()
        {

            var products = new List<Product>();

            if (netService.IsConnected())
            {
                products = await apiService.GetProducts();
                dataService.Save(products);
            }
            else
            {
                products = dataService.Get<Product>(true);
            }

            reloadProducts(products);

        }

        private void reloadProducts(List<Product> products)
        {
            Products.Clear();
            foreach (var product in products.OrderBy(p => p.Description))
            {
                Products.Add(new ProductItemViewModel
                {
                    BarCode = product.BarCode,
                    Category = product.Category,
                    CategoryId = product.CategoryId,
                    Company = product.Company,
                    CompanyId = product.CompanyId,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    Remarks = product.Remarks,
                    Stock = product.Stock,
                    Impuesto = product.Impuesto,
                    ImpuestoId = product.ImpuestoId,

                });
            }
        }

        private void reloadCustomers(List<Customer> customers)
        {
            Customers.Clear();
            foreach (var customer in customers.OrderBy(c => c.FirstName).ThenBy(c => c.LastName))
            {
                Customers.Add(new CustomerItemViewModel
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    City = customer.City,
                    Department = customer.Department,
                    CityId = customer.CityId,
                    DepartmentId = customer.DepartmentId,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Photo = customer.Photo,
                    Phone = customer.Phone,
                    UserName = customer.UserName,

                });

            }
        }

        private async void LoadLocalCustomers()
        {
            var customers = await apiService.GetCustomers();
            reloadCustomers(customers);
        }

        private async void LoadCustomers()
        {
            var customers = new List<Customer>();

            if (netService.IsConnected())
            {
                customers = await apiService.GetCustomers();
                dataService.Save(customers);
            }
            else
            {
                customers = dataService.Get<Customer>(true);
            }

            reloadCustomers(customers);
        }


        #endregion

        #region Commands
        public ICommand SearchProductCommand { get { return new RelayCommand(SearchProduct); } }
        public ICommand SearchCustomerCommand { get { return new RelayCommand(SearchCustomer); } }

        private async void SearchCustomer()
        {
            var customers = await apiService.GetCustomerSearch(CustomerFilter);
            Customers.Clear();
            foreach (var customer in customers)
            {
                Customers.Add(new CustomerItemViewModel
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    City = customer.City,
                    Department = customer.Department,
                    CityId = customer.CityId,
                    DepartmentId = customer.DepartmentId,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Photo = customer.Photo,
                    Phone = customer.Phone,
                    UserName = customer.UserName,

                });

            }
        }

        private async void SearchProduct()
        {
            var products = await apiService.GetProductSearch(ProductsFilter);

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(new ProductItemViewModel
                {

                    BarCode = product.BarCode,
                    Category = product.Category,
                    CategoryId = product.CategoryId,
                    Company = product.Company,
                    CompanyId = product.CompanyId,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    ProductId = product.ProductId,
                    Remarks = product.Remarks,
                    Stock = product.Stock,
                    Impuesto = product.Impuesto,
                    ImpuestoId = product.ImpuestoId,
                });
            }

        }
        #endregion
    }
}
