using System.IO;
using System.Runtime.Intrinsics.X86;

namespace GeminiAsistanBackend.Domain.Enums;

public enum IslemDurum
{
    YEREL_CALISTI, 
    OLLAMA_YALITTI,
    OLLAMA_AI_LEARNED,
    AI_LEARNED,
    MANUEL,
    CALISACAK_KOD_BASARILI,
    HATA
}


/*

dotnet ef migrations add InitialCreate --project Infrastructure --startup-project Api --context AppDbContext
dotnet ef database update --project Infrastructure --startup-project Api --context AppDbContext


----------

işlem sırası 
- entity,dbcontext,migration
- repository,unit of work
- service 
- controller
 */
