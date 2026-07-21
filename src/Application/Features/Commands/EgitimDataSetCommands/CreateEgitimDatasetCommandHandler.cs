using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;

public sealed class CreateEgitimDatasetCommandHandler
    : IRequestHandler<CreateEgitimDatasetCommand, EgitimDatasetResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateEgitimDatasetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EgitimDatasetResponse> Handle(
        CreateEgitimDatasetCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new EgitimDataset
        {
            tetikleyici_metin = request.TetikleyiciMetin,
            type_num = request.TypeNum,
            sesTetikleyici_id = request.SesTetikleyiciId
        };

        await _context.EgitimDataset.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new EgitimDatasetResponse
        {
            Id = entity.Id,
            TetikleyiciMetin = entity.tetikleyici_metin,
            TypeNum = entity.type_num,
            SesTetikleyiciId = entity.sesTetikleyici_id
        };
    }
}