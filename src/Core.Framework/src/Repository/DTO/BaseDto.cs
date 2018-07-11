/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-18 18:08:25
 */
using System;
using System.Runtime.Serialization;

namespace Core.Framework.Repository.DTO
{

    [Serializable]
    [DataContract]
    public abstract class BaseDto : IComparable
    {
        public BaseDto()
        {
            IsActive = true;
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public long CreatedBy { get; set; }

        [DataMember]
        public DateTime? UpdateDate { get; set; }

        [DataMember]
        public long? UpdatedBy { get; set; }
        
        [DataMember]
        public bool IsActive { get; set; }

        public void OnDeserialization(Object o) { }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;

            BaseDto baseDto = obj as BaseDto;
            if (baseDto != null)
                return this.Id.CompareTo(baseDto.Id);
            else
                throw new ArgumentException("Object is not a BaseDto");
        }
        
        public override bool Equals(Object obj)
        {
            BaseDto baseDto = obj as BaseDto;
            if (baseDto == null)
                return false;
            else
                return Id.Equals(baseDto.Id);
        }
        
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(this.GetType());
        }
        public T BaseCopy<T>(BaseDto dto) where T : class
        {
           dto.Id = this.Id;
           dto.CreateDate = this.CreateDate;
           dto.CreatedBy = this.CreatedBy;
           dto.UpdateDate = this.UpdateDate;
           dto.UpdatedBy = this.UpdatedBy;
           dto.IsActive = this.IsActive;
           return dto as T;
        }

    }
}