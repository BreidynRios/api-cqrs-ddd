using MediatR;

namespace Application.Features.Securities.Queries.GetBearerToken
{
    public record GetBearerTokenQuery : IRequest<string>
    {
        public string ClientId { get; set; }
    }
}
