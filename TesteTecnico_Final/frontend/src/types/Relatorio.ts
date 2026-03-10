export interface TotalPorPessoa {
    id: number;
    nome: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface TotalPorCategoria {
    id: number;
    descricao: string;
    finalidade: string;
    totalReceitas: number;
    totalDespesas: number;
    saldo: number;
}

export interface RelatorioPessoasResponse {
    totalReceitas: number;
    totalDespesas: number;
    saldoConsolidado: number;
    totaisPorPessoas: TotalPorPessoa[];
}

export interface RelatorioCategoriasResponse {
    totalReceitas: number;
    totalDespesas: number;
    saldoConsolidado: number;
    totaisPorCategorias: TotalPorCategoria[];
}
