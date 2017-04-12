using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_Commerce.Interfaces;
using Xamarin.Forms;

namespace APP_Commerce.Services
{
    public class NetService
    {
        public bool IsConnected()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckNetworkConnection();
            return networkConnection.IsConnected;
        }
    }
}
