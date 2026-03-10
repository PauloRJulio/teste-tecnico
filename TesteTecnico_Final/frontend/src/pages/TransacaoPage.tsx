import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";

import type { Transacao } from "../types/Transacao";
import { transacaoService } from "../services/transacaoService";

export default function TransacaoPage() {

    const navigate = useNavigate();
    const [rows, setRows] = useState<Transacao[]>([]);

    useEffect(() => {
        carregarTransacoes();
    }, []);

    async function carregarTransacoes() {
        const data = await transacaoService.listar();
        setRows(data);
    }

    const columns: GridColDef<Transacao>[] = [
        { field: "id", headerName: "ID", width: 90, flex: 1 },

        { field: "descricao", headerName: "Descrição", width: 200, flex: 2 },

        {
            field: "valor",
            headerName: "Valor",
            width: 120,
            flex: 1,
            renderCell: (params) =>
                Number(params.row.valor).toLocaleString("pt-BR", {
                    style: "currency",
                    currency: "BRL",
                }),
        },
        
        {
            field: "tipoTransacao",
            headerName: "Tipo",
            width: 120,
            flex: 1
        },

        {
            field: "categoriaDescricao",
            headerName: "Categoria",
            width: 200,
            flex: 2,
        },

        {
            field: "pessoaNome",
            headerName: "Pessoa",
            width: 200,
            flex: 2,
        },
    ];

    return (
        <Box sx={{ width: "100%", mt: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Transações</Typography>

                <Stack direction="row" spacing={1}>
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={() => navigate("/transacoes/nova")}
                    >
                        Novo
                    </Button>

                    <Button
                        variant="contained"
                        startIcon={<RefreshIcon />}
                        onClick={carregarTransacoes}
                    >
                        Atualizar
                    </Button>
                </Stack>
            </Stack>

            <Box sx={{ height: "75vh" }}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    pageSizeOptions={[5]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 5 } },
                    }}
                />
            </Box>
        </Box>
    );
}