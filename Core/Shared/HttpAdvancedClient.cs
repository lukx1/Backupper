using Newtonsoft.Json;
using Shared.NetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class HttpAdvancedClient
    {
        

        private INetMessage TranslateResponse(string responseMessage)
        {
            int index = responseMessage.IndexOf('{');
            uint uid = uint.Parse(responseMessage.Substring(0, index));
            JsonConvert.DeserializeObject<int>(responseMessage.Substring(index));
            return null;
        }

        public async Task<string> SendPost(INetMessage message, string controller)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57597/");
            var json = JsonConvert.SerializeObject(message);
            var response = await client.PostAsync("api/"+controller, new StringContent(json));
            var reponseString = await response.Content.ReadAsStringAsync();
            INetMessage result = TranslateResponse(reponseString);
            return reponseString;
        }
    }
}
