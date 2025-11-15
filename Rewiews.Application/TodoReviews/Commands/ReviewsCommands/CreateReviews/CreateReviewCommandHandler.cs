using AutoMapper;
using MediatR;
using Rewiews.Domain.Entities;
using Rewiews.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.Commands.ReviewsCommands.CreateReviews
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, string>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public CreateReviewCommandHandler(
            IReviewRepository reviewRepository,
            IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = _mapper.Map<Review>(request);
            review.ProductId = request.ProductId;

            await _reviewRepository.AddAsync(review);

            return review.Id!;
        }
    }
}
