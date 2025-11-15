using AutoMapper;
using MediatR;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.Common.Exceptions;
using Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews;
using Rewiews.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.ReviewsQueries.GetReviewsHandler
{
    public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto>
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetReviewByIdQueryHandler(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
        {
            var review = await _repository.GetByIdAsync(request.Id);

            if (review == null)
                throw new NotFoundException("Review", request.Id);

            return _mapper.Map<ReviewDto>(review);
        }
    }
}
