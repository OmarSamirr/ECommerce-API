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
    }
}
