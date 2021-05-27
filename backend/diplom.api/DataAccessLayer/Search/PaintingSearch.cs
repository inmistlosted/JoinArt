using diplom.api.Controllers;
using diplom.api.Models;
using diplom.api.Providers;
using diplom.api.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using diplom.api.Models.RequestModels;
using Microsoft.AspNetCore.Http;
using diplom.api.Models.ResponseModels;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace diplom.api.DataAccessLayer.Search
{
    public class PaintingSearch
    {
        const int OwnerId = 1;
        const string Title = "title";
        const string Materials = "materials";
        const string Painter = "painter";
        const double Price = 10;
        const string Description = "description";
        const bool Status = true;
        const IFormFile Image = null;
        const string GenresIds = "1,2";

        private readonly IPaintingProvider _paintingProvider;

        public async Task FindPainting(string queryValue, string aliasName)
        {
            await TestGetPaintingMethod();

            ElasticClient client = new ElasticClient();

            ISearchResponse<Painting> response = await client.SearchAsync<Painting>(search => search
                .Index(aliasName)
                .Query(query => query
                    .Bool(boolean => boolean
                        .Must(must => must
                            .DisMax(dm => dm
                                .Queries(
                                    qs => qs.MultiMatch(match => match
                                        .Fields(fields => fields
                                            .Field(field => field.Title)
                                            .Field(field => field.Materials)
                                            .Field(field => field.Description)
                                        )
                                        .Operator(Operator.And)
                                        .Query(queryValue)
                                        .Boost(1000000)
                                    ),
                                    qs => qs.ConstantScore(cs => cs
                                        .Filter(f => f.Match(m => m
                                                .Field(field => field.Title)
                                                .Query(queryValue)
                                                .MinimumShouldMatch("100%")
                                            )
                                        )
                                        .Boost(100000)
                                    )
                                )
                            )
                        )
                    )
                )
            );

        }

        [Fact]
        public async Task ControllerShouldReturnTopGenres()
        {
            Mock<DataAccessSettings> dataAccessSettings = new Mock<DataAccessSettings>();
            Mock<IDataAccessAdapter> dataAccessLayer = new Mock<IDataAccessAdapter>(dataAccessSettings);
            Mock<IMemoryCache> cache = new Mock<IMemoryCache>();
            Mock<IPaintingProvider> paintingProvider = new Mock<IPaintingProvider>(dataAccessLayer, cache);
            Mock<IGenreProvider> genreProvider = new Mock<IGenreProvider>(dataAccessLayer, cache);
            Mock<IOptions<AppSettings>> settings = new Mock<IOptions<AppSettings>>();

            GenresController controller = new GenresController(
                dataAccessLayer.Object, 
                cache.Object, 
                settings.Object, 
                genreProvider.Object, 
                paintingProvider.Object);

            IActionResult result = await controller.GetTopGenres();

            result
                .Should()
                .BeOfType<IList<Genre>>()
                .And
                .NotBeNull();
        }

        [TestMethod]
        private async Task TestGetPaintingMethod()
        {
            AddPaintingRequestModel paintingInfo = new AddPaintingRequestModel
            {
                OwnerId = OwnerId,
                Title = Title,
                Materials = Materials,
                Painter = Painter,
                Price = Price,
                Description = Description,
                Status = Status,
                Image = Image,
                GenresIds = GenresIds,
            };

            AddPaintingResponseModel addPaintingResponse = await _paintingProvider.AddPainting(paintingInfo);

            PaintingResponseModel painting = await _paintingProvider.GetPainting(addPaintingResponse.PaintingId, 0);

            await _paintingProvider.DeletePainting(addPaintingResponse.PaintingId);

            Assert.IsNotNull(painting);
            Assert.AreEqual(painting.Owner.UserId, OwnerId);
            Assert.AreEqual(painting.Title, Title);
            Assert.AreEqual(painting.Materials, Materials);
            Assert.AreEqual(painting.Painter, Painter);
            Assert.AreEqual(painting.Price, Price);
            Assert.AreEqual(painting.Description, Description);
            Assert.AreEqual(painting.Status, Status);
        }
    }
}
