using GeminiAsistanBackend.Application.DTOs.TetikleyiciKomutlar;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries;

public sealed class GetAllTetikleyiciKomutQueryHandler : IRequestHandler<GetAllTetikleyiciKomutQuery, List<TetikleyiciKomutReponse>>
{
    public IUnitOfWork _unitOfWork;

    public GetAllTetikleyiciKomutQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<TetikleyiciKomutReponse>> Handle(GetAllTetikleyiciKomutQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.TetikleyiciKomutlar.GetAllAsync(cancellationToken);

        return entities
            .Select(x => new TetikleyiciKomutReponse
            {
                TetikleticiId = x.TetikleyiciId,
                KomutId = x.KomutId,
            }).ToList();
    }

}
