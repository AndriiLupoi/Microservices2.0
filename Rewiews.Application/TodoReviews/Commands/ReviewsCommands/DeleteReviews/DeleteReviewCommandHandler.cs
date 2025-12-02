using MediatR;
using Microsoft.Extensions.Logging;
using Rewiews.Application.TodoReviews.Commands.ReviewsCommands.DeleteReviews;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, string>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<DeleteReviewCommandHandler> _logger;

    public DeleteReviewCommandHandler(IReviewRepository reviewRepository, ILogger<DeleteReviewCommandHandler> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<string> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting review Id: {Id} for ProductId: {ProductId}", request.Id, request.ProductId);

        var review = await _reviewRepository.GetByIdAsync(request.Id);

        if (review == null || review.ProductId != request.ProductId)
        {
            _logger.LogWarning("Review Id: {Id} for ProductId: {ProductId} not found", request.Id, request.ProductId);
            return $"Review '{request.Id}' not found.";
        }

        await _reviewRepository.DeleteAsync(request.Id);

        _logger.LogInformation("Review Id: {Id} deleted successfully", request.Id);
        return $"Review '{request.Id}' deleted successfully.";
    }
}
