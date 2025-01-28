namespace SistemaRamais.Ramais
{
    public static class RamalConsts
    {
        private const string DefaultSorting = "{0}Numero asc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "Ramal." : string.Empty);
        }

        public const int NomeMinLength = 0;
        public const int NomeMaxLength = 100;
        public const int NumeroMinLength = 0;
        public const int NumeroMaxLength = 10;
        public const int DepartamentoMinLength = 0;
        public const int DepartamentoMaxLength = 100;
        public const int EmailMinLength = 0;
        public const int EmailMaxLength = 100;
    }
}