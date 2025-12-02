using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.CreateReviews;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, string>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateReviewCommandHandler> _logger;

    public CreateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IMapper mapper,
        ILogger<CreateReviewCommandHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating review for ProductId: {ProductId}", request.ProductId);

        var review = _mapper.Map<Review>(request);
        review.ProductId = request.ProductId;

        await _reviewRepository.AddAsync(review);

        _logger.LogInformation("Review created successfully with Id: {Id}", review.Id);
        return review.Id!;
    }
}
