using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Předem známo jako HttpAdvancedClient
    /// </summary>
    public class Messenger
    {
        private HttpClient client;
        private string jsonResponse;
        private HttpStatusCode _statusCode;
        private HttpStatusCode statusCode { get => _statusCode; }
        /// <summary>
        /// Vytvoří instanci a uloží si kontaktní server
        /// </summary>
        /// <param name="server"></param>
        public Messenger(string server)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(server);
        }

        /// <summary>
        /// Reads the message using generics
        /// </summary>
        /// <typeparam name="T"> result message</typeparam>
        /// <returns>Message</returns>
        public T ReadMessage<T>()
        {
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }

        /// <summary>
        /// Pokud je kód vetší nebo rovno 200 a menší než 300 vrací true jinak false
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessStatusCode()
        {
            return ((int)statusCode >= 200 && (int)statusCode < 300);
        }

        /// <summary>
        /// Univerzálně přečte zprávu bez překladu do INetMessage
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> ReadMessageAsDict()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
        }

        /// <summary>
        /// Pošle zprávu na cílový kontroler a interně jí přečte
        /// </summary>
        /// <param name="message"></param>
        /// <param name="controller"></param>
        public async void SendPost(INetMessage message, string controller)
        {
            var json = JsonConvert.SerializeObject(message);
            var response = await client.PostAsync("api/"+controller, new StringContent(json));
            jsonResponse = await response.Content.ReadAsStringAsync();
            _statusCode = response.StatusCode;
        }
    }
}
