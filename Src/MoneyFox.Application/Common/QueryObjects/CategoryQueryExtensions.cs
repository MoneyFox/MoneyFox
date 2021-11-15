﻿using MoneyFox.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MoneyFox.Application.Common.QueryObjects
{
    /// <summary>
    ///     Provides Extensions for categories queries.
    /// </summary>
    public static class CategoryQueryExtensions
    {
        /// <summary>
        ///     Adds a filter for Categories who have a certain string in the name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <param name="searchterm">Search term to look for.</param>
        /// <returns>Query with the added filter.</returns>
        public static IEnumerable<Category> WhereNameContains(this IEnumerable<Category> query, string searchterm)
            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            => query.Where(category => category.Name.ToUpper().IndexOf(searchterm.ToUpper()) >= 0);

        /// <summary>
        ///     Orders a category query by name.
        /// </summary>
        /// <param name="query">Existing query.</param>
        /// <returns>Ordered Query</returns>
        public static IQueryable<Category> OrderByName(this IQueryable<Category> query)
            => query.OrderBy(category => category.Name);
    }
}