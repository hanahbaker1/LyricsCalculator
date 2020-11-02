using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using LyricsCalculator.Processor.Models;
using Moq;
using NUnit.Framework;

namespace LyricsCalculator.Processor.Tests
{
    [TestFixture]
    public class SearchRepositoryTests
    {
        private Mock<ISongsClient> _songsClientMock;
        private Mock<ILyricsClient> _lyricsClientMock;
        private Fixture _fixture;
        private const string Artist = "Bill Withers";

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _songsClientMock = new Mock<ISongsClient>();
            _lyricsClientMock = new Mock<ILyricsClient>();

            _songsClientMock.Setup(client => client.GetSongsByArtistAsync(Artist))
                .Returns(StubSongClientResponse);

            _lyricsClientMock.Setup(client => client.GetLyricsByArtistAndTitle(Artist, "Ain't No Sunshine"))
                .Returns(Task.FromResult(AintNoSunshineLyricsResponse()));
        }

        [Test]
        public void Ctor_SongsClientNull_ThrowsException()
        {
            Func<SearchRepository> act = () => new SearchRepository(null, _lyricsClientMock.Object);

            act.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("songClient");
        }

        [Test]
        public void Ctor_LyricsClientNull_ThrowsException()
        {
            Func<SearchRepository> act = () => new SearchRepository(_songsClientMock.Object, null);

            act.Should().ThrowExactly<ArgumentNullException>().And.ParamName.Should().Be("lyricsClient");
        }

        [Test]
        public async Task GetLyricsStatisticsAsync_WhenCalled_ShouldCallSongsClient()
        {
            _songsClientMock.Setup(client => client.GetSongsByArtistAsync(It.IsAny<string>())).Returns((_fixture.Create<Artist>(),
                _fixture.Create<IReadOnlyList<string>>()));

            var sut = GetDefaultSut();
            await sut.GetLyricsStatisticsAsync(Artist);

            _songsClientMock.Verify(client => client.GetSongsByArtistAsync(Artist), Times.Once);
        }

        [Test]
        public async Task GetLyricsStatisticsAsync_WhenCalled_ShouldCallLyricsClient()
        {
            var song = _fixture.Create<string>();
            _songsClientMock.Setup(client => client.GetSongsByArtistAsync(It.IsAny<string>())).Returns((_fixture.Create<Artist>(),
                new List<string> { song }));
            _lyricsClientMock.Setup(client => client.GetLyricsByArtistAndTitle(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(_fixture.Create<SongLyrics>()));

            var sut = GetDefaultSut();
            await sut.GetLyricsStatisticsAsync(Artist);

            _lyricsClientMock.Verify(client => client.GetLyricsByArtistAndTitle(Artist, song), Times.Once);
        }

        [Test]
        public async Task GetLyricsStatisticsAsync_WhenLyricsClientReturnsNull_ShouldReturnNull()
        {
            _lyricsClientMock.Setup(client => client.GetLyricsByArtistAndTitle(Artist, "Ain't No Sunshine"))
                .Returns(Task.FromResult<SongLyrics>(null));

            var sut = GetDefaultSut();
            var result = await sut.GetLyricsStatisticsAsync(Artist);

            result.Should().BeNull();
        }

        [Test]
        public async Task GetLyricsStatisticsAsync_ValidData_SetsResponse()
        {
            _lyricsClientMock.SetupSequence(client => client.GetLyricsByArtistAndTitle(Artist, It.IsAny<string>()))
                .Returns(Task.FromResult(AintNoSunshineLyricsResponse()))
                .Returns(Task.FromResult(LovelyDayLyricsResponse()));
            var listOfLyrics = new List<SongLyrics> { AintNoSunshineLyricsResponse(), LovelyDayLyricsResponse() };

            var stubResponse = new Result
            {
                Artist = Artist,
                SongsAnalysed = 2,
                AverageWords = Math.Round(listOfLyrics.Average(lyrics => lyrics.WordCountGrouping.WordCount), 2),
                AverageDistinctWords = Math.Round(listOfLyrics.Average(lyrics => lyrics.WordCountGrouping.DistinctWordCount), 2),
                SongWithMostWords = listOfLyrics.Max(),
                SongWithFewestWords = listOfLyrics.Min()
            };

            var sut = GetDefaultSut();
            var result = await sut.GetLyricsStatisticsAsync(Artist);

            result.Should().BeEquivalentTo(stubResponse);
        }

        private SearchRepository GetDefaultSut() => new SearchRepository(_songsClientMock.Object, _lyricsClientMock.Object);

        private (Artist artist, IReadOnlyList<string> songs) StubSongClientResponse()
        {
            var artist = new Artist
            {
                Id = Guid.Empty.ToString(),
                Name = Artist
            };
            var songs = new List<string> { "Ain't No Sunshine", "Lovely Day" };

            return (artist, songs);
        }

        private SongLyrics AintNoSunshineLyricsResponse()
        {
            return new SongLyrics("Ain't No Sunshine", AintNoSunshineLyrics());
        }

        private SongLyrics LovelyDayLyricsResponse()
        {
            return new SongLyrics("Lovely Day", LovelyDayLyrics());
        }

        private string AintNoSunshineLyrics()
        {
            return "Ain't no sunshine when she's gone.\nIt's not warm when she's away." +
                   "\nAin't no sunshine when she's gone\nand she's always gone too long\nanytime she goes away." +
                   "\nWonder this time where she's gone,\n\nwonder if she's gone to stay\n\nAin't no sunshine when she's gone" +
                   "\n\nand this house just ain't no home\n\nanytime she goes away.\n\nAnd I know, I know, I know, I know, I know," +
                   "\n\nI know, I know, I know, I know, I know, I know,\n\nI know, I know, I know, I know, I know, I know," +
                   "\n\n I know, I know, I know, I know, I know, I know,\n\nI know, I know, I know\n\nHey, I ought to leave the young thing alone," +
                   "\n\nbut ain't no sunshine when she's gone,\n\nain't no sunshine when she's gone,\n\nonly darkness everyday." +
                   "\n\nAin't no sunshine when she's gone,\n\nand this house just ain't no home\n\nanytime she goes away.\n\nAnytime she goes away." +
                   "\n\nAnytime she goes away.\n\nAnytime she goes away.\n\nAnytime she goes away";
        }

        private string LovelyDayLyrics()
        {
            return "When I wake up in the morning, love\r\nAnd the sunlight hurts my eyes\r\nAnd something without warning, love" +
                   "\r\nBears heavy on my mind\r\nThen I look at you\n\nAnd the world's alright with me\n\nJust one look at you" +
                   "\n\nAnd I know it's gonna be\n\nA lovely day\n\n... lovely day, lovely day, lovely day ..." +
                   "\n\n\n\nWhen the day that lies ahead of me\n\nSeems impossible to face\n\nWhen someone else instead of me" +
                   "\n\nAlways seems to know the way\n\n\n\nThen I look at you\n\nAnd the world's alright with me\n\nJust one look at you" +
                   "\n\nAnd I know it's gonna be\n\nA lovely day.....\n\n\n\nWhen the day that lies ahead of me\n\nSeems impossible to face" +
                   "\n\nWhen someone else instead of me\n\nAlways seems to know the way\n\n\n\nThen I look at you\n\nAnd the world's alright with me" +
                   "\n\nJust one look at you\n\nAnd I know it's gonna be\n\nA lovely day......";
        }
    }
}