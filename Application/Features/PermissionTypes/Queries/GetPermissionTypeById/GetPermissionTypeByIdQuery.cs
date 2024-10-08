﻿using Application.DTOs.Response;
using MediatR;

namespace Application.Features.PermissionTypes.Queries.GetPermissionTypeById
{
    public record GetPermissionTypeByIdQuery : IRequest<Result<GetPermissionTypeByIdDto>>
    {
        public int Id { get; set; }
        public GetPermissionTypeByIdQuery(int id)
        {
            Id = id;
        }
    }
}
