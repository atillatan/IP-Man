/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using Core.Framework.Repository.DTO;
using System;
using System.Runtime.Serialization;

namespace DeviceManager.DTO
{
    [Serializable]
    [DataContract]
    public class TemplatePropertyDto : BaseDto
    {

        [DataMember]
        public int TemplateId { get; set; }

        [DataMember]
        public int PropertyId { get; set; }

        [DataMember]
        public string DefaultValue { get; set; }

        [DataMember]
        public int IsRequired { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public TemplateDto TemplateDto { get; set; }

        [DataMember]
        public PropertyDto PropertyDto { get; set; }

    }
}