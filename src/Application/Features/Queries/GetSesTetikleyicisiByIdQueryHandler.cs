using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Queries;

public sealed class GetSesTetikleyicisiByIdQueryHandler
{
    public readonly IUnitOfWork _unitOfWork;

    public GetSesTetikleyicisiByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SesTetikleyiciResponse> Handle(GetSesTetikleyicisiByIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.SesTetikleyicileri.GetByIdAsync(request.id,cancellationToken);

        if (entities is null)
            return null;

        return new SesTetikleyiciResponse
        {
            Id = entities.Id,
            TetikleyiciMetin = entities.TetikleyiciMetin,
            EklenmeTuru = entities.EklenmeTuru,
            llmConfidenceScore = entities.llm_confidence_score
        };

    }
}
