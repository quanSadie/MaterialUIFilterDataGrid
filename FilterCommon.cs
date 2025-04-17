﻿#region (c) 2019 Gilles Macabies All right reserved

// Author     : Gilles Macabies
// Solution   : DataGridFilter
// Projet     : DataGridFilter
// File       : FilterCommon.cs
// Created    : 26/01/2021
//

#endregion (c) 2019 Gilles Macabies All right reserved

using System;
using System.Collections.Generic;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InvalidXmlDocComment
// ReSharper disable TooManyChainedReferences
// ReSharper disable ExcessiveIndentation
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable CheckNamespace

// https://stackoverflow.com/questions/2251260/how-to-develop-treeview-with-checkboxes-in-wpf
// https://stackoverflow.com/questions/14876032/group-dates-by-year-month-and-date-in-wpf-treeview
// https://www.codeproject.com/Articles/28306/Working-with-Checkboxes-in-the-WPF-TreeView

namespace FilterDataGrid
{
    public sealed class FilterCommon : NotifyProperty
    {
        #region Private Fields

        private bool isFiltered;

        #endregion Private Fields

        #region Public Constructors

        public FilterCommon()
        {
            PreviouslyFilteredItems = new HashSet<object>(EqualityComparer<object>.Default);
        }

        #endregion Public Constructors

        #region Public Properties

        public string FieldName { get; set; }
        public Type FieldType { get; set; }

        public bool IsFiltered
        {
            get => isFiltered;
            set
            {
                isFiltered = value;
                OnPropertyChanged("IsFiltered");
            }
        }

        public HashSet<object> PreviouslyFilteredItems { get; set; }

        public Loc Translate { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Add the filter to the predicate dictionary
        /// </summary>
        public void AddFilter(Dictionary<string, Predicate<object>> criteria)
        {
            if (IsFiltered) return;

            // predicate of filter
            bool Predicate(object o)
            {
                var value = FieldType == typeof(DateTime)
                    ? ((DateTime?)o.GetType().GetProperty(FieldName)?.GetValue(o, null))?.Date
                    : o.GetType().GetProperty(FieldName)?.GetValue(o, null);

                return !PreviouslyFilteredItems.Contains(value);
            }

            // add to list of predicates
            criteria.Add(FieldName, Predicate);

            IsFiltered = true;
        }

        #endregion Public Methods
    }
}