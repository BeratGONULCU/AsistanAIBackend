using GeminiAsistanBackend.Application.DTOs.SesTetikleyici;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries;

public sealed class GetSesTetikleyicileriMetinByIdQueryHandler : IRequestHandler<GetSesTetikleyicileriMetinByIdQuery, string?>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSesTetikleyicileriMetinByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<string?> Handle(GetSesTetikleyicileriMetinByIdQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.SesTetikleyicileri.GetByIdAsync(request.id, cancellationToken);

        if (entities?.TetikleyiciMetin == null)
        {
            return null;
        }

        return entities.TetikleyiciMetin;
    }
}
