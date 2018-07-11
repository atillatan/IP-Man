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

    [Table("TEMPLATE_PROPERTY")]
    [Serializable]
    public class TemplateProperty : BaseEntity
    {
        public int TemplateId { get; set; }
        public int PropertyId { get; set; }
        public string DefaultValue { get; set; }
        public int IsRequired { get; set; }
        public int Version { get; set; }
        public Template Template { get; set; }
        public Property Property { get; set; }

        public TemplateProperty CopyFrom(TemplatePropertyDto dto)
        {
            this.TemplateId = dto.TemplateId;
            this.PropertyId = dto.PropertyId;
            this.DefaultValue = dto.DefaultValue;
            this.IsRequired = dto.IsRequired;
            this.Version = dto.Version;
            if (dto?.TemplateDto != null) this.Template.CopyFrom(dto?.TemplateDto);
            if (dto?.PropertyDto != null) this.Property.CopyFrom(dto?.PropertyDto);
            BaseCopyFrom<TemplateProperty>(dto);
            return this;
        }

        public TemplatePropertyDto CopyTo(TemplatePropertyDto dto)
        {
            dto.TemplateId = this.TemplateId;
            dto.PropertyId = this.PropertyId;
            dto.DefaultValue = this.DefaultValue;
            dto.IsRequired = this.IsRequired;
            dto.Version = this.Version;
            if ((dto?.TemplateDto) != null) this.Template.CopyTo(dto?.TemplateDto);
            if ((dto?.PropertyDto) != null) this.Property.CopyTo(dto?.PropertyDto);
            BaseCopyTo<TemplatePropertyDto>(dto);
            return dto;
        }
    }
}