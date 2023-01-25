using DeclaredPersonsAdapter.Application.Requests.DeclaredPersonAnalyser;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using EPakapojumiDataServiceContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Adapters.Shared.Constants;

namespace DeclaredPersonsAdapter.IntegrationTests.ServicesTests;

public class DeclaredPersonODataServiceIntegrationTests : IClassFixture<DeclaredPersonsAdapterFixture>
{
    private readonly DeclaredPersonsAdapterFixture _fixture;

    public DeclaredPersonODataServiceIntegrationTests(DeclaredPersonsAdapterFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task TestGetGroupedSummary_ReturnsUnGrouped()
    {
        // Arrange
        const string districtName = "Riga";
        const int districtId = 23;

        var request = new DeclaredPersonAnalyserOptionsRequest()
        {
            District = 516,
            Source = ODataSourceConstants.Epakalpojumi.DeclaredPersons,
            Out = "res.json"
        };

        var logger = new Mock<ILogger<DeclaredPersonODataTestService>>().Object;
        
        var sut = new DeclaredPersonODataTestService(_fixture.DeclaredPersonODataRepository, _fixture.Mapper, logger);

        // Act
        var result = await sut.GetGroupedSummary(request);

        // Assert
        // Perform assertions on result
        Assert.NotNull(result);
        Assert.Empty(result.Data);
    }

    private Func<DeclaredPersonAnalyserOptionsRequest, Task<List<DeclaredPersons>>> GetDeclaredPersonsByRequestStub(List<DeclaredPersons> testDeclaredPersonsResponse)
    {
        return (req) => Task.FromResult(testDeclaredPersonsResponse);
    }

    public class DeclaredPersonODataTestService : DeclaredPersonODataService
    {
        public DeclaredPersonODataTestService(IDeclaredPersonODataRepository declaredPersonODataRepository, IMapper mapper, ILogger<DeclaredPersonODataService> logger) : base(declaredPersonODataRepository, mapper, logger)
        {
        }

        protected override async Task<List<DeclaredPersons>> GetDeclaredPersonsByRequest(DeclaredPersonAnalyserOptionsRequest request)
        {
            return new List<DeclaredPersons>()
            {
                new() { day = 1, district_id = request.District, district_name = "Test", month = 3, year = 2018, value = 23 },
                new() { day = 2, district_id = request.District, district_name = "Test", month = 3, year = 2018, value = 5 },
                new() { day = 2, district_id = request.District, district_name = "Test", month = 4, year = 2018, value = 11 },
                new() { day = 3, district_id = request.District, district_name = "Test", month = 3, year = 2019, value = 3 },
                new() { day = 4, district_id = request.District, district_name = "Test", month = 3, year = 2019, value = 16 },
                new() { day = 5, district_id = request.District, district_name = "Test", month = 3, year = 2020, value = 1 }
            };
        }
    }
}
