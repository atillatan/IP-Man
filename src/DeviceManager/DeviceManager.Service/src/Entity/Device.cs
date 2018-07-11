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

    [Table("DEVICE")]
    [Serializable]
    public class Device : BaseEntity
    {
        public int ParentId { get; set; }
        public int TemplateId { get; set; }
        public int ModelId { get; set; }
        public int State { get; set; }
        public string DeviceCode { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public int Version { get; set; }
        public Template Template { get; set; }
        public Model Model { get; set; }

        public Device CopyFrom(DeviceDto dto)
        {
            this.ParentId = dto.ParentId;
            this.ModelId = dto.ModelId;
            this.TemplateId = dto.TemplateId;
            this.State = dto.State;
            this.DeviceCode = dto.DeviceCode;
            this.SerialNumber = dto.SerialNumber;
            this.LastAccessDate = dto.LastAccessDate;            
            this.Version = dto.Version;
            if (dto?.TemplateDto != null) this.Template.CopyFrom(dto.TemplateDto);
            if (dto?.ModelDto != null) this.Model.CopyFrom(dto.ModelDto);
            BaseCopyFrom<Device>(dto);
            return this;
        }

        public DeviceDto CopyTo(DeviceDto dto)
        {
            dto.ParentId = this.ParentId;
            dto.ModelId = this.ModelId;
            dto.TemplateId = this.TemplateId;
            dto.State = this.State;
            dto.DeviceCode = this.DeviceCode;
            dto.SerialNumber = this.SerialNumber;
            dto.LastAccessDate = this.LastAccessDate;            
            dto.Version = this.Version;
            if (dto?.TemplateDto != null) this.Template.CopyTo(dto.TemplateDto);
            if (dto?.ModelDto != null) this.Model.CopyTo(dto.ModelDto);
            BaseCopyTo<DeviceDto>(dto);
            return dto;
        }
    }
}