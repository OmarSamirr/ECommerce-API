using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<T> where T : class
    {
        Expression<Func<T, bool>> Criteria { get; } //Filter
        List<Expression<Func<T, object>>> IncludeExpressions { get; } //Loading Nav Props
        Expression<Func<T, object>> OrderBy { get; } //Order Ascending
        Expression<Func<T, object>> OrderByDescending { get; } //Order Descending
        int Skip { get; }
        int Take { get; }
        bool IsPaginated { get; }

    }
}
