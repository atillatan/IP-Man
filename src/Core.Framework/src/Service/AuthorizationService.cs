using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Framework.Service
{
    public class AuthorizationService
    {
        private static readonly IDictionary<string, string[]> aclCache = new Dictionary<string, string[]>();

        public IDictionary<string, string[]> ACL
        {
            get { return aclCache; }
        }

        public AuthorizationService(Action<IDictionary<string, string[]>> setupAction)
        {
            setupAction.Invoke(aclCache);
        }

        public bool isAuthorized(Authorized authorizedAttributes, ServiceContext serviceContext)
        {
            return isAuthorized(authorizedAttributes.Authorizations, serviceContext);
        }

        public bool isAuthorized(string authorizedKey, ServiceContext serviceContext)
        {
            return isAuthorized(new string[] { authorizedKey }, serviceContext);
        }

        public bool isAuthorized(string[] authorizedKeys, ServiceContext serviceContext)
        {
            if (authorizedKeys == null) return false;

            if (authorizedKeys.Contains("UNAUTHORIZED")) return true;


            if (serviceContext?.UserInfo?.Roles != null)
            {

                string[] userRoleCodes = serviceContext.UserInfo.Roles.ToArray();

                if (userRoleCodes.Contains("system")) return true;

                var resultlist = aclCache.Values.Where(acl => (
                                                                  authorizedKeys.Contains(acl[0]) &&
                                                                  userRoleCodes.Contains(acl[1])
                                                                )
                                                       ).ToList();

                return resultlist.Count() > 0;
            }

            return false;
        }
    }

    public class AuthorizationOptions
    {
        public string RoleSystem { get; set; }
        public string RoleAdmin { get; set; }
        public string RoleAnonymous { get; set; }

    }

}