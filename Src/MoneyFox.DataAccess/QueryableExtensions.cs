using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using MvvmCross.Platform;

namespace MoneyFox.DataAccess
{
    public static class QueryableExtensions
    {
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
        {
            return new ProjectionExpression<TSource>(source);
        }
    }

    public class ProjectionExpression<TSource>
    {
        private static readonly Dictionary<string, Expression> ExpressionCache =
            new Dictionary<string, Expression>();

        private readonly IQueryable<TSource> source;

        public ProjectionExpression(IQueryable<TSource> source)
        {
            this.source = source;
        }

        public IQueryable<TDest> To<TDest>()
        {
            var queryExpression = GetCachedExpression<TDest>() ?? BuildExpression<TDest>();

            return source.Select(queryExpression);
        }

        private static Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = GetCacheKey<TDest>();

            return ExpressionCache.ContainsKey(key)
                ? ExpressionCache[key] as Expression<Func<TSource, TDest>>
                : null;
        }

        private static Expression<Func<TSource, TDest>> BuildExpression<TDest>()
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                .Select(
                    destinationProperty => BuildBinding(parameterExpression, destinationProperty, sourceProperties))
                .Where(binding => binding != null);

            var expression =
                Expression.Lambda<Func<TSource, TDest>>(
                    Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = GetCacheKey<TDest>();

            ExpressionCache.Add(key, expression);

            return expression;
        }

        private static MemberAssignment BuildBinding(Expression parameterExpression, MemberInfo destinationProperty,
            IEnumerable<PropertyInfo> sourceProperties)
        {
            var propertyInfos = sourceProperties as IList<PropertyInfo> ?? sourceProperties.ToList();
            var sourceProperty = propertyInfos.FirstOrDefault(src => src.Name == destinationProperty.Name);

            if (sourceProperty != null)
            {
                return Expression.Bind(destinationProperty, Expression.Property(parameterExpression, sourceProperty));
            }

            var propertyNames = SplitCamelCase(destinationProperty.Name);

            if (propertyNames.Length == 2)
            {
                sourceProperty = propertyInfos.FirstOrDefault(src => src.Name == propertyNames[0]);

                var sourceChildProperty =
                    sourceProperty?.PropertyType.GetProperties()
                        .FirstOrDefault(src => src.Name == propertyNames[1]);

                if (sourceChildProperty != null)
                {
                    return Expression.Bind(destinationProperty,
                        Expression.Property(Expression.Property(parameterExpression, sourceProperty),
                            sourceChildProperty));
                }
            }

            return null;
        }

        private static string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }

        private static string[] SplitCamelCase(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.None).Trim().Split(' ');
        }
    }
}