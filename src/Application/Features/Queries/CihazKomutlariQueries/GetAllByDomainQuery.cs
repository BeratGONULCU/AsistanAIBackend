using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;

public sealed record GetAllByDomainQuery(string domain) : IRequest<List<CihazKomutuResponse>>;
