using System;

namespace SistemaRamais.Ramais
{
    public abstract class RamalExcelDtoBase
    {
        public string Numero { get; set; } = null!;
        public string Departamento { get; set; } = null!;
        public string Responsavel { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Telefone { get; set; } = null!;
    }
}