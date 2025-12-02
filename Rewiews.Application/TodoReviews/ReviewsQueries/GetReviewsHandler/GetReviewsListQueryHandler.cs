using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews;
using Rewiews.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class GetReviewsListQueryHandler : IRequestHandler<GetReviewsListQuery, IReadOnlyCollection<ReviewDto>>
{
    private readonly IReviewRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetReviewsListQueryHandler> _logger;

    public GetReviewsListQueryHandler(IReviewRepository repository, IMapper mapper, ILogger<GetReviewsListQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<ReviewDto>> Handle(GetReviewsListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching reviews list for ProductId: {ProductId}", request.ProductId);

        var reviews = await _repository.ListByProductAsync(request.ProductId);
        var dtos = _mapper.Map<IReadOnlyCollection<ReviewDto>>(reviews);

        _logger.LogInformation("Fetched {Count} reviews for ProductId: {ProductId}", dtos.Count, request.ProductId);
        return dtos;
    }
}
