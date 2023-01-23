using AutoMapper;
using DeclaredPersonsAdapter.Application.Interfaces.Repositories;
using DeclaredPersonsAdapter.Application.Interfaces.Services;
using DeclaredPersonsAdapter.Application.Responses.DeclaredPersons.Get;
using Microsoft.Extensions.Logging;
using Shared.Wrapper;
using Shared.Wrapper.Interfaces;

namespace DeclaredPersonsAdapter.Infrastructure.Services;

public class DeclaredPersonODataService : IDeclaredPersonODataService
{
    private readonly IDeclaredPersonODataRepository _declaredPersonODataRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeclaredPersonODataService> _logger;

    public DeclaredPersonODataService(
        IDeclaredPersonODataRepository declaredPersonODataRepository, 
        IMapper mapper, 
        ILogger<DeclaredPersonODataService> logger
        )
    {
        _declaredPersonODataRepository = declaredPersonODataRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IResult<List<GetDeclaredPersonResponse>>> GetAll()
    {
        try
        {
            var declaredPersonOData = await _declaredPersonODataRepository.GetAllAsync();

            var resultMapped = _mapper.Map<List<GetDeclaredPersonResponse>>(declaredPersonOData);

            return await Result<List<GetDeclaredPersonResponse>>.SuccessAsync(resultMapped);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
