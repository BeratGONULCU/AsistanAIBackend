using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Queries;

public sealed record GetCihazKomutuByIdQuery(int id) : IRequest<CihazKomutuResponse>;
