using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.DTOs.EgitimDatasets;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSetCommands;

public class DeleteEgitimDatasetCommandHandler : IRequestHandler<DeleteEgitimDatasetCommand, bool>
{
    // burada ilgili kayıt silinecek
    public async Task<bool> Handle(DeleteEgitimDatasetCommand request, CancellationToken cancellationToken)
    {
        return true;
    }
}
