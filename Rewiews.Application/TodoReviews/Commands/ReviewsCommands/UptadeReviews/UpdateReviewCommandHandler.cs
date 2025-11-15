using AutoMapper;
using MediatR;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.UptadeReviews
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, string>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id);

            if (review == null || review.ProductId != request.ProductId)
                throw new NotFoundException("Review", request.Id);

            _mapper.Map(request, review);

            await _reviewRepository.UpdateAsync(review);

            return $"Review '{review.Id}' updated successfully.";
        }
    }
}
