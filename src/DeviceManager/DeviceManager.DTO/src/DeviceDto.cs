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
    public class DeviceDto : BaseDto
    {

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public int TemplateId { get; set; }

        [DataMember]
        public int ModelId { get; set; }

        [DataMember]
        public int State { get; set; }

        [DataMember]
        public string DeviceCode { get; set; }

        [DataMember]
        public string SerialNumber { get; set; }

        [DataMember]
        public DateTime? LastAccessDate { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public TemplateDto TemplateDto { get; set; }

        [DataMember]
        public ModelDto ModelDto { get; set; }

    }
}