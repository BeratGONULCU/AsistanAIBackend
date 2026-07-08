using GeminiAsistanBackend.Application.DTOs.CihazKomut;
using GeminiAsistanBackend.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.CihazKomutlariQueries;

public class CheckDuplicateCihazKomutlariQueryHandler : IRequestHandler<CheckDuplicateCihazKomutlariQuery, bool?>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckDuplicateCihazKomutlariQueryHandler(IUnitOfWork unitOfWork) {  _unitOfWork = unitOfWork; }

    public async Task<bool?> Handle(CheckDuplicateCihazKomutlariQuery request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.CihazKomutlari.AnyAsync(x => x.CalisacakKod == request.metin);
    }

}
