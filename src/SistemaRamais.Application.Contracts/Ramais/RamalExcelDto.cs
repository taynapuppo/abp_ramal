using System;

namespace SistemaRamais.Ramais
{
    public abstract class RamalExcelDtoBase
    {
        public string Nome { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Departamento { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}