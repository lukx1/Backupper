using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    /// <summary>
    /// Odesílá a přijímá http zprávy
    /// </summary>
    /// Předem známo jako HttpAdvancedClient.
    /// <see cref="Send(INetMessage, string, HttpMethod)"/> na odesílání.
    /// <see cref="ReadMessage{T}"/> na čtení
    public class Messenger
    {
        private HttpClient client;
        private string jsonResponse;
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Vytvoří instanci a uloží si kontaktní server
        /// </summary>
        /// <param name="serverUrl"></param>
        public Messenger(string serverUrl)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
            client.BaseAddress = new Uri(serverUrl);

        }

        /// <summary>
        /// Přečte zprávu genericky
        /// </summary>
        /// Není možné přeložit pomocí interfacu. T musí být identický 
        /// s přijmutým jsonem
        /// <typeparam name="T"> result message</typeparam>
        /// <returns>Message</returns>
        public T ReadMessage<T>()
        {
            return ReadMessage<T>(jsonResponse);
            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }

        /// <summary>
        /// Přečte zprávu genericky
        /// </summary>
        /// Není možné přeložit pomocí interfacu. T musí být identický 
        /// s přijmutým jsonem
        /// <typeparam name="T"> result message</typeparam>
        /// <returns>Message</returns>
        public T ReadMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Pokud je kód vetší nebo rovno 200 a menší než 300 vrací true jinak false
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessStatusCode()
        {
            return ((int)StatusCode >= 200 && (int)StatusCode < 300);
        }

        /// <summary>
        /// Univerzálně přečte zprávu
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ReadMessageAsDict()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
        }

        /// <summary>
        /// Asynchroní odesílání zprávy
        /// </summary>
        /// 
        ///     Exceptiony:
        ///         V případu chyby HTTP hází Aggregate Exception který
        ///         obsahuje HttpRequestException který obsahuje SocketException
        ///         
        /// <param name="message">Zpráva k odeslání</param>
        /// <param name="controller">Jméno kontroleru</param>
        /// <param name="httpMethod">Druh http zprávy</param>>
        /// 
        /// <returns>Odpověď v Jsonu</returns>
        public async Task<string> SendAsyncGetJson(INetMessage message, string controller, HttpMethod httpMethod)
        {
            var json = JsonConvert.SerializeObject(message);
            HttpResponseMessage reponse = await client.SendAsync(new HttpRequestMessage(httpMethod, "api/" + controller) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
            return await reponse.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Synchroní odeslání zprávy
        /// </summary>
        /// 
        ///     Exceptiony:
        ///         V případu chyby HTTP hází Aggregate Exception který
        ///         obsahuje HttpRequestException který obsahuje SocketException    
        ///         
        /// <param name="message">Zpráva k odeslání</param>
        /// <param name="controller">Jméno kontroleru</param>
        /// <param name="httpMethod">Druh http zprávy</param>>
        public void Send(INetMessage message, string controller, HttpMethod httpMethod)
        {
            var json = JsonConvert.SerializeObject(message);
            Task<HttpResponseMessage> sendTask = client.SendAsync(new HttpRequestMessage(httpMethod, "api/" + controller) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
            sendTask.Wait();
            var responseMessage = sendTask.Result;
            Task<string> readTask = responseMessage.Content.ReadAsStringAsync();
            readTask.Wait();
            this.jsonResponse = readTask.Result;
        }


    }
}
