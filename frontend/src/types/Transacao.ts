export interface Transacao {
    id: number
    descricao: string
    valor: number
    tipoTransacao: number
    categoriaId: number
    categoriaDescricao: string
    pessoaId: number
    pessoaNome: string
}