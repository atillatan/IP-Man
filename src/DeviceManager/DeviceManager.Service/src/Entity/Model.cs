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

    [Table("MODEL")]
    [Serializable]
    public class Model : BaseEntity
    {
        public string BrandName { get; set; }
        public string ModelName { get; set; }


        public Model CopyFrom(ModelDto dto)
        {
            this.BrandName = dto.BrandName;
            this.ModelName = dto.ModelName;
            BaseCopyFrom<Model>(dto);
            return this;
        }

        public ModelDto CopyTo(ModelDto dto)
        {
            dto.BrandName = this.BrandName;
            dto.ModelName = this.ModelName;
            BaseCopyTo<ModelDto>(dto);
            return dto;
        }
    }
}