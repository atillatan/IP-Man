/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Framework.Middleware;
using Core.Framework.Repository;

namespace Core.Framework.Service
{
    public interface IProxyService
    {
        object Instance { get; }
        Expression<Func<object>> ExpFunc { get; }
        MethodCallExpression MethodCallExpression { get; }
    }

    public class ProxyService<TService> : IProxyService
    {
        TService service;

        public MethodCallExpression MethodCallExpression
        {
            get;
            private set;
        }

        public Expression<Func<object>> ExpFunc
        {
            get;
            private set;
        }

        public object Instance => service;

        public ProxyService(TService service)
        {
            this.service = service;
        }

        public async Task<ServiceResponse<TResult>> RunAsync<TResult>(Expression<Func<TService, ServiceResponse<TResult>>> instanceFunction)
        {
            MethodCallExpression = instanceFunction.Body as MethodCallExpression;
            ExpFunc = () => instanceFunction.Compile()(service);
            InvokeContext context = new InvokeContext(this);

            try
            {
                await Application.Current.app.Invoke(context);

                if (context?.Result?.Value.GetType() == typeof(ServiceResponse<TResult>))
                {
                    return context.Result.Value as ServiceResponse<TResult>;
                }
                else
                {
                    return new ServiceResponse<TResult>(true, ResultType.Success, string.Empty, (TResult)context.Result.Value);
                }
            }
            catch (ServiceException se)
            {
                switch (se.ExceptionType)
                {
                    case ExceptionType.Error:
                        return new ServiceResponse<TResult>(false, ResultType.Error, se.Message);
                    case ExceptionType.Warning:
                        return new ServiceResponse<TResult>(false, ResultType.Warning, se.Message);
                    case ExceptionType.Information:
                        return new ServiceResponse<TResult>(false, ResultType.Information, se.Message);
                    default:
                        return new ServiceResponse<TResult>(false, ResultType.Error, se.Message);
                }
            }
            catch (Exception ext)
            {
                throw new ServiceException("unhandled exception has occurred in your application! look InnerException for details", ext);
            }
        }

        public async Task<TResult> RunAsync<TResult>(Expression<Func<TService, TResult>> instanceFunction)
        {
            MethodCallExpression = instanceFunction.Body as MethodCallExpression;
            ExpFunc = () => instanceFunction.Compile()(service);
            InvokeContext context = new InvokeContext(this);

            try
            {
                await Application.Current.app.Invoke(context);
                return (TResult)context.Result.Value;
            }
            catch (Exception ext)
            {
                throw new ServiceException("unhandled exception has occurred in your application! look InnerException for details", ext);
            }
        }

        public  TResult Run<TResult>(Expression<Func<TService, TResult>> instanceFunction)
        {
            MethodCallExpression = instanceFunction.Body as MethodCallExpression;
            ExpFunc = () => instanceFunction.Compile()(service);
            InvokeContext context = new InvokeContext(this);

            try
            {
                Application.Current.app.Invoke(context);
                return (TResult)context.Result.Value;
            }
            catch (Exception ext)
            {
                throw new ServiceException("unhandled exception has occurred in your application! look InnerException for details", ext);
            }
        }
    }

}