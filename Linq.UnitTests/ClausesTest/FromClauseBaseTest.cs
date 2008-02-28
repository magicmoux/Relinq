using System;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rubicon.Data.Linq.Clauses;
using Rubicon.Data.Linq.DataObjectModel;
using Rubicon.Data.Linq.Parsing.FieldResolving;

namespace Rubicon.Data.Linq.UnitTests.ClausesTest
{
  [TestFixture]
  public class FromClauseBaseTest
  {
    [Test]
    public void GetTable ()
    {
      ParameterExpression id = Expression.Parameter (typeof (Student), "s1");
      IQueryable querySource = ExpressionHelper.CreateQuerySource ();

      MainFromClause fromClause = new MainFromClause (id, querySource);
      Assert.AreEqual (new Table ("studentTable", "s1"), fromClause.GetTable (StubDatabaseInfo.Instance));
    }

    [Test]
    public void GetTable_CachesInstance ()
    {
      ParameterExpression id = Expression.Parameter (typeof (Student), "s1");
      IQueryable querySource = ExpressionHelper.CreateQuerySource ();

      MainFromClause fromClause = new MainFromClause (id, querySource);
      Table t1 = fromClause.GetTable (StubDatabaseInfo.Instance);
      Table t2 = fromClause.GetTable (StubDatabaseInfo.Instance);
      Assert.AreSame (t1, t2);
    }

    [Test]
    public void AddJoinClause()
    {
      MainFromClause fromClause = ExpressionHelper.CreateMainFromClause();

      JoinClause joinClause1 = ExpressionHelper.CreateJoinClause();
      JoinClause joinClause2 = ExpressionHelper.CreateJoinClause();

      fromClause.Add (joinClause1);
      fromClause.Add (joinClause2);

      Assert.That (fromClause.JoinClauses, Is.EqualTo (new object[] { joinClause1, joinClause2 }));
      Assert.AreEqual (2, fromClause.JoinClauses.Count);
    }

    [Test]
    public void FromClause_ImplementsIQueryElement()
    {
      Assert.IsTrue (typeof (IQueryElement).IsAssignableFrom (typeof (FromClauseBase)));
    }

    [Test]
    public void Resolve_Succeeds_MainFromClause ()
    {
      ParameterExpression identifier = Expression.Parameter (typeof (Student), "fromIdentifier1");
      MainFromClause fromClause = new MainFromClause (identifier, ExpressionHelper.CreateQuerySource ());

      JoinedTableContext context = new JoinedTableContext ();
      FieldDescriptor fieldDescriptor = fromClause.ResolveField (StubDatabaseInfo.Instance, context, identifier, identifier,new WhereFieldAccessPolicy());
      Assert.AreEqual (new Column (new Table ("studentTable", "fromIdentifier1"), "*"), fieldDescriptor.Column);
      Assert.AreSame (fromClause, fieldDescriptor.FromClause);
    }

    [Test]
    public void Resolve_Succeeds_AdditionalFromClause ()
    {
      ParameterExpression identifier = Expression.Parameter (typeof (Student), "fromIdentifier1");
      AdditionalFromClause fromClause = CreateAdditionalFromClause (identifier);

      JoinedTableContext context = new JoinedTableContext ();
      FieldDescriptor fieldDescriptor = fromClause.ResolveField (StubDatabaseInfo.Instance, context, identifier, identifier,new WhereFieldAccessPolicy());
      Assert.AreEqual (new Column (new Table ("studentTable", "fromIdentifier1"), "*"), fieldDescriptor.Column);
      Assert.AreSame (fromClause, fieldDescriptor.FromClause);
    }

    private AdditionalFromClause CreateAdditionalFromClause (ParameterExpression additionalFromIdentifier)
    {
      MainFromClause mainFromClause = new MainFromClause (ExpressionHelper.CreateParameterExpression (), ExpressionHelper.CreateQuerySource ());
      LambdaExpression fromExpression = Expression.Lambda (Expression.Constant (null, typeof (IQueryable<Student>)));
      LambdaExpression projectionExpression = Expression.Lambda (Expression.Constant (null, typeof (Student)));
      return new AdditionalFromClause (mainFromClause, additionalFromIdentifier, fromExpression, projectionExpression);
    }
  }
}