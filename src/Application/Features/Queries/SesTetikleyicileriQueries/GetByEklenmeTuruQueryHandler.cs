using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;

public class GetByEklenmeTuruQueryHandler : IRequestHandler<GetByEklenmeTuruQuery, IReadOnlyCollection<SesTetikleyiciResponse>>
{
    private readonly IUnitOfWork _unitofwork;

    public GetByEklenmeTuruQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }       

    // burada girilecek eklenmeturu verisi string olarak geleceği için enum dönüşümü olması gerek.
    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> Handle(GetByEklenmeTuruQuery request, CancellationToken cancellationToken)
    {
        return await _unitofwork.SesTetikleyicileri.GetSesTetikleyiciByEklenmeTuru(request.eklenmeTuru,cancellationToken);
    }

}
