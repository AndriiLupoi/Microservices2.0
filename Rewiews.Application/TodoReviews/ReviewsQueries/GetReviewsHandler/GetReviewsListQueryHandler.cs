using AutoMapper;
using MediatR;
using Rewiews.Application.Common.DTOs;
using Rewiews.Application.TodoReviews.ReviewsQueries.GetReviews;
using Rewiews.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rewiews.Application.TodoReviews.ReviewsQueries.GetReviewsHandler
{
    public class GetReviewsListQueryHandler : IRequestHandler<GetReviewsListQuery, IReadOnlyCollection<ReviewDto>>
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;

        public GetReviewsListQueryHandler(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ReviewDto>> Handle(GetReviewsListQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _repository.ListByProductAsync(request.ProductId);
            return _mapper.Map<IReadOnlyCollection<ReviewDto>>(reviews);

        }

    }
}
