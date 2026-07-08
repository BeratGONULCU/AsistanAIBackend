using GeminiAsistanBackend.Application.DTOs.EgitimDataset;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands.EgitimDataSet;

/* bunun service içerisinde şöyle olacak;
 
- for döngüsü ile ilgili ses_tetikleyicileri ile egitim_dataset.ses_tetikleyicileri_id değerini içerisine yazacak. 
yani bir nevi sync yapacak.
 
- yani ses_tetikleyicileri.id değeri ile egitim_dataset.ses_tetikleyicileri_id değerleri sync olması gerek.
 */
public sealed record CreateEgitimDatasetCommand
(
    string TetikleyiciMetin,
    int? typenum,
    int SesTetikleyiciId
) : IRequest<EgitimDatasetResponse>;