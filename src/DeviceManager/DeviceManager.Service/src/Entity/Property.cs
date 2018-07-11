/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:13:49
 */
using System;
using System.Collections.Generic;
using Core.Framework.Repository.Entity;
using DeviceManager.DTO;

namespace DeviceManager.Service.Entity
{

    [Table("PROPERTY")]
    [Serializable]
    public class Property : BaseEntity
    {
        public string Name { get; set; }
         public int Version { get; set; }

        public Property CopyFrom(PropertyDto dto)
        {
            this.Name = dto.Name;
            this.Version = dto.Version;
            BaseCopyFrom<Property>(dto);
            return this;
        }

        public PropertyDto CopyTo(PropertyDto dto)
        {
            dto.Name = this.Name;     
            dto.Version = this.Version;
            BaseCopyTo<PropertyDto>(dto);
            return dto;
        }
    }
}