using FuzzySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace SistemaRamais.Ramais
{
    public class RamalSearchService
    {
        private readonly IRamalRepository _ramalRepository;

        // Construtor com dependência do repositório de ramais
        public RamalSearchService(IRamalRepository ramalRepository)
        {
            _ramalRepository = ramalRepository;
        }

        // Função para normalizar os nomes, removendo acentos
        private static string RemoverAcentos(string texto)
        {
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        // Função para calcular a distância de Levenshtein
        private static int LevenshteinDistance(string a, string b)
        {
            var n = a.Length;
            var m = b.Length;
            var d = new int[n + 1, m + 1];

            for (var i = 0; i <= n; d[i, 0] = i++) ;
            for (var j = 0; j <= m; d[0, j] = j++) ;

            for (var i = 1; i <= n; i++)
            {
                for (var j = 1; j <= m; j++)
                {
                    var cost = a[i - 1] == b[j - 1] ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

        // Método de busca usando Levenshtein e FuzzySharp
        public async Task<List<(string Ramal, string Responsavel, int Score)>> BuscarResponsavelAsync(string query)
        {
            // Remover acentos da query
            var querySemAcentos = RemoverAcentos(query);

            // Obtenha todos os ramais com uma busca insensível a maiúsculas/minúsculas e remova acentos com 'unaccent'
            var ramais = await _ramalRepository.GetListAsync(r => EF.Functions.ILike(
                EF.Functions.Unaccent(RemoverAcentos(r.Responsavel)), $"%{querySemAcentos}%"));

            return ramais
                .Select(d =>
                {
                    // Separa o nome completo em partes (considerando o último como sobrenome)
                    var partesNome = d.Responsavel.Split(' ');
                    var sobrenome = partesNome.Last();  // Considera o último nome como sobrenome

                    // Faz a comparação com o sobrenome
                    return (d.Numero, d.Responsavel, Score: Fuzz.PartialRatio(querySemAcentos, RemoverAcentos(sobrenome)));
                })
                .Where(d => d.Score > 60) // Ajuste conforme necessário para permitir maior ou menor flexibilidade
                .OrderByDescending(d => d.Score)
                .ToList();
        }
    }
}
