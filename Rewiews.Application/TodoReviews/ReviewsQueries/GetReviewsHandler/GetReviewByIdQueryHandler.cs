using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
{
    private readonly IReviewRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetReviewByIdQueryHandler> _logger;

    public GetReviewByIdQueryHandler(IReviewRepository repository, IMapper mapper, ILogger<GetReviewByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching review Id: {Id}", request.Id);

        var review = await _repository.GetByIdAsync(request.Id);

        if (review == null)
        {
            _logger.LogWarning("Review Id: {Id} not found", request.Id);
            throw new NotFoundException("Review", request.Id);
        }

        _logger.LogInformation("Review Id: {Id} retrieved successfully", request.Id);
        return _mapper.Map<ReviewDto>(review);
    }
}
