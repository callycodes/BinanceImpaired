using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Text;

namespace BinanceImpaired
{
    class Program
    {
        
        
        public static SpeechConfig speechConfig = null;

    
    public static string StripPunctuation(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
        {
            if (!char.IsPunctuation(c))
                sb.Append(c);
        }
        return sb.ToString();
    }

    async static Task Main(string[] args)
    {
        speechConfig = SpeechConfig.FromSubscription(APIKeys.AZURE_SUBSCRIPTION_KEY, APIKeys.AZURE_REGION);
        await Speaker.SayText("Welcome to Trading Impaired");
        await Speaker.SayText("The options the software offers will be listed in just a moment. If you'd like to learn how to use a function, simply select the function, then say 'HOW DOES THIS WORK'.");
        await Speaker.SayText(
            "Option 1: Check Price. " +
            "Option 2: Get trade info for a cryptocurrency pair. " +
            "You can also say. Settings. or. QUIT.");
 
        
        var input = "";
        while (!input.Equals("quit")) {
            await Speaker.SayText("Say your option now");
            input = await Speaker.GetSpeechInput();
            input = StripPunctuation(input.ToLower());
            Console.WriteLine("Found input: " + input);

            switch (input) {
                case "one":
                case "1":
                    await PriceFinder.Handle();
                break;
                case "quit":
                    return;
            }
        }
        //await FromMic(speechConfig);
    }


    }
}
