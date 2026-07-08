using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.SesTetikleyicileriQueries;

public sealed class GetRedmineSesTetikleyicileriQueryHandler : IRequestHandler<GetRedmineSesTetikleyicileriQuery, IReadOnlyCollection<SesTetikleyiciResponse>>
{
    private readonly IUnitOfWork _unitofwork;

    public GetRedmineSesTetikleyicileriQueryHandler(IUnitOfWork unitofwork)
    {
        _unitofwork = unitofwork;
    }

    public async Task<IReadOnlyCollection<SesTetikleyiciResponse>> Handle(GetRedmineSesTetikleyicileriQuery request, CancellationToken cancellationToken)
    {
        // burada  ses_tetikleyici içerisindeki eklenme_turu = REDMINE olan kayıtları
        // excel içerisine yazacak ve veri seti büyütülecek

        var entities = await _unitofwork.SesTetikleyicileri.GetAllAsync(cancellationToken).ConfigureAwait(false);

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
