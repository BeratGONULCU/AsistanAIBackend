using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;

public sealed class CreateEgitimDatasetCommandHandler : IRequestHandler<CreateEgitimDatasetCommand, EgitimDatasetResponse>
{
    public readonly IApplicationDbContext _context;

    public CreateEgitimDatasetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<EgitimDatasetResponse> Handle(CreateEgitimDatasetCommand request, CancellationToken cancellationToken)
    {
        var entities = new EgitimDataset
        {
            tetikleyici_metin = request.TetikleyiciMetin,
            type_num = request.typenum ?? 0,
            sesTetikleyici_id = request.SesTetikleyiciId
        };

        await _context.EgitimDataset.AddAsync(entities);
        await _context.SaveChangesAsync(cancellationToken);

        return new EgitimDatasetResponse
        {
            Id = entities.Id,
            TetikleyiciMetin = entities.tetikleyici_metin,
            TypeNum = entities.type_num,
            SesTetikleyiciId = entities.sesTetikleyici_id
        };

    }
}
