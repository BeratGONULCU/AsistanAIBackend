using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Commands;

// bunun record yapılma sebebi ;
// bu nesnelerin sisteme girdikten sonra yolda kazara değiştirilmesini önleyen değişmezlik (immutability) özelliğine sahip olmalarıdır
public sealed record DeleteEgitimDatasetCommand(int Id) : IRequest<bool>;
