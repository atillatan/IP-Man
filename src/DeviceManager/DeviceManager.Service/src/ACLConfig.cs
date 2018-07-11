using System.Collections.Generic;
using Core.Framework.Service;
using Core.Framework.Util;

namespace DeviceManager.Service
{
    public class ACLConfig
    {        public static List<dynamic> GetAcl()
        {
            string RoleSystem = ConfigManager.Get<string>("role.system", "1c86feaa-5db6-4c49-b1d2-96502ac7208c");
            string RoleAdmin = ConfigManager.Get<string>("role.admin", "66007699-aa3e-4e13-843f-31f2db97905c");
            string RoleAnonymous = ConfigManager.Get<string>("role.anonymous", "Anonymous");

            return new List<dynamic>
            {
                // SETTINGS_INSERT
                new { PermissionCode = "SETTINGS_INSERT", RoleId = RoleSystem },
                new { PermissionCode = "SETTINGS_INSERT", RoleId = RoleAdmin },
                new { PermissionCode = "SETTINGS_INSERT", RoleId = RoleAnonymous },

                // SETTINGS_GET
                new { PermissionCode = "SETTINGS_GET", RoleId = RoleSystem },
                new { PermissionCode = "SETTINGS_GET", RoleId = RoleAdmin },
                new { PermissionCode = "SETTINGS_GET", RoleId = RoleAnonymous },
                
                // SETTINGS_UPDATE
                new { PermissionCode = "SETTINGS_UPDATE", RoleId = RoleSystem },
                new { PermissionCode = "SETTINGS_UPDATE", RoleId = RoleAdmin },
                new { PermissionCode = "SETTINGS_UPDATE", RoleId = RoleAnonymous },
                
                // SETTINGS_DELETE
                new { PermissionCode = "SETTINGS_DELETE", RoleId = RoleSystem },
                new { PermissionCode = "SETTINGS_DELETE", RoleId = RoleAdmin },
                new { PermissionCode = "SETTINGS_DELETE", RoleId = RoleAnonymous },

                // SETTINGS_LIST
                new { PermissionCode = "SETTINGS_LIST", RoleId = RoleSystem },
                new { PermissionCode = "SETTINGS_LIST", RoleId = RoleAdmin },
                new { PermissionCode = "SETTINGS_LIST", RoleId = RoleAnonymous },

                //#################################################################################

                // LANGUAGE_INSERT
                new { PermissionCode = "LANGUAGE_INSERT", RoleId = RoleSystem },
                new { PermissionCode = "LANGUAGE_INSERT", RoleId = RoleAdmin },
                new { PermissionCode = "LANGUAGE_INSERT", RoleId = RoleAnonymous },     

                // LANGUAGE_GET
                new { PermissionCode = "LANGUAGE_GET", RoleId = RoleSystem },
                new { PermissionCode = "LANGUAGE_GET", RoleId = RoleAdmin },
                new { PermissionCode = "LANGUAGE_GET", RoleId = RoleAnonymous },           

                // LANGUAGE_UPDATE
                new { PermissionCode = "LANGUAGE_UPDATE", RoleId = RoleSystem },
                new { PermissionCode = "LANGUAGE_UPDATE", RoleId = RoleAdmin },
                new { PermissionCode = "LANGUAGE_UPDATE", RoleId = RoleAnonymous },
                
                // LANGUAGE_DELETE
                new { PermissionCode = "LANGUAGE_DELETE", RoleId = RoleSystem },
                new { PermissionCode = "LANGUAGE_DELETE", RoleId = RoleAdmin },
                new { PermissionCode = "LANGUAGE_DELETE", RoleId = RoleAnonymous },   

                // LANGUAGE_LIST
                new { PermissionCode = "LANGUAGE_LIST", RoleId = RoleSystem },
                new { PermissionCode = "LANGUAGE_LIST", RoleId = RoleAdmin },
                new { PermissionCode = "LANGUAGE_LIST", RoleId = RoleAnonymous },

                //#################################################################################
                
                // USER_INSERT
                new { PermissionCode = "USER_INSERT", RoleId = RoleSystem },
                new { PermissionCode = "USER_INSERT", RoleId = RoleAdmin },
                new { PermissionCode = "USER_INSERT", RoleId = RoleAnonymous },     

                // USER_GET
                new { PermissionCode = "USER_GET", RoleId = RoleSystem },
                new { PermissionCode = "USER_GET", RoleId = RoleAdmin },
                new { PermissionCode = "USER_GET", RoleId = RoleAnonymous },           

                // USER_UPDATE
                new { PermissionCode = "USER_UPDATE", RoleId = RoleSystem },
                new { PermissionCode = "USER_UPDATE", RoleId = RoleAdmin },
                new { PermissionCode = "USER_UPDATE", RoleId = RoleAnonymous },
                
                // USER_DELETE
                new { PermissionCode = "USER_DELETE", RoleId = RoleSystem },
                new { PermissionCode = "USER_DELETE", RoleId = RoleAdmin },
                new { PermissionCode = "USER_DELETE", RoleId = RoleAnonymous },   

                // USER_LIST
                new { PermissionCode = "USER_LIST", RoleId = RoleSystem },
                new { PermissionCode = "USER_LIST", RoleId = RoleAdmin },
                new { PermissionCode = "USER_LIST", RoleId = RoleAnonymous },                

                //#################################################################################

                // ROLE_INSERT
                new { PermissionCode = "ROLE_INSERT", RoleId = RoleSystem },
                new { PermissionCode = "ROLE_INSERT", RoleId = RoleAdmin },
                new { PermissionCode = "ROLE_INSERT", RoleId = RoleAnonymous },     

                // ROLE_GET
                new { PermissionCode = "ROLE_GET", RoleId = RoleSystem },
                new { PermissionCode = "ROLE_GET", RoleId = RoleAdmin },
                new { PermissionCode = "ROLE_GET", RoleId = RoleAnonymous },           

                // ROLE_UPDATE
                new { PermissionCode = "ROLE_UPDATE", RoleId = RoleSystem },
                new { PermissionCode = "ROLE_UPDATE", RoleId = RoleAdmin },
                new { PermissionCode = "ROLE_UPDATE", RoleId = RoleAnonymous },
                
                // ROLE_DELETE
                new { PermissionCode = "ROLE_DELETE", RoleId = RoleSystem },
                new { PermissionCode = "ROLE_DELETE", RoleId = RoleAdmin },
                new { PermissionCode = "ROLE_DELETE", RoleId = RoleAnonymous },   

                // ROLE_LIST
                new { PermissionCode = "ROLE_LIST", RoleId = RoleSystem },
                new { PermissionCode = "ROLE_LIST", RoleId = RoleAdmin },
                new { PermissionCode = "ROLE_LIST", RoleId = RoleAnonymous }

                //#################################################################################
            };
        }


    }
}