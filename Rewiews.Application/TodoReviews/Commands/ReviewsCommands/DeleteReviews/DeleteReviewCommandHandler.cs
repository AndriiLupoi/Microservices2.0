using MediatR;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.DeleteReviews
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, string>
    {
        private readonly IReviewRepository _reviewRepository;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<string> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id);

            if (review == null || review.ProductId != request.ProductId)
                return $"Review '{request.Id}' not found.";

            await _reviewRepository.DeleteAsync(request.Id);

            return $"Review '{request.Id}' deleted successfully.";
        }
    }
}
