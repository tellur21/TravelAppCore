using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.UseCases;

public class GetAllTravelPackagesHandler : IRequestHandler<GetAllTravelPackagesQuery, List<TravelPackage>>
{
    private readonly ITravelPackageRepository _repo;

    public GetAllTravelPackagesHandler(ITravelPackageRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<TravelPackage>> Handle(GetAllTravelPackagesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
