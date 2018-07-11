/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using Core.Framework.Repository.DTO;
using System;
using System.Runtime.Serialization;

namespace DeviceManager.DTO.System
{
    [Serializable]
    [DataContract]
    public class LanguageDto : BaseDto
    {
        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Val { get; set; }

        [DataMember]
        public string LanguageCode { get; set; }

        [DataMember]
        public int? Iseditable { get; set; }

    }
}