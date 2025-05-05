using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> CreateQuery<T>(IQueryable<T> inputQuery, ISpecifications<T> specifications) where T : class
        {
            var query = inputQuery;
            if (specifications.Criteria != null) //We Have a Filter
                query = query.Where(specifications.Criteria);

            //foreach (var include in specifications.IncludeExpressions)//Load nav props
            //    query.Include(include);

            //adding sorting
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            else if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            //check for pagination
            if(specifications.IsPaginated)
                query = query.Skip(specifications.Skip).Take(specifications.Take);


            //using aggregate instead of foreach
            query = specifications.IncludeExpressions
                                  .Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
