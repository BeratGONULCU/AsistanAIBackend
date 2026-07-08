using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.EgitimDataQueries;

public sealed class GetAllUntrainedEgitimDataQueryHandler : IRequestHandler<GetAllUntrainedEgitimDataQuery, List<EgitimDatasetResponse>>
{
    public readonly IUnitOfWork _unitOfWork;

    public GetAllUntrainedEgitimDataQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<List<EgitimDatasetResponse>> Handle(GetAllUntrainedEgitimDataQuery request, CancellationToken cancellationToken)
    {
        var entities = await _unitOfWork.EgitimDataset.GetAllAsync(cancellationToken);

        return entities
            .OrderBy(e => e.Id)
            .Select(e => new EgitimDatasetResponse { 
            Id = e.Id,
            TetikleyiciMetin = e.tetikleyici_metin,
            TypeNum = e.type_num,
            SesTetikleyiciId = e.sesTetikleyici_id
            }).ToList();
    }

    
}