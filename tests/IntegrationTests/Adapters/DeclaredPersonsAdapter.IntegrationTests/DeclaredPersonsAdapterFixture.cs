using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Infrastructure.Mappings;
using DeclaredPersonsAdapter.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeclaredPersonsAdapter.IntegrationTests;

public class DeclaredPersonsAdapterFixture : IDisposable
{
    public IDeclaredPersonODataService DeclaredPersonODataService { get; set; }
    public IDeclaredPersonODataRepository DeclaredPersonODataRepository { get; set; }
    public IMapper Mapper { get; set; }

    public DeclaredPersonsAdapterFixture()
    {
        DeclaredPersonODataRepository = new Mock<IDeclaredPersonODataRepository>().Object;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<DeclaredPersonMappingProfile>();
        });

        Mapper = config.CreateMapper();
        
        var logger = new Mock<ILogger<DeclaredPersonODataService>>().Object;

        DeclaredPersonODataService = new DeclaredPersonODataService(DeclaredPersonODataRepository, Mapper, logger);
    }

    public void Dispose()
    {
    }
}
