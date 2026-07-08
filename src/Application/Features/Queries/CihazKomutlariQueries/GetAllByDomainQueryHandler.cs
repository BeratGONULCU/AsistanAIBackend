using DocumentFormat.OpenXml.Spreadsheet;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;

public class GetAllByDomainQueryHandler : IRequestHandler<GetAllByDomainQuery,List<CihazKomutuResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllByDomainQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
   
    public async Task<List<CihazKomutuResponse>> Handle(GetAllByDomainQuery request , CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.CihazKomutu.GetAllByDomain(request.domain, cancellationToken);

        if (entities == null || !entities.Any())
        {
            return new List<CihazKomutuResponse>(); 
        }

        var response = entities.Select(e => new CihazKomutuResponse
        {
            Id = e.Id,
            type = e.type,
            domain = e.domain,
            target = e.target,
            operation = e.operation,
            CalisacakKod = e.CalisacakKod,
            Aciklama = e.Aciklama
        }).ToList();

        return response;
    }
}
