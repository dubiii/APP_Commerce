using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_Commerce.Data;
using APP_Commerce.Models;

namespace APP_Commerce.Services
{
    public class DataService
    {
        public User GetUser()
        {
            using (var da = new DataAccess())
            {
                return da.First<User>(false);
            }
        }

        public Response UpdateUser(User user)
        {
            try
            {
                using (var data = new DataAccess())
                {
                    data.Update(user);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario actualizado",
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

        public Response InsertUser(User user)
        {
            try
            {
                using (var data = new DataAccess())
                {
                    var oldUser = data.First<User>(false);
                    if (oldUser != null)
                    {
                        data.Delete(oldUser);
                    }
                    data.Insert(user);
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Usuario insertado",
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

        //public void SaveProducts(List<Product> products)
        //{
        //    using (var da = new DataAccess())
        //    {
        //        var oldProducts = da.GetList<Product>(false);
        //        foreach (var product in oldProducts)
        //        {
        //            da.Delete(product);
        //        }

        //        foreach (var product in products)
        //        {
        //            da.Insert(product);
        //        }
        //    }
        //}

        public void Save<T>(List<T> list) where T : class
        {
            using (var da = new DataAccess())
            {
                var oldRecords = da.GetList<T>(false);
                foreach (var record in oldRecords)
                {
                    da.Delete(record);
                }

                foreach (var record in list)
                {
                    da.Insert(record);
                }
            }
        }


        //public List<Product> GetProducts()
        //{
        //    using (var da = new DataAccess())
        //    {
        //        return da.GetList<Product>(true).OrderBy(p => p.Description).ToList();
        //    }
        //}

        //public List<Customer> GetCustomers()
        //{
        //    using (var da = new DataAccess())
        //    {
        //        return da.GetList<Customer>(true).OrderBy(p => p.FirstName).ThenBy(p => p.LastName).ToList();
        //    }
        //}

        public List<T> Get<T>(bool withChildren) where T : class
        {
            using (var da = new DataAccess())
            {
                return da.GetList<T>(withChildren).ToList(); // no es necesario agregar los contains porque solo es el primer GET
            }
        }

        public Response Login(string usuario, string password)
        {
            try
            {
                using (var da = new DataAccess())
                {
                    var user = da.First<User>(true);
                    if (user == null)
                    {
                        return new Response
                        {
                            IsSuccess = false,
                            Message = "No hay conexion a Internet y no hay usuario registrado"
                        };
                    }

                    if (user.UserName.ToUpper() == usuario.ToUpper() && user.Password == password)
                    {
                        return new Response
                        {
                            IsSuccess = true,
                            Message = "Ingreso OK",
                            Result = usuario
                        };
                    }

                    return new Response
                    {
                        IsSuccess = false,
                        Message = "usuario o contraseña incorrectos"
                    };

                }

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


    }
}
