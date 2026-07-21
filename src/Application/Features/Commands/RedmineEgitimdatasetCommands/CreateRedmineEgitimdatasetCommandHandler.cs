using GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;

public sealed class CreateRedmineEgitimdatasetCommandHandler
    : IRequestHandler<CreateRedmineEgitimdatasetCommand, RedmineEgitimdatasetResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateRedmineEgitimdatasetCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RedmineEgitimdatasetResponse> Handle(
        CreateRedmineEgitimdatasetCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new RedmineEgitimDataset
        {
            redmine_tetikleyici_metin = request.RedmineTetikleyiciMetin,
            action = request.Action,
            sesTetikleyici_id = request.SesTetikleyiciId
        };

        await _context.RedmineEgitimDataset.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RedmineEgitimdatasetResponse
        {
            Id = entity.Id,
            redmine_tetikleyici_metin = entity.redmine_tetikleyici_metin,
            action = entity.action,
            sesTetikleyici_id = entity.sesTetikleyici_id
        };
    }
}