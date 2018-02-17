﻿using Newtonsoft.Json;
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
    /// Odesílá a přijímá http zprávy
    /// </summary>
    /// Předem známo jako HttpAdvancedClient.
    /// <see cref="Send(INetMessage, string, HttpMethod)"/> na odesílání.
    /// <see cref="ReadMessage{T}"/> na čtení
    public class Messenger
    {
        private HttpClient client;
        private string jsonResponse;
        private HttpStatusCode _statusCode;
        private HttpStatusCode statusCode { get => _statusCode; }

        /// <summary>
        /// Vytvoří instanci a uloží si kontaktní server
        /// </summary>
        /// <param name="serverUrl"></param>
        public Messenger(string serverUrl)
        {
            client = new HttpClient();
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
        /// Univerzálně přečte zprávu
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> ReadMessageAsDict()
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
        }

        /// <summary>
        /// Pošle jakoukoliv zprávu na cílový kontroler a interně jí přečte
        /// </summary>
        /// <param name="message"></param>
        /// <param name="controller"></param>
        public async void Send(INetMessage message, string controller, HttpMethod httpMethod)
        {
            var json = JsonConvert.SerializeObject(message);
            var response = await client.SendAsync(new HttpRequestMessage(httpMethod, "api/" + controller) { Content = new StringContent(json)});
            jsonResponse = await response.Content.ReadAsStringAsync();
            _statusCode = response.StatusCode;
        }

        /// <summary>
        /// Pošle post zprávu na cílový kontroler a interně jí přečte
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
