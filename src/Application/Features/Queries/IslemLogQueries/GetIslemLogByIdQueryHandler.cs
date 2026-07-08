using GeminiAsistanBackend.Application.Commands;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Application.DTOs.IslemLog;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public sealed class GetIslemLogByIdQueryHandler  : IRequestHandler<GetIslemLogByIdQuery,IslemLogResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetIslemLogByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IslemLogResponse> Handle(GetIslemLogByIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.IslemLoglari.GetByIdAsync(request.id, cancellationToken);

        if (entities == null)
        { 
            return null;
        }

        return new IslemLogResponse
        {
            Id = entities.Id,
            DuyulanSes = entities.DuyulanSes,
            Durum = entities.Durum,
            CevapMetni = entities.CevapMetni,
            TarihSaat = DateTime.Now,
            KomutId = entities.KomutId,
            raw_ai_json = entities.raw_ai_json,
        };
    }
}
