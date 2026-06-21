using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;

namespace GeminiAsistanBackend.Application.Queries;

public sealed record GetAllCihazKomutlariQuery(): IRequest<List<CihazKomutuResponse>>;

