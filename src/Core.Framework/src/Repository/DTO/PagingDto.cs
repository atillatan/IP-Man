/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System;
using System.Runtime.Serialization;

namespace Core.Framework.Repository.DTO
{
    [Serializable]
    [DataContract]
    public class PagingDto
    {

        [DataMember]
        public int pageNumber { get; set; }

        [DataMember]
        public int pageSize { get; set; }

        [DataMember]
        public string orderBy { get; set; }

        [DataMember]
        public string order { get; set; }

        [DataMember]
        public int count { get; set; }
    }
}