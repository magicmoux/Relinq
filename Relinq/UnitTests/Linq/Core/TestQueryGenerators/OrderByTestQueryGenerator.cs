// This file is part of the re-motion Core Framework (www.re-motion.org)
// Copyright (C) 2005-2009 rubicon informationstechnologie gmbh, www.rubicon.eu
// 
// The re-motion Core Framework is free software; you can redistribute it 
// and/or modify it under the terms of the GNU Lesser General Public License 
// as published by the Free Software Foundation; either version 2.1 of the 
// License, or (at your option) any later version.
// 
// re-motion is distributed in the hope that it will be useful, 
// but WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the 
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with re-motion; if not, see http://www.gnu.org/licenses.
// 
using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Data.Linq.UnitTests.Linq.Core.TestDomain;

namespace Remotion.Data.Linq.UnitTests.Linq.Core.TestQueryGenerators
{
  public static class OrderByTestQueryGenerator
  {
    public static IQueryable<Cook> CreateSimpleOrderByQuery (IQueryable<Cook> source)
    {
      return from s1 in source orderby s1.FirstName select s1;
    }

    public static IQueryable<Kitchen> CreateRelationMemberOrderByQuery (IQueryable<Kitchen> source)
    {
      return from sd in source orderby sd.Cook select sd;
    }

    public static IQueryable<Cook> CreateOrderByNonDBPropertyQuery (IQueryable<Cook> source)
    {
      return from s1 in source orderby s1.NonDBStringProperty select s1;
    }

    public static IQueryable<Cook> CreateTwoOrderByQuery (IQueryable<Cook> source)
    {
      return from s1 in source orderby s1.FirstName orderby s1.Name descending select s1;
    }

    public static IQueryable<Cook> CreateThreeOrderByQuery (IQueryable<Cook> source)
    {
      return from s1 in source orderby s1.FirstName, s1.Name orderby s1.Name descending select s1;
    }

    public static IQueryable<Cook> CreateOrderByQueryWithOrderByAndThenBy (IQueryable<Cook> source)
    {
      return from s in source orderby s.FirstName, s.Name descending, s.Holidays select s;
    }

    public static IQueryable<Cook> CreateOrderByQueryWithMultipleOrderBys (IQueryable<Cook> source)
    {
      return from s in source orderby s.FirstName, s.Name descending, s.Holidays orderby s.Name select s;
    }

    public static MethodCallExpression CreateOrderByQueryWithOrderByAndThenBy_OrderByExpression (IQueryable<Cook> source)
    {
      IQueryable<Cook> query = CreateOrderByQueryWithOrderByAndThenBy (source);
      return (MethodCallExpression) query.Expression;
    }

    public static MethodCallExpression CreateOrderByQueryWithMultipleOrderBys_OrderByExpression (IQueryable<Cook> source)
    {
      IQueryable<Cook> query = CreateOrderByQueryWithMultipleOrderBys (source);
      return (MethodCallExpression) query.Expression;
    }

  }
}