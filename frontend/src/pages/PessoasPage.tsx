import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import RefreshIcon from "@mui/icons-material/Refresh";

import { pessoaService } from "../services/pessoaService";
import type { Pessoa } from "../types/Pessoa";

export default function PessoasPage() {
    const navigate = useNavigate();
    const [rows, setRows] = useState<Pessoa[]>([]);

    useEffect(() => {
        carregarPessoas();
    }, []);

    async function carregarPessoas() {
        const data = await pessoaService.listar();
        setRows(data);
    }

    function handleEditar(pessoa: Pessoa) {
        navigate(`/pessoas/${pessoa.id}/editar`);
    }

    async function handleExcluir(id: number) {
        if (!confirm("Deseja excluir essa pessoa?")) return;
        await pessoaService.excluir(id);
        await carregarPessoas();
    }

    const columns: GridColDef[] = [
        { field: "id", headerName: "ID", width: 90, flex: 1 },
        { field: "nome", headerName: "Nome", width: 200, flex: 3 },
        { field: "idade", headerName: "Idade", type: "number", width: 120, flex: 1 },
        {
            field: "acoes",
            headerName: "Ações",
            width: 120,
            sortable: false,
            renderCell: (params) => (
                <>
                    <IconButton color="primary" onClick={() => handleEditar(params.row)}>
                        <EditIcon />
                    </IconButton>
                    <IconButton color="error" onClick={() => handleExcluir(params.row.id)}>
                        <DeleteIcon />
                    </IconButton>
                </>
            ),
        },
    ];

    return (
        <Box sx={{ width: "100%", mt: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Pessoas</Typography>

                <Stack direction="row" spacing={1}>
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={() => navigate("/pessoas/novo")}
                    >
                        Novo
                    </Button>

                    <Button
                        variant="contained"
                        startIcon={<RefreshIcon />}
                        onClick={carregarPessoas}
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