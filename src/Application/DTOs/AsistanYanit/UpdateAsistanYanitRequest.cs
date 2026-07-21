using GeminiAsistanBackend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.DTOs.AsistanYanit;

public class UpdateAsistanYanitRequest
{
	public AsistanYanitTuru yanitTuru { get; set; }
}