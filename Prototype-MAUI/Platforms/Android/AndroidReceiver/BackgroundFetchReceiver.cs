using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using PS = MauiApp8.Services.PublishSubscribeService;
using Microsoft.Toolkit.Mvvm.Messaging;
using MauiApp8.Services.BackgroundServices;
using IBF = MauiApp8.Services.BackgroundServices;
using MauiApp8.Model;
namespace MauiApp8.Platforms.Android.AndroidReceiver
{
    [BroadcastReceiver(Enabled = true)]
    public class BackgroundFetchReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            await UpdateBackgroundData("https://oskarnightscoutweb1.azurewebsites.net/");
            Console.WriteLine("Testing Background fetch...");


            // Implement youribf background fetch logic here
            IBF.IBackgroundService ibf = new IBF.DataBase();
            
           

            WeakReferenceMessenger.Default.Send(new Fetch.Update_Google { Response = 8 });
            WeakReferenceMessenger.Default.Send(new Fetch.Update_Health { Response = 101 });
        }

        public async Task UpdateBackgroundData(string DomainName)
        {
            int response = 0;
            IBF.IBackgroundService ibf = new IBF.DataBase();
            await ibf.UpdateGlucose(DomainName);
            try
            {
                response += 1;
                // handle the result
            }
            catch (Exception ex)
            {
                Console.WriteLine("Glucose Uptdate Issue:  " + ex);
            }
            await ibf.UpdateInsulin(DomainName);
            try
            {
                response += 2;
                // handle the result
            }
            catch (Exception ex)
            {
                Console.WriteLine("Glucose Uptdate Issue:  " + ex);
            }

            WeakReferenceMessenger.Default.Send(new Fetch.UpdateResponse { Response = 200 });
        }
    }
}
