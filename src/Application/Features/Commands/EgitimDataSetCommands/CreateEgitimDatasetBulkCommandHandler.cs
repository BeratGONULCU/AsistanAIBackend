using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;

public sealed class CreateEgitimDatasetBulkCommandHandler
    : IRequestHandler<CreateEgitimDatasetBulkCommand, List<EgitimDatasetResponse>>
{
    private readonly IApplicationDbContext _context;

    public CreateEgitimDatasetBulkCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EgitimDatasetResponse>> Handle(
        CreateEgitimDatasetBulkCommand request,
        CancellationToken cancellationToken)
    {
        if (request.items is null || request.items.Count == 0)
            return new List<EgitimDatasetResponse>();

        var createdEntities = new List<EgitimDataset>();

        foreach (var item in request.items)
        {
            var entity = new EgitimDataset
            {
                tetikleyici_metin = item.TetikleyiciMetin,
                type_num = item.TypeNum ?? 0,
                sesTetikleyici_id = item.sesTetikleyiciId
            };

            await _context.EgitimDataset.AddAsync(entity, cancellationToken);
            createdEntities.Add(entity);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return createdEntities.Select(x => new EgitimDatasetResponse
        {
            Id = x.Id,
            TetikleyiciMetin = x.tetikleyici_metin,
            TypeNum = x.type_num,
            SesTetikleyiciId = x.sesTetikleyici_id
        }).ToList();
    }
}