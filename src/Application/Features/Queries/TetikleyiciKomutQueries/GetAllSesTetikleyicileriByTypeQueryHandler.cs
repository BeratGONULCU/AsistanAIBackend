using DocumentFormat.OpenXml.Bibliography;
using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.TetikleyiciKomutQueries;

 public class GetAllSesTetikleyicileriByTypeQueryHandler : IRequestHandler<GetAllSesTetikleyicileriByTypeQuery, IReadOnlyCollection<SesTetikleyiciResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllSesTetikleyicileriByTypeQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> Handle(GetAllSesTetikleyicileriByTypeQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.SesTetikleyicileri.GetSesTetikleyiciByType(request.type, cancellationToken);

        return result;
    }

}
