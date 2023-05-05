using Android.Content;
using IBF = MauiApp8.Services.BackgroundServices;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.Messaging;

namespace MauiApp8.Platforms.Android.AndroidReceiver
{
    [BroadcastReceiver(Enabled = true)]
    public class BackgroundFetchReceiver : BroadcastReceiver
    {
        public override async void OnReceive(Context context, Intent intent)
        {
            await Task.Run(async () => await UpdateBackgroundData("https://oskarnightscoutweb1.azurewebsites.net/"));
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
            if(response == 0) { WeakReferenceMessenger.Default.Send(new Alarms.Alarm { Response = response, Message = "No fetching of Data...", On = true }); }
            if (response == 1) { WeakReferenceMessenger.Default.Send(new Alarms.Alarm { Response = response, Message = "No fetching Insulin Data...", On = true }); }
            if (response == 2) { WeakReferenceMessenger.Default.Send(new Alarms.Alarm { Response = response, Message = "No fetching Glucose Data...", On = true }); }
            float? latestGlucose = await ibf.ReadLatestGlucoseValue();
            if (Int32.TryParse(latestGlucose.ToString(), out int last))
            {
                if (latestGlucose != 0) { if (latestGlucose > 180 || latestGlucose < 55) { WeakReferenceMessenger.Default.Send(new Alarms.Alarm { Response = last, Message = "Glucose Value is not within acceptable range...", On = true }); } }
            }
            else
            {
                Console.WriteLine($"Int32.TryParse could not parse '{latestGlucose.ToString()}' to an int.");
            }
           
        }
    }
}
