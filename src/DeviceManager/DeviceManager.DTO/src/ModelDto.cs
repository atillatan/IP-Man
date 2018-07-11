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
    public class ModelDto : BaseDto
    {

        [DataMember]
        public string BrandName { get; set; }
 
        
        [DataMember]
        public string ModelName { get; set; }


    }
}