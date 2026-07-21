using GeminiAsistanBackend.Application.DTOs.RedmineEgitimdataset;
using MediatR;

namespace GeminiAsistanBackend.Application.Features.Commands.RedmineEgitimdatasetCommands;

public sealed record CreateRedmineEgitimdatasetCommand(
    string RedmineTetikleyiciMetin,
    string Action,
    int SesTetikleyiciId
) : IRequest<RedmineEgitimdatasetResponse>;