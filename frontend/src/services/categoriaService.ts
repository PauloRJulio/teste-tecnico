import type { Categoria } from "../types/Categoria.ts";
import type {BaseResponse} from "../types/BaseResonse.ts";

const BASE_URL = "https://localhost:7062/api/categoria";

export const categoriaService = {
    async listar(descricao?: string, finalidade?: number): Promise<Categoria[]> {
        const params = new URLSearchParams();
        if (descricao) params.append("descricao", descricao);
        if (finalidade !== undefined) params.append("finalidade", String(finalidade));

        const url = `${BASE_URL}/GetAll${params.toString() ? `?${params}` : ""}`;
        const res = await fetch(url);
        return res.json();
    },

    async buscarPorId(id: number): Promise<Categoria> {
        const res = await fetch(`${BASE_URL}/Get/${id}`);
        const data = await res.json();

        return data.item;
    },

    async criar(input: { descricao: string; finalidade: number }): Promise<BaseResponse> {
        const res = await fetch(`${BASE_URL}/Create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(input),
        });

        return res.json();
    },
};