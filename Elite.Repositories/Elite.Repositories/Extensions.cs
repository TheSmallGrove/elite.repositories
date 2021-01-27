using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elite.Repositories
{
    public static class Extensions
    {
        public static IServiceCollection AddSettings(this IServiceCollection services)
        {
            //services.AddSingleton<IRepositorySettings>(
            //    _ => _.GetService<IConfiguration>()
            //    .GetSection("Repository")
            //    .Get<RepositorySettings>());

            return services;
        }

        //public static async Task<IPagedEnumerable<T>> PagedAsync<T>(this IQueryable<T> queryable, IEnumerable<Criteria> criteria)
        //{
        //    Criteria paging = criteria.FindOne(CriteriaConstants.Paging);

        //    var count = await queryable.CountAsync();

        //    if (paging != null)
        //    {
        //        int pageIndex = paging.GetValueOrDefault<int>(CriteriaConstants.PageIndex, 0);
        //        int pageSize = paging.GetValueOrDefault<int>(CriteriaConstants.PageSize, 10);

        //        queryable = queryable
        //            .Skip(pageIndex * pageSize)
        //            .Take(pageSize);
        //    }

        //    return new PagedEnumerable<T>(count, await queryable.ToArrayAsync());
        //}

        //public static IQueryable<T> Sorted<T>(this IQueryable<T> queryable, IEnumerable<Criteria> criteria, IDictionary<string, string> orderings)
        //{
        //    Criteria sorting = criteria.FindOne(CriteriaConstants.Sorting);

        //    var name = sorting.GetValueOrDefault<string>(CriteriaConstants.Name, string.Empty);

        //    if (string.IsNullOrEmpty(name))
        //        throw new ApplicationException($"Sort name was not specified");

        //    string sort;
        //    if (orderings.TryGetValue(name, out sort))
        //        return queryable.OrderBy(sort);

        //    throw new ApplicationException($"Invalid sort criteria specified '{name}'");
        //}

        //public static IEnumerable<Criteria> FindMany(this IEnumerable<Criteria> criteria, string type)
        //{
        //    return criteria.Where(_ => _.Type == type).ToArray();
        //}

        //public static Criteria FindOne(this IEnumerable<Criteria> criteria, string type)
        //{
        //    if (criteria.Where(_ => _.Type == type).Count() > 1)
        //        throw new ApplicationException("Multiple paging criteria specified in query are not allowed");

        //    return criteria.SingleOrDefault(_ => _.Type == type);
        //}

        //public static IPagedEnumerable<K> Map<T, K>(this IPagedEnumerable<T> enumerable, Func<T, K> map)
        //{
        //    return new PagedEnumerable<K>(
        //        enumerable.VirtualItemCount,
        //        (from i in enumerable.Items
        //         select map(i)).ToArray());
        //}
    }
}
