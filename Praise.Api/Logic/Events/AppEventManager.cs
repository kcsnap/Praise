using Praise.Api.Logic.Interfaces;

namespace Praise.WebApp.Logic.Events
{
    public class AppEventManager
    {
        //IHttpContextAccessor contextAccessor;
        IPraiseLogic praiseLogic;

        public AppEventManager(/*IHttpContextAccessor _contextAccessor*/)
        {
            //contextAccessor = _contextAccessor;            
        }

        public async Task Initialise(WebApplication? app)
        {
            //praiseLogic = contextAccessor.HttpContext.RequestServices.GetRequiredService<IPraiseLogic>();
            praiseLogic = app.Services.GetRequiredService<IPraiseLogic>();

            await InitSendPraisePublished(app);
        }

        private async Task InitSendPraisePublished(WebApplication app)
        {
            //var badgeLogic = contextAccessor.HttpContext.RequestServices.GetRequiredService<ISendPraiseReceivedEvent>();
            //var notificationLogic = contextAccessor.HttpContext.RequestServices.GetRequiredService<ISendPraiseReceivedEvent>();
            var SendPraiseEventSubscribers = app.Services.GetRequiredService<IEnumerable<ISendPraiseReceivedEvent>>();

            // when sending praise 1) notify relevant people 2) check for new badges
            foreach(var subscriber in SendPraiseEventSubscribers)
            {
                praiseLogic.SendPraisePublished += subscriber.OnSendPraiseReceived;
            }
            //praiseLogic.SendPraisePublished += notificationLogic.OnSendPraiseReceived;
            //praiseLogic.SendPraisePublished += badgeLogic.OnSendPraiseReceived;
        }
    }
}
