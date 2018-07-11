/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:16:05
 */
using Core.Framework.Repository;
using Core.Framework.Service;
using DeviceManager.Service.Entity.System;

namespace DeviceManager.Service.Repository.SQLite
{
    public class SettingsRepository : BaseRepositorySQLite<Settings>, ISettingsRepository
    {
        public SettingsRepository(DatabaseContext databaseContext, ServiceContext serviceContext) : base(databaseContext.Connection, serviceContext.UserInfo.Username)
        { }

    }
}