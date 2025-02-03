document.addEventListener('DOMContentLoaded', function () {
    // Chamar a função para carregar as estatísticas
    carregarEstatisticas();
});

async function carregarEstatisticas() {
    try {
        // Fazer uma requisição GET para o método GetEstatisticas da API
        const response = await fetch('/api/ramais/estatisticas');
        
        if (!response.ok) {
            throw new Error('Erro ao carregar estatísticas');
        }
        
        // Obter os dados da resposta (JSON)
        const data = await response.json();

        // Atualizar os elementos HTML com os dados recebidos
        document.getElementById('totalRamais').textContent = data.totalRamais;
        document.getElementById('totalDepartamentos').textContent = data.totalDepartamentos;
        document.getElementById('totalUsuariosAtivos').textContent = data.totalUsuariosAtivos;
    } catch (error) {
        console.error('Erro ao carregar as estatísticas:', error);
    }
}
