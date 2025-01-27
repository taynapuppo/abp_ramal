using Volo.Abp.Application.Dtos;
using System;

namespace SistemaRamais.Ramais
{
    public abstract class RamalExcelDownloadDtoBase
    {
        public string DownloadToken { get; set; } = null!;

        public string? FilterText { get; set; }

        public string? Numero { get; set; }
        public string? Departamento { get; set; }
        public string? Responsavel { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        public RamalExcelDownloadDtoBase()
        {

        }
    }
}