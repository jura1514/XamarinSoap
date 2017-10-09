using App3.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public  async void Submit_Clicked(object sender, EventArgs e)
        {
            var name = username.Text;
            var pass = password.Text;

            try
            {
                var sessionPassword = await this.Authenticate(name, pass);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<string> Authenticate(string userName, string password)
        {
            var login = new Login { Hdr = GetUniqueHeader() };

            var request = new strLogin { DeviceId = "Device1" };

            login.Request = request;

            try
            {
                var response = await this.LoginAsync(login, userName, password);

                if (response != null && !string.IsNullOrEmpty(response.Result.SessionPassword))
                {
                    return response.Result.SessionPassword;
                }
                else
                {
                    throw new Exception("Error: Service issue!", null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error to sign in!", ex);
            }
        }

        private Task<LoginCompletedEventArgs> LoginAsync(Login login, string userName, string password)
        {
            var client = this.UpdateServiceUrl();

            var tcs = new TaskCompletionSource<LoginCompletedEventArgs>();

            EventHandler<LoginCompletedEventArgs> handler = null;
            handler += (sender, e) =>
            {
                if (e.Error == null)
                {
                    tcs.SetResult(e);
                }
                else
                {
                    tcs.SetException(e.Error);
                }

                client.LoginCompleted -= handler;
            };

            var base64EncodedCredentials = this.EncodeBasicAuthenticationCredentials(userName, password);

            // ReSharper disable once UnusedVariable
            using (var scope = new OperationContextScope(client.InnerChannel))
            {
                var request = this.GetBasicAuthenticationRequest(base64EncodedCredentials);

                OperationContext.Current.OutgoingMessageProperties.Add(HttpRequestMessageProperty.Name, request);

                client.LoginCompleted += handler;
                client.LoginAsync(login);
            }

            return tcs.Task;
        }

        public imcwpPortTypeClient UpdateServiceUrl()
        {
            var serviceUrl = "http://10.0.1.144:29791";
            EndpointAddress serivceUrl = new EndpointAddress(serviceUrl);

            BasicHttpBinding binding = CreateBasicHttp();

            var ascoTmsService = new imcwpPortTypeClient(binding, serivceUrl);
            //var ascoTmsService = new imcwpPortTypeClient("imcwp");
            return ascoTmsService;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "imcwp",
                Namespace = "imcwp",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };

            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        private const string AuthHeader = "Authorization";
        private const string SoapHeader = "SOAPAction";

        protected string EncodeBasicAuthenticationCredentials(string username, string password)
        {
            // first concatenate the user name and password, separated with :
            var credentials = username + ":" + password;

            // Http uses ascii character encoding, WP7 doesn’t include
            // support for ascii encoding but it is easy enough to convert
            // since the first 128 characters of unicode are equivalent to ascii.
            // Any characters over 128 can’t be expressed in ascii so are replaced
            // by ?
            var asciiCredentials = (from c in credentials.ToCharArray()
                                    select c <= 0x7f ? (byte)c : (byte)'?').ToArray();

            // finally Base64 encode the result
            return Convert.ToBase64String(asciiCredentials);
        }

        protected Header GetUniqueHeader()
        {
            var header = new Header
            {
                DestinationId = "tms",
                SenderId = "wp",
                Timestamp = DateTime.UtcNow,
                UniqueKey = Guid.NewGuid().ToString(),
                Version = 1
            };

            return header;
        }

        protected HttpRequestMessageProperty GetBasicAuthenticationRequest(string base64EncodedCredentials)
        {
            var request = new HttpRequestMessageProperty();
            request.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + base64EncodedCredentials;

            return request;
        }
    }
}
