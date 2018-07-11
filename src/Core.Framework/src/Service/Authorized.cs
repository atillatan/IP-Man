/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;

namespace Core.Framework.Service
{
 public class Authorized : System.Attribute
    {
        private string[] authorizations;

        public Authorized(params string[] authorizations)
        {
            this.authorizations = authorizations;
        }

        public string[] Authorizations
        {
            get
            {
                return this.authorizations;
            }
        }

    }
}