//using Praise.Api.Logic.Interfaces;

//namespace Praise.WebApp.Logic.Events.EventBindings
//{
//    public interface ISendPraiseEventBindings
//    {
//        Task Bind();
//    }

//    public class SendPraiseEventBindings(IPraiseLogic _praiseLogic, ISendPraiseReceivedEvent _badgeLogic) : ISendPraiseEventBindings
//    {
//        public async Task Bind()
//        { 
//            _praiseLogic.SendPraisePublished += _badgeLogic.OnSendPraiseReceived;
//        }
//    }
//}
