using System;
using FluentAssertions;
using NUnit.Framework;

namespace LyricsCalculator.Common.Tests
{
    [TestFixture]
    public class UriExtensionTests
    {
        [Test]
        public void GuaranteeEndingSlash_WhenUriDoesNotEndWithASlash_ShouldAddTrailingSlashToEncodedUri()
        {
            var uri = new Uri("http://example.com");

            var result = uri.GuaranteeEndingSlash();

            result.AbsoluteUri.Should().Be("http://example.com/");
        }

        [Test]
        public void GuaranteeEndingSlash_WhenUriAndPathDoesNotEndWithASlash_ShouldAddTrailingSlashToEncodedUri()
        {
            var uri = new Uri("http://example.com/api/method");

            var result = uri.GuaranteeEndingSlash();

            result.AbsoluteUri.Should().Be("http://example.com/api/method/");
        }

        [Test]
        public void GuaranteeEndingSlash_WhenUriAlreadyEndsWithASlash_ShouldNotModifyUri()
        {
            var uri = new Uri("http://example.com/");

            var result = uri.GuaranteeEndingSlash();

            result.Should().BeSameAs(uri);
        }

        [Test]
        public void GuaranteeEndingSlash_WhenUriAndPathAlreadyEndsWithASlash_ShouldNotModifyUri()
        {
            var uri = new Uri("http://example.com/api/method/");

            var result = uri.GuaranteeEndingSlash();

            result.Should().BeSameAs(uri);
        }
    }
}
