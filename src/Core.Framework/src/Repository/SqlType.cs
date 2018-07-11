/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by: Atilla Tanrikulu
 * @Last Modified time: 2018-04-16 10:11:10
 */
namespace Core.Framework.Repository
{
    public enum SqlType : int
    {
        Insert = 1,
        Select = 2,
        Update = 3,
        Delete = 4
    }
}