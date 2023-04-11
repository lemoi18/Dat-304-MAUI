using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Oauth2.v2;
using MauiApp8.Services.BackgroundServices;
using Microsoft.Toolkit.Mvvm.Messaging; 
using CommunityToolkit.Mvvm;



namespace MauiApp8.Services.PublishSubscribeService
{
    public class Publish
    {
        internal readonly IBackgroundService _backgroundService;

        internal Publish(IBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
        }
        
        public class UpdateResponse
        {
            public int Response { get; set; }
        }

        public class Alarm
        {
            public int Response { get; set; }
            public bool On { get; set; } = false;
        }

        public async Task UpdateBackgroundData(string DomainName) 
        {
            int response = 0;
            await _backgroundService.UpdateGlucose(DomainName);
                try
            {
                response += 1;
                // handle the result
            }
            catch (Exception ex)
            {
                Console.WriteLine("Glucose Uptdate Issue:  " + ex);
            }
            await _backgroundService.UpdateInsulin(DomainName);
            try
            {
                response += 2;
                // handle the result
            }
            catch (Exception ex)
            {
                Console.WriteLine("Glucose Uptdate Issue:  " + ex);
            }

            WeakReferenceMessenger.Default.Send(new UpdateResponse { Response = response });
        }

        public async Task Turn_On() 
        {
            WeakReferenceMessenger.Default.Send(new Alarm { Response = 0, On = true}); }

        public async Task Turn_Off()
        {
            WeakReferenceMessenger.Default.Send(new Alarm { Response = 0, On = false });
        }
        public async Task CheckSubscribe() 
        {
            WeakReferenceMessenger.Default.Register<UpdateResponse>(this, (sender, message) =>
            {
                Console.WriteLine(message.Response);
                Console.WriteLine(sender);
                Console.WriteLine("Updated Data...");
            });

        }

    }
}
