import type { Transacao } from "../types/Transacao.ts";
import type {BaseResponse} from "../types/BaseResponse.ts";

const BASE_URL = "https://localhost:7062/api/transacao";
export const transacaoService = {
    async listar(descricao?: string, valor?: number, tipoTransacao?: number, pessoaId?: number, categoriaId?:number): Promise<Transacao[]> {
        const params = new URLSearchParams();
        if (descricao) params.append("descricao", descricao);
        if (valor !== undefined) params.append("valor", String(valor));
        if (tipoTransacao !== undefined) params.append("tipoTransacao", String(tipoTransacao));
        if (pessoaId !== undefined) params.append("pessoaId", String(pessoaId));
        if (categoriaId !== undefined) params.append("categoriaId", String(categoriaId));
        
        const url = `${BASE_URL}/GetAll${params.toString() ? `?${params}` : ""}`;
        const res = await fetch(url);
        return res.json();
    },

    async criar(input: { descricao?: string, valor?: number, tipoTransacao?: number, pessoaId?: number, categoriaId?:number}): Promise<BaseResponse> {
        const res = await fetch(`${BASE_URL}/Create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(input),
        });

        return res.json();
    },
};