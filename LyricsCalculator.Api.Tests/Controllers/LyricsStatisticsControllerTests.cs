using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LyricsCalculator.Api.Controllers;
using LyricsCalculator.Processor;
using LyricsCalculator.Processor.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LyricsCalculator.Api.Tests.Controllers
{
    [TestFixture]
    public class LyricsStatisticsControllerTests
    {
        private Mock<ISearchRepository> _searchRepositoryMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _searchRepositoryMock = new Mock<ISearchRepository>();
        }

        [Test]
        public void Ctor_SearchRepositoryNull_ThrowsException()
        {
            Func<LyricsStatisticsController> act = () => new LyricsStatisticsController(null);

            act.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("searchRepository");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("a")]
        public async Task Search_NullOrCriteriaMinLengthNotMet_BadRequestAsync(string artistName)
        {
            var sut = GetDefaultSut();

            var response = await sut.Search(artistName);

            response.Result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task Search_ValidCriteriaAndNoResults_ReturnsNoContent()
        {
            var sut = GetDefaultSut();

            var response = await sut.Search(_fixture.Create<string>());

            response.Result.Should().BeOfType<NoContentResult>();
        }

        [Test]
        public async Task Search_ValidCriteriaAndHasResults_ReturnsResults()
        {
            var stubResults = _fixture.Create<Result>();

            _searchRepositoryMock.Setup(m => m.GetLyricsStatisticsAsync(stubResults.Artist))
                .Returns(Task.FromResult(stubResults));

            var sut = GetDefaultSut();

            var response = await sut.Search(stubResults.Artist);

            response.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().Be(stubResults);
        }

        private LyricsStatisticsController GetDefaultSut() => new LyricsStatisticsController(_searchRepositoryMock.Object);
    }
}