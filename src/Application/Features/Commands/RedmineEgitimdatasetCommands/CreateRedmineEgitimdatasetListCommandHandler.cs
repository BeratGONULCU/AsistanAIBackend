using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;

public sealed class CreateRedmineEgitimdatasetListCommandHandler : IRequestHandler<CreateRedmineEgitimdatasetListCommand,List<RedmineEgitimdatasetResponse>>
{
    private IApplicationDbContext _context;

    public CreateRedmineEgitimdatasetListCommandHandler(IApplicationDbContext context)
    {  
        _context = context; 
    }

    // burada dışarıdan tablo içine veri girişi olacak. burada herhangi bir çoklu veri girişi sağlanır mı?
    public async Task<List<RedmineEgitimdatasetResponse>> Handle(CreateRedmineEgitimdatasetListCommand request,CancellationToken cancellationToken)
    {
        if (request.items is null || request.items.Count == 0)
            return new List<RedmineEgitimdatasetResponse>();

        var createdEntities = new List<RedmineEgitimDataset>();

        foreach (var item in request.items)
        {
            var entity = new RedmineEgitimDataset
            {
                redmine_tetikleyici_metin = item.redmine_tetikleyici_metin,
                action = item.action,
                sesTetikleyici_id = item.sesTetikleyici_id
            };

            await _context.RedmineEgitimDataset.AddAsync(entity, cancellationToken);
            createdEntities.Add(entity);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return createdEntities.Select(x => new RedmineEgitimdatasetResponse
        {
            Id = x.Id,
            redmine_tetikleyici_metin = x.redmine_tetikleyici_metin,
            action = x.action,
            sesTetikleyici_id = x.sesTetikleyici_id
        }).ToList();
    }
}
