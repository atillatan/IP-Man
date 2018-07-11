/*
 * @Author: Atilla Tanrikulu 
 * @Date: 2018-04-16 10:10:45 
 * @Last Modified by:   Atilla Tanrikulu 
 * @Last Modified time: 2018-04-16 10:10:45 
 */
namespace Core.Framework.Service
{
    public class MessagesConstants
    {
        #region System Success

        public const string SCC_DATA_INSERTED = "SCC_DATA_INSERTED";
        public const string SCC_DATA_UPDATED = "SCC_DATA_UPDATED";
        public const string SCC_DATA_DELETED = "SCC_DATA_DELETED";
        public const string SCC_FILE_SAVED = "SCC_FILE_SAVED";

        #endregion

        #region System Errors

        public const string ERR_DATA_NOT_FOUND_TO_SAVE = "ERR_DATA_NOT_FOUND_TO_SAVE";
        public const string ERR_INSERT = "ERR_INSERT";
        public const string ERR_UPDATE = "ERR_UPDATE";
        public const string ERR_DELETE = "ERR_DELETE";
        public const string ERR_FILE_SAVE = "ERR_FILE_SAVE";
        public const string ERR_ORA_00001 = "ORA-00001";//Unique Constraint
        public const string ERR_ORA_1722 = "ORA-1722";//Invalid Number
        public const string ERR_ORA_01407 = "ORA-01407";//not null
        public const string ERR_ORA_12899 = "ORA-12899";//maxlength
        public const string ERR_ORA_02291 = "ORA-02291";//foreign key constraint

        public const string ERR_OCCURRED = "ERR_OCCURRED";

        #endregion

        #region System Warnings

        public const string WRN_RECORD_NOT_FOUND = "WRN_RECORD_NOT_FOUND";

        #endregion Warning
    }
}