using System.Linq.Expressions;

namespace ZYTreeHole_Share;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (page <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than 0.");
        }

        if (pageSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size must be greater than 0.");
        }

        return query.Skip((page - 1) * pageSize).Take(pageSize);
    }
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> source, string sortField, bool ascending)  
    {  
        if (string.IsNullOrEmpty(sortField))  
            return source; // 如果没有提供排序字段，返回原始查询  

        var parameter = Expression.Parameter(typeof(T), "x");  
        var property = Expression.Property(parameter, sortField);  
        // 创建排序表达式  
        Expression<Func<T, object>> sortExpression;  
    
        // 使用 Convert 将属性转换为 object，以支持各种类型的属性  
        var convertExpression = Expression.Convert(property, typeof(object));  
        sortExpression = Expression.Lambda<Func<T, object>>(convertExpression, parameter);  

        // 指定类型实参以避免错误  
        if (ascending)  
        {  
            return source.OrderByDescending(sortExpression);  
        }

        return source.OrderBy(sortExpression);
    }
}