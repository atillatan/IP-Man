/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:12:26
 */
using System;
using System.Collections.Generic;
using Core.Framework.Repository.Entity;
using DeviceManager.DTO.System;

namespace DeviceManager.Service.Entity.System
{
    [Table("SETTINGS")]
    [Serializable]
    public class Settings : BaseEntity
    {
        public string Key { get; set; }

        public string Val { get; set; }


        public Settings CopyFrom(SettingsDto dto)
        {
            this.Key = dto.Key;
            this.Val = dto.Val;
            BaseCopyFrom<Settings>(dto);
            return this;
        }

        public SettingsDto CopyTo(SettingsDto dto)
        {
            dto.Key = this.Key;
            dto.Val = this.Val;
            BaseCopyTo<SettingsDto>(dto);
            return dto;
        }
    }
}