/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
using System.Collections.Generic;
using Core.Framework.Repository.DTO;
using Core.Framework.Service;
using Dapper;

namespace Core.Framework.Repository
{
    public interface IRepository<T>
    {
        #region CRUD Operations
        long Insert(T obj);//C: Create

        T Get(long id);//R: Read

        int Update(T obj);//U: Update

        int Delete(long id);//D: Delete

        #endregion

        #region Extensions

        int DeleteHard(long id);

        int DeleteHard(long[] id);

        IEnumerable<T> List(BaseDto baseDto, ref PagingDto pagingDto);

        IEnumerable<T> ListAll();

        int InsertAll(List<T> list);

        int UpdateAll(List<T> list);

        T SaveOrUpdate(T obj);

        IEnumerable<T> Get(long[] id);

        int Delete(long[] id);

        int Count();

        int Count(BaseDto baseDto);

        #endregion

        #region Helpers

        DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, out string sqlWhere);

        DynamicParameters GenerateSearchDynamicParameters(BaseDto baseDto, string alias, out string sqlWhere);

        DynamicParameters GenerateSearchDynamicParameters(Dictionary<string, BaseDto> baseDtoList, out string sqlWhere);

        string PagingSql(PagingDto pagingDto);

        string OrderBy(PagingDto pagingDto);

        string OrderBy(PagingDto pagingDto, string alias);

        #endregion
    }
}