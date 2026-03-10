import { api } from "./api";
import type { RelatorioPessoasResponse, RelatorioCategoriasResponse } from "../types/Relatorio";

export const relatorioService = {
    async obterTotaisPorPessoa(): Promise<RelatorioPessoasResponse> {
        const response = await api.get<RelatorioPessoasResponse>("/Pessoa/GetRelatorioTotaisPorPessoa");
        return response.data;
    },

    async obterTotaisPorCategoria(): Promise<RelatorioCategoriasResponse> {
        const response = await api.get<RelatorioCategoriasResponse>("/Categoria/GetRelatorioTotaisPorCategoria");
        return response.data;
    }
};
