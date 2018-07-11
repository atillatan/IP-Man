/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:09:50 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:09:50 
 */

using System;
using Core.Framework.Middleware;

namespace Core.Framework
{
    public interface IApplicationBuilder
    {
        InvokeDelegate Build();
        IApplicationBuilder Use(Func<InvokeDelegate, InvokeDelegate> middleware);
    }
}