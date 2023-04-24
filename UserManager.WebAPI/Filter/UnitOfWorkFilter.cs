
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using UserManager.WebAPI.Attribute;

namespace UserManager.WebAPI.Filter;

public class UnitOfWorkFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var result = await next();
        //当action没有异常时不调用SaveChange方法
        if (result.Exception != null)
        {
            return;
        }

        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor == null)
        {
            return;
        }

        var attributes = descriptor.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        //当没有发现添加UnitOfWorKAttribute就不处理
        if (attributes == null)
        {
            return;
        }

        foreach (var dbContext in attributes.DbContextTypes)
        {
            //从DI中拿到DBContext实例
            var dbCtx = context.HttpContext.RequestServices.GetService(dbContext) as DbContext;
            if (dbCtx != null)
            {
                await dbCtx.SaveChangesAsync();
            }
        }
    }
}