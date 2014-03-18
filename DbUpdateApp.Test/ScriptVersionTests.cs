using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbUpdateApp.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace DbUpdateApp.Test
{
    [TestFixture]
    class ScriptVersionTests
    {
        [TestCase(new[] { 0 }, new[] { 1 })]
        [TestCase(new[] { 0, 3 }, new[] { 0, 4 })]
        [TestCase(new[] { 0, 0, 5 }, new[] { 0, 0, 6 })]
        [TestCase(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 5 })]
        [TestCase(new[] { 0, 0, 0, 0 }, new[] { 1, 0, 0, 0 })]
        [TestCase(new[] { 0, 3, 0, 0 }, new[] { 0, 4, 0, 0 })]
        [TestCase(new[] { 0, 0, 5, 0 }, new[] { 0, 0, 6, 0 })]
        [TestCase(new[] { 0, 0, 0, 4 }, new[] { 0, 0, 0, 5 })]
        public void Should_Generate_New_version(int[] baseVersion, int[] shouldBeVersion)
        {
            var newVersion = ScriptVersion.NewVersionNumber(baseVersion);

            Assert.AreEqual(shouldBeVersion, newVersion);
        }
    }
}
