using GeminiAsistanBackend.Application.DTOs.AsistanYanit;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.AsistanYanitCommands;

public sealed class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand,Boolean>
{
    private readonly IApplicationDbContext _context;

    public DeleteSessionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Boolean> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            return false;
        }

        List<AsistanYanit> listSession = new();

        // burada sessionID değeri ile bir listeye atıcaz 

        listSession = await _context.AsistanYanit
            .Where(e => e.SessionId == request.sessionID)
            .ToListAsync(cancellationToken);


        if (listSession == null || !listSession.Any())
        {
            return false;
        }

        _context.AsistanYanit.RemoveRange(listSession);

        var check = await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
