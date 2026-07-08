using GeminiAsistanBackend.Application.DTOs.IslemLog;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.IslemLogQueries;

public class GetLogsByDateQueryHandler : IRequestHandler<GetLogsByDateQuery,IEnumerable<IslemLogResponse>>
{
    public readonly IUnitOfWork _unitOfWork;

    public GetLogsByDateQueryHandler(IUnitOfWork unitOfWork)
        { _unitOfWork = unitOfWork; }

    public async Task<IEnumerable<IslemLogResponse>> Handle(GetLogsByDateQuery request,CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.IslemLoglari.GetLogsByDate();

        var response = result.Select(x => new IslemLogResponse
        {
            Id = x.Id,
            DuyulanSes = x.DuyulanSes,
            Durum = x.Durum,
            CevapMetni = x.CevapMetni,
            TarihSaat = x.TarihSaat,
            KomutId = x.KomutId,
            raw_ai_json = x.raw_ai_json
        }).ToList();

        return response;
    }
}
