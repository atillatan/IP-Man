/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:05 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:05 
 */
 
using System;
using System.Threading.Tasks;

namespace Core.Framework.Middleware
{
    public class InvokeMiddleware
    {
        private readonly InvokeDelegate _next;

        public InvokeMiddleware(InvokeDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(InvokeContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            object result;
            result = context.Request.Function.Compile()();
            context.Result.Value = result;
            await _next.Invoke(context);
        }
    }

}