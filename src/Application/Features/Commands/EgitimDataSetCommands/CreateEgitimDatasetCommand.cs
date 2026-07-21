using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;

public sealed record CreateEgitimDatasetCommand(
    string TetikleyiciMetin,
    int TypeNum,
    int SesTetikleyiciId
) : IRequest<EgitimDatasetResponse>;