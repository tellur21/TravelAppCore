using Domain.Entities;
using MediatR;

namespace Application.UseCases;

public record GetAllTravelPackagesQuery : IRequest<List<TravelPackage>>;
