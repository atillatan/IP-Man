/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-05-18 18:08:49
 */
using System;
using Core.Framework.Repository.DTO;

namespace Core.Framework.Repository.Entity
{
    [Serializable]
    public abstract class BaseEntity : IComparable
    {
        public BaseEntity()
        {
            IsActive = true;
        }

        public long Id { get; set; }

        public DateTime CreateDate { get; set; }

        public long CreatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public long? UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public void OnDeserialization(Object o)
        {
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;

            BaseEntity baseEntity = obj as BaseEntity;
            if (baseEntity != null)
                return this.Id.CompareTo(baseEntity.Id);
            else
                throw new ArgumentException("Object is not a BaseEntity");
        }

        public override bool Equals(Object obj)
        {
            BaseEntity baseEntity = obj as BaseEntity;
            if (baseEntity == null)
                return false;
            else
                return Id.Equals(baseEntity.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(this.GetType());
        }

        public T BaseCopyFrom<T>(BaseDto dto) where T : class
        {
            this.Id = dto.Id;
            this.CreateDate = dto.CreateDate;
            this.CreatedBy = dto.CreatedBy;
            this.UpdateDate = dto.UpdateDate;
            this.UpdatedBy = dto.UpdatedBy;
            this.IsActive = dto.IsActive;
            return this as T;
        }

        public T BaseCopyTo<T>(BaseDto dto) where T : class
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
