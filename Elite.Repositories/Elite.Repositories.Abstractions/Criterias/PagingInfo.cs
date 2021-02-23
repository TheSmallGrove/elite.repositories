using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elite.Repositories.Abstractions.Criterias
{
    public class PagingInfo
    {
        public int CurrentPage { get; set; }
        public double Percentage { get; set; }
        public int RecordsPerPage { get; set; }
        public int VirtualNumberOfPages { get; set; }
        public int VirtualNumberOfRecords { get; set; }
    }

    public static class PagingExtensions
    {
        public static PagingInfo Calculate(this PagingCriteria criteria, int virtualCount)
        {
            return new PagingInfo
            {
                CurrentPage = criteria.PageIndex,
                Percentage = Math.Round(criteria.PageIndex / (Math.Ceiling((double)virtualCount / criteria.PageSize) - 1), 3),
                RecordsPerPage = criteria.PageSize,
                VirtualNumberOfPages = (int)Math.Ceiling((double)virtualCount / criteria.PageSize),
                VirtualNumberOfRecords = virtualCount
            };
        }

        public static PagingCriteria GoToFirst(this PagingInfo info)
        {
            return new PagingCriteria
            {
                PageIndex = 0,
                PageSize = info.RecordsPerPage
            };
        }

        public static PagingCriteria GoToPrevious(this PagingInfo info)
        {
            return new PagingCriteria
            {
                PageIndex = (info.CurrentPage > 1) ? info.CurrentPage - 2 : 0,
                PageSize = info.RecordsPerPage
            };
        }

        public static PagingCriteria GoToNext(this PagingInfo info)
        {
            return new PagingCriteria
            {
                PageIndex = (info.CurrentPage < info.VirtualNumberOfPages - 1) ? info.CurrentPage : info.VirtualNumberOfPages - 1,
                PageSize = info.RecordsPerPage
            };
        }

        public static PagingCriteria GoToLast(this PagingInfo info)
        {
            return new PagingCriteria
            {
                PageIndex = info.VirtualNumberOfPages - 1,
                PageSize = info.RecordsPerPage
            };
        }

    }
}
