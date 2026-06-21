using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Queries;

public sealed class GetAllSesTetikleyicileriQueryHandler : IRequestHandler<GetAllSesTetikleyicileriQuery, List<SesTetikleyiciResponse>>
{
    public readonly IUnitOfWork _unitOfWork;

    public GetAllSesTetikleyicileriQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // burada bütün sestetikleyici verileri gelir.
    public async Task<List<SesTetikleyiciResponse>> Handle(GetAllSesTetikleyicileriQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.SesTetikleyicileri.GetAllAsync(cancellationToken);

        return entities
            .OrderBy(x => x.Id)
            .Select(x => new SesTetikleyiciResponse { 
                Id = x.Id,
                TetikleyiciMetin = x.TetikleyiciMetin,
                EklenmeTuru = x.EklenmeTuru,
                llmConfidenceScore = x.llm_confidence_score
            }).ToList();
    }
}
