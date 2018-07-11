/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
namespace Core.Framework.Repository
{
    public interface IProxy
    {
        bool IsDirty { get; set; }
    }
}