import type { Pessoa } from "../types/Pessoa";
import type {BaseResponse} from "../types/BaseResonse.ts";

const BASE_URL = "https://localhost:7062/api/pessoa";

export const pessoaService = {
    async listar(nome?: string, idade?: number): Promise<Pessoa[]> {
        const params = new URLSearchParams();
        if (nome) params.append("nome", nome);
        if (idade !== undefined) params.append("idade", String(idade));

        const url = `${BASE_URL}/GetAll${params.toString() ? `?${params}` : ""}`;
        const res = await fetch(url);
        return res.json();
    },

    async buscarPorId(id: number): Promise<Pessoa> {
        const res = await fetch(`${BASE_URL}/Get/${id}`);
        const data = await res.json();

        return data.item;
    },

    async criar(input: { nome: string; idade: number }): Promise<BaseResponse> {
        const res = await fetch(`${BASE_URL}/Create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(input),
        });

        return res.json();
    },

    async atualizar(id: number, input: { nome: string; idade: number }): Promise<BaseResponse> {
        const res = await fetch(`${BASE_URL}/Update/${id}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(input),
        });

        return res.json();
    },

    async excluir(id: number): Promise<void> {
        await fetch(`${BASE_URL}/Delete/${id}`, { method: "DELETE" });
    },
};