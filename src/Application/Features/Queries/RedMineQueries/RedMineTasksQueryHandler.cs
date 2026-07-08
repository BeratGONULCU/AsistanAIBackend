using GeminiAsistanBackend.Application.DTOs.RedMineDto;
using GeminiAsistanBackend.Application.Interfaces.RedMineTask;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Queries.RedMineQueries;

public class RedMineTasksQueryHandler : IRequestHandler<RedMineTasksQuery, RedMineDataResponse>
{
    //private readonly IRedmineTokenStore _tokenStore;
    private readonly IRedmineService _redmineService;

    public RedMineTasksQueryHandler(IRedmineService redmineService)
    {
        _redmineService = redmineService;
        // this.token = token; // burada değer gelirse override mi ediliyor?
    }

    public async Task<RedMineDataResponse> Handle(RedMineTasksQuery request, CancellationToken cancellationToken)
    {
        var token = "22229cbe50ee8907648470105191e07cb222a803"; // burada _tokenStore ile veriler alınacak.

        if (string.IsNullOrWhiteSpace(token))
            throw new InvalidOperationException("token değeri alınamadı.");

        return await _redmineService.GetMyTasksAsync(token, cancellationToken).ConfigureAwait(false);
    }


}
