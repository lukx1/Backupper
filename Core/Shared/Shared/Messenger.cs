using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
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
        /// <summary>
        /// Nepoužívat pokud klient je používán asynchroně
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; } //TODO: Tohle nemuze byt pouzito v async funkcich

        /// <summary>
        /// Vytvoří instanci a uloží si kontaktní server
        /// </summary>
        /// <param name="serverUrl"></param>
        public Messenger(string serverUrl, bool allowSelfSignedCerts = false)
        {
            if (serverUrl.ToUpper().StartsWith("HTTPS://"))
            {
                WebRequestHandler handler = new WebRequestHandler();
                X509Certificate2 certificate = new X509Certificate2();
                handler.ClientCertificates.Add(certificate);
                client = new HttpClient(handler);
                if (allowSelfSignedCerts)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (s, crt, chn,sslpe) => true;
                }
            }
            else
            {
                client = new HttpClient();
            }
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
        public static T ReadMessage<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }

        /// <summary>
        /// Univerzálně přečte zprávu
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ReadMessageAsDict(string jsonResponse)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
        }

        /// <summary>
        /// Asynchroní odesílání zprávy
        /// </summary>
        /// 
        ///     Exceptiony:
        ///         HTTP:
        ///             V případu chyby HTTP hází Aggregate Exception který
        ///             obsahuje HttpRequestException který obsahuje SocketException
        ///         INet:
        ///             Pokud je status kód špatný (není v rozmezí 200-299) je hozen
        ///             INetException
        ///         JSon:
        ///             Pokud je špatně zadán generický parametr může být hozen
        ///             JsonException
        /// <param name="message">Zpráva k odeslání</param>
        /// <param name="controller">Jméno kontroleru</param>
        /// <param name="httpMethod">Druh http zprávy</param>>
        /// 
        /// <returns>Odpověď serveru</returns>
        public async Task<ServerMessage<TResponse>> SendAsync<TResponse>(INetMessage message, string controller, HttpMethod httpMethod)
        {
            var json = JsonConvert.SerializeObject(message);
            HttpResponseMessage reponse = await client.SendAsync(new HttpRequestMessage(httpMethod, "api/" + controller) { Content = new StringContent(json, Encoding.UTF8, "application/json") });
            var serverJson = await reponse.Content.ReadAsStringAsync();
            var resp = new ServerMessage<TResponse>(serverJson, (int)reponse.StatusCode);
            if (!IsSuccessStatusCode((int)resp.StatusCode))
            {
                if (resp.ErrorMessages.Count > 0)
                    throw new INetException<TResponse>(resp, resp.ErrorMessages[0].message, resp.ErrorMessages);
                else
                    throw new INetException<TResponse>(resp, "Neúspěch", resp.ErrorMessages);
            }
            return resp;
        }

        public bool IsSuccessStatusCode(int code) => code >= 200 && code < 300;

        public class ServerMessage<T>
        {
            public T ServerResponse;

            public HttpStatusCode StatusCode { get; private set; }

            public List<ErrorMessage> ErrorMessages
            {
                get
                {
                    if (errorMessages == null) // Send nemusí kontrolovat pro null
                        return new List<ErrorMessage>();
                    return errorMessages;
                }
            }
            private List<ErrorMessage> errorMessages = new List<ErrorMessage>();

            public ServerMessage(string serverResponseJson,int statusCode)
            {
                this.StatusCode = (HttpStatusCode)statusCode;
                this.ServerResponse = JsonConvert.DeserializeObject<T>(serverResponseJson);
                if (ServerResponse is INetError)
                    this.errorMessages = ((INetError)ServerResponse).ErrorMessages;
            }
        }

    }
}
