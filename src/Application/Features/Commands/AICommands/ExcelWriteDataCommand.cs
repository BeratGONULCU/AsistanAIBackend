using MediatR;
using System.IO;

namespace GeminiAsistanBackend.Application.Features.Commands.AICommands;

public sealed record ExcelWriteDataCommand(Stream FileStream, string FileName,long length) : IRequest<bool>;

