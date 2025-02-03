// graficos.js

$(document).ready(function () {
    var ctx = document.getElementById('graficoRamais').getContext('2d');

    // Exemplo de dados para o gráfico
    var dadosGrafico = {
        labels: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio'], // Meses (ou categorias)
        datasets: [{
            label: 'Total de Ramais por Mês', // Título do gráfico
            data: [10, 20, 15, 30, 25], // Dados para o gráfico (número de ramais por mês)
            backgroundColor: 'rgba(75, 192, 192, 0.2)', // Cor de fundo da barra
            borderColor: 'rgb(1, 10, 10)', // Cor da borda da barra
            borderWidth: 1
        }]
    };

    var config = {
        type: 'bar',
        data: dadosGrafico,
        options: {
            scales: {
                y: {
                    beginAtZero: true 
                }
            }
        }
    };

    // Criar o gráfico
    var graficoRamais = new Chart(ctx, config);
});
