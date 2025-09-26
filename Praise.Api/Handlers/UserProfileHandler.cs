//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Options;
//using Praise.Api.Logic.Interfaces;

//namespace Praise.Api.Handlers
//{
//    public class UserProfileAction() : IActionFilter
//    {
//        public void OnActionExecuting(ActionExecutingContext context)
//        {
            

//            // This runs BEFORE the action method
//            Console.WriteLine("Before action execution");
//        }

//        public void OnActionExecuted(ActionExecutedContext context)
//        {
//            // This runs AFTER the action method
//            Console.WriteLine("After action execution");

//            if (context.Exception == null && context.Result is ObjectResult result)
//            {
//                result.Value = new { data = result.Value, wrapped = true };
//            }
//        }
//    }
//}
