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

    [Table("DEVICE_PROPERTY")]
    [Serializable]
    public class DeviceProperty : BaseEntity
    {
        public int DeviceId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyValue { get; set; }
        public int Version { get; set; }
        public Device Device { get; set; }
        public Property Property { get; set; }

        public DeviceProperty CopyFrom(DevicePropertyDto dto)
        {
            this.DeviceId = dto.DeviceId;
            this.PropertyId = dto.PropertyId;
            this.PropertyValue = dto.PropertyValue;
            this.Version = dto.Version;
            if (dto?.DeviceDto != null) this.Device.CopyFrom(dto.DeviceDto);
            if (dto?.PropertyDto != null) this.Property.CopyFrom(dto.PropertyDto);
            BaseCopyFrom<DeviceProperty>(dto);
            return this;
        }

        public DevicePropertyDto CopyTo(DevicePropertyDto dto)
        {
            dto.DeviceId = this.DeviceId;
            dto.PropertyId = this.PropertyId;
            dto.PropertyValue = this.PropertyValue;
            dto.Version = this.Version;
            if (dto?.DeviceDto != null) this.Device.CopyTo(dto.DeviceDto);
            if (dto?.PropertyDto != null) this.Property.CopyTo(dto.PropertyDto);
            BaseCopyTo<DevicePropertyDto>(dto);
            return dto;
        }
    }
}