/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Linq;
using Core.Framework.Middleware;

namespace Core.Framework.Service
{
    public class UserInfo
    {
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>() { new Claim("role", "system"), new Claim("sub", "1") };
        public ConcurrentDictionary<object, object> Items { get; set; } = new ConcurrentDictionary<object, object>();   
        public IList<string> Roles
        {
            get
            {
                return Claims.Where(c => c.Type.Equals("role")).Select(x => x.Value).ToList();
            }            
        }

        public string Username
        {
            get
            {
                return Claims.Where(c => c.Type.Equals("sub")).Select(x => x.Value).SingleOrDefault();
            }             
        }

        public string Language { get; set; } = "tr-TR";
        public string OrganizationUnit { get; set; }
    }
}
