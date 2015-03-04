// Copyright (c) Andy Sipe. All rights reserved. Licensed under the MIT License (MIT). See License.txt in the project root for license information.

using System.Linq;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Ploeh.AutoFixture;

namespace Maddir.UnitTests {
  [TestFixture]
  public abstract class BaseTestCase {
    protected class EqualToConstraint : Constraint {
      public EqualToConstraint(object exp) : base(exp) {
        mExp = exp;
      }

      public override bool Matches(object act) {
        actual = act;
        mResult = DoCompare(act, mExp);
        return mResult.AreEqual;
      }

      public override void WriteDescriptionTo(MessageWriter writer) {
        writer.WriteLine(mResult.DifferencesString);
      }

      private readonly object mExp;
      private ComparisonResult mResult;
    }

    protected static class Are {
      public static EqualToConstraint EqualTo(object expected) {
        return new EqualToConstraint(expected);
      }
    }

    static BaseTestCase() {
      _ObjectComparer.Config.IgnoreObjectTypes = true;
    }

    [SetUp]
    public void BaseSetup() {
      MokFac = new MockRepository(MockBehavior.Strict);
      ObjectFixture = new Fixture();
    }

    [TearDown]
    public void BaseTearDown() {
      VerifyMocks();
    }

    protected Mock<T> Mok<T>() where T : class {
      return MokFac
        .Create<T>();
    }

    protected T[] BA<T>(params T[] items) {
      return items;
    }

    protected T CA<T>() {
      return ObjectFixture
        .Create<T>();
    }

    protected T[] CM<T>(int count) {
      return (count == 0)
               ? BA<T>()
               : ObjectFixture
                   .CreateMany<T>(count)
                   .ToArray();
    }

    protected void AssertAreEqual(object actual, object expected) {
      var result = DoCompare(actual, expected);
      Assert.That(result.AreEqual, Is.True, result.DifferencesString);
    }

    protected static bool AreEqual(object actual, object expected) {
      return DoCompare(actual, expected).AreEqual;
    }

    protected static TValue IsEq<TValue>(TValue x) {
      return Match
        .Create(value => AreEqual(value, x), () => It.IsAny<TValue>());
    }

    private static ComparisonResult DoCompare(object actual, object expected) {
      return _ObjectComparer
        .Compare(expected, actual);
    }

    private void VerifyMocks() {
      MokFac
        .VerifyAll();
    }

    protected MockRepository MokFac{get;private set;}
    protected Fixture ObjectFixture{get;private set;}
    private static readonly CompareLogic _ObjectComparer = new CompareLogic();
  }
}