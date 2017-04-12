using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APP_Commerce.Models;
using Newtonsoft.Json;

namespace APP_Commerce.Services
{
    public class ApiService
    {
        public async Task<Response> Login(string email, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var request = JsonConvert.SerializeObject(loginRequest);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://dubistore.azurewebsites.net");
                var url = "/api/Users/Login";
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Usuario o Contraseña Incorrectos"
                    };
                }

                var result = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(result);

                return new Response
                {
                    IsSuccess = true,
                    Message = "Ingreso OK",
                    Result = user
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://dubistore.azurewebsites.net");
                var url = "/api/Products/Products";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(result);
                return products.OrderBy(p => p.Description).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Product>> GetProductSearch(string filter)
        {

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://dubistore.azurewebsites.net");
                var url = "/api/Products/Products";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(result);
                return products.OrderBy(p => p.Description).Where(p => p.Description.ToUpper().Contains(filter.ToUpper())).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Customer>> GetCustomerSearch(string filter)
        {

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://dubistore.azurewebsites.net");
                var url = "/api/Customers";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(result);
                return customers.OrderBy(p => p.FirstName).Where(p => p.FirstName.ToUpper().Contains(filter.ToUpper()) || p.LastName.ToUpper().Contains(filter.ToUpper())).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<Customer>> GetCustomers()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://dubistore.azurewebsites.net");
                var url = "/api/Customers";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(result);
                return customers.OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
