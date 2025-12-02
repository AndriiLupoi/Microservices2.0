using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, string>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateReviewCommandHandler> _logger;

    public UpdateReviewCommandHandler(IReviewRepository reviewRepository, IMapper mapper, ILogger<UpdateReviewCommandHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<string> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating review Id: {Id} for ProductId: {ProductId}", request.Id, request.ProductId);

        var review = await _reviewRepository.GetByIdAsync(request.Id);

        if (review == null || review.ProductId != request.ProductId)
        {
            _logger.LogWarning("Review Id: {Id} not found for ProductId: {ProductId}", request.Id, request.ProductId);
            throw new NotFoundException("Review", request.Id);
        }

        _mapper.Map(request, review);
        await _reviewRepository.UpdateAsync(review);

        _logger.LogInformation("Review Id: {Id} updated successfully", review.Id);
        return $"Review '{review.Id}' updated successfully.";
    }
}
