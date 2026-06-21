using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries;

public sealed class GetAllEgitimDatasetQueryHandler : IRequestHandler<GetAllEgitimDatasetQuery, List<EgitimDatasetResponse>>
{
    public IUnitOfWork _unitOfWork;

    public GetAllEgitimDatasetQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<EgitimDatasetResponse>> Handle(GetAllEgitimDatasetQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.EgitimDataset.GetAllAsync(cancellationToken);

        return entities
            .OrderBy(e => e.Id)
            .Select(x => new EgitimDatasetResponse
            {
                Id = x.Id,
                TetikleyiciMetin = x.tetikleyici_metin,
                TypeNum = x.type_num,
                SesTetikleyiciId = x.sesTetikleyici_id
            }).ToList();
    }
}
