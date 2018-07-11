/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-02 18:13:49
 */
using System;
using System.Collections.Generic;
using Core.Framework.Repository.Entity;
using DeviceManager.DTO.System;

namespace DeviceManager.Service.Entity.System
{

    [Table("LANGUAGE")]
    [Serializable]
    public class Language : BaseEntity
    {
        public string Key { get; set; }

        public string Val { get; set; }

        public string LanguageCode { get; set; }

        public int? Iseditable { get; set; }

        public Language CopyFrom(LanguageDto dto)
        {
            this.Key = dto.Key;
            this.Val = dto.Val;
            this.LanguageCode = dto.LanguageCode;
            this.Iseditable = dto.Iseditable;
            BaseCopyFrom<Language>(dto);
            return this;
        }

        public LanguageDto CopyTo(LanguageDto dto)
        {
            dto.Key = this.Key;
            dto.Val = this.Val;
            dto.LanguageCode = this.LanguageCode;
            dto.Iseditable = this.Iseditable;
            BaseCopyTo<LanguageDto>(dto);
            return dto;
        }
    }
}