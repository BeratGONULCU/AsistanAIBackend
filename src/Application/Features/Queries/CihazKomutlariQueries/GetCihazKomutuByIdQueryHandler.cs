using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;

public sealed class GetCihazKomutuByIdQueryHandler : IRequestHandler<GetCihazKomutuByIdQuery,CihazKomutuResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCihazKomutuByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CihazKomutuResponse> Handle(GetCihazKomutuByIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CihazKomutlari.GetByIdAsync(request.id,cancellationToken);

        if (entities == null)
        {
            return null;
        }

        return new CihazKomutuResponse
        {
            Id = entities.Id,
            type = entities.type,
            domain = entities.domain,
            target = entities.target,
            operation = entities.operation,
            CalisacakKod = entities.CalisacakKod,
            Aciklama = entities.Aciklama
        };
    }
}
