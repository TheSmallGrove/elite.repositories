using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Elite.Repositories.Abstractions.Criterias;

namespace Elite.Repositories.Abstractions.Tests
{
    public class RestrictionCriteriaTests
    {
        [Fact]
        public void FromQuerySet_Without_Null()
        {
            // ARRANGE
            var arguments = new object[] { "a", 10, 123.45, Guid.NewGuid() };

            // ACT
            var result = RestrictionCriteria.FromQuerySet(
                new string[] { "Name == ??", "Test > ??", "Value.Contains(??)", "Id > ?? && Id < ??" }, arguments);

            // ASSERT
            result.RestrictionTemplate.Should().Be("(Name == @0) && (Test > @1) && (Value.Contains(@2)) && (Id > @3 && Id < @3)");
            result.Arguments.Should().BeEquivalentTo(arguments);
        }

        [Fact]
        public void FromQuerySet_With_Null()
        {
            // ARRANGE
            var arguments = new object[] { "a", 10, null, Guid.NewGuid() };

            // ACT
            var result = RestrictionCriteria.FromQuerySet(
                new string[] { "Name == ??", "Test > ??", "Value.Contains(??)", "Id > ?? && Id < ??" }, arguments);

            // ASSERT
            result.RestrictionTemplate.Should().Be("(Name == @0) && (Test > @1) && (Id > @2 && Id < @2)");
            result.Arguments.Should().BeEquivalentTo(new object[] { arguments[0], arguments[1], arguments[3] });
        }

        [Fact]
        public void FromQuerySet_With_Empty()
        {
            // ARRANGE
            var arguments = new object[] { null, null, null, null };

            // ACT
            var result = RestrictionCriteria.FromQuerySet(
                new string[] { "Name == ??", "Test > ??", "Value.Contains(??)", "Id > ?? && Id < ??" }, arguments);

            // ASSERT
            result.RestrictionTemplate.Should().Be("1 == 1");
            result.Arguments.Should().BeEquivalentTo(new object[0]);
        }
    }
}
