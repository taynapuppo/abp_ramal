using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace SistemaRamais.Ramais
{
    public class RamalSearchService : IRamalSearchService
    {
        private readonly IRamalRepository _ramalRepository;
        private readonly ILookupNormalizer _lookupNormalizer;

        // Construtor para inicialização de dependências
        public RamalSearchService(IRamalRepository ramalRepository, ILookupNormalizer lookupNormalizer)
        {
            _ramalRepository = ramalRepository;
            _lookupNormalizer = lookupNormalizer;
        }

        public virtual async Task<List<(string Ramal, string Nome, int Score)>> BuscarNomeAsync(string query)
        {
            // Normaliza a consulta para maiúsculas, já que você está usando NormalizedName
            var normalizedQuery = _lookupNormalizer.NormalizeName(query).ToUpper();
            var cleanQuery = RemoveDiacritics(normalizedQuery);

            // Obtém todos os ramais do repositório
            var ramais = await _ramalRepository.GetListAsync();

            // Filtra os ramais para aqueles cujo NormalizedName contenha a consulta
            var resultados = ramais
                .Where(r => RemoveDiacritics(r.NormalizedName).Contains(cleanQuery)) // Filtro baseado no nome normalizado
                .Select(r => new
                {
                    r.Numero,
                    r.Nome,
                    Score = FuzzyMatchScore(r.NormalizedName, cleanQuery)  // Calcula o score fuzzy
                })
                .ToList();

            // Retorna os resultados com o nome e o score
            return resultados.Select(r => (r.Numero, r.Nome, r.Score)).ToList();
        }

        // Função para calcular o score de similaridade entre o nome normalizado e a query
        private int FuzzyMatchScore(string normalizedName, string query)
        {
            return FuzzySharp.Fuzz.Ratio(normalizedName, query);  
        }

        private static string RemoveDiacritics(string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
            var stringBuilder = new System.Text.StringBuilder();

            foreach (var chr in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(chr);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(chr);
            }

            return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

    }
}
