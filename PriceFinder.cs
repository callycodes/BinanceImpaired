using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace BinanceImpaired
{
    public class PriceFinder
    {
        


        public static async Task Handle() {
            var cryptoToCheck = "";
            while (true) {
                await Speaker.SayText("Please say a crypto slug or abbreviation to check, such as Bitcoin or BTC");
                cryptoToCheck = await Speaker.GetSpeechInput();
                cryptoToCheck = Program.StripPunctuation(cryptoToCheck.ToLower());
                await Speaker.SayText($"You would like to check the price of: {cryptoToCheck}? Yes or no");
                var cont = await Speaker.GetSpeechInput();
                if (cont.ToLower().Contains("yes")) {
                    break;
                }
            }
            var priceInfo = await PriceFinder.GetPrice(cryptoToCheck);
            await Speaker.SayText(priceInfo);
            Console.ReadLine();
        }

        public static async Task<string> GetPrice(string symbol) {
            var response = await MakeRequest(symbol);
            Console.WriteLine(response);
            dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
            var coinSlug = jsonObject.data["1"].name;
            var price = jsonObject.data["1"].quote["USD"].price;
            var output = $"The current price of {coinSlug} is {price} US Dollars";
            return output;
        }

        public static async Task<string> MakeRequest(string symbol) {
            try {

                using var client = new HttpClient();

                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", APIKeys.COINMARKETCAP_KEY);

                Console.WriteLine($"Making request to: https://pro-api.coinmarketcap.com/v2/cryptocurrency/quotes/latest?slug={symbol}");
                var url = $"https://pro-api.coinmarketcap.com/v2/cryptocurrency/quotes/latest?slug={symbol}";
                HttpResponseMessage response = await client.GetAsync(url);
                //response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            } catch (Exception e) {
                return "error";
            }
    }
        
    }
}