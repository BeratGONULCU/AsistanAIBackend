using MediatR;
using System.IO;

namespace GeminiAsistanBackend.Application.Features.Commands;

// Artık hiçbir web kütüphanesine bağımlı değil, tamamen saf C#!
public sealed record ExcelWriteDataCommand(Stream FileStream, string FileName,long length) : IRequest<bool>;

