using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.UI
{
    public class CustomPagination<T>
    {
        List<T> Data;
        int PageNo;
        int PageSize;
        string Query;
        public CustomPagination()
        {
        }

        public string GetPaginatedQuery(string Query)
        {
            return "";
        }
        public CustomPagination(List<object> Data, int PageNo, int PageSize)
        {
            this.Data = new List<T>();
            this.PageNo = PageNo;
            this.PageSize = PageSize;
        }

        public CustomPagination(string Query, int PageNo, int PageSize)
        {
            this.PageNo = PageNo;
            this.PageSize = PageSize;
            this.Query += Query + " OFFSET @ ROWS FETCH NEXT @NextFetch ROWS ONLY";
        }
        public int PageCount { get{return Data.Count;}}
        public bool HasNextPage 
        {
            get
            {
                if(this.PageNo < PageCount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                if (this.PageNo > 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
