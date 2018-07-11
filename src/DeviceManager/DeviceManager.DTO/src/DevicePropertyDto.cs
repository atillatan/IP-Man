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
    public class DevicePropertyDto : BaseDto
    {

        [DataMember]
        public int DeviceId { get; set; }

        [DataMember]
        public int PropertyId { get; set; }

        [DataMember]
        public string PropertyValue { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public DeviceDto DeviceDto { get; set; }


        [DataMember]
        public PropertyDto PropertyDto { get; set; }

    }
}