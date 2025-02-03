using System;

namespace SistemaRamais.Application.Contracts.Ramais
{
    public class RamalRelatorioDto
    {
        public string? Nome { get; set; }
        public string? Numero { get; set; }
        public string? Departamento { get; set; }
        public DateTime? DataCriacao { get; set; }
        public string? CriadoPor { get; set; }
    }
}