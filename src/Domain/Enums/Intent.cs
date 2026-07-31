using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Domain.Enums;

public enum Intent
{
    COMMAND,
    CHAT,
    QUESTION,
    INFO,
    UNCERTAIN
}
