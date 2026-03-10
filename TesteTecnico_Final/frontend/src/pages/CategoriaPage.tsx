import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";

import type {Categoria} from "../types/Categoria.ts";
import {categoriaService} from "../services/categoriaService.ts";

export default function CategoriaPage() {
    const navigate = useNavigate();
    const [rows, setRows] = useState<Categoria[]>([]);

    useEffect(() => {
        carregarCategorias();
    }, []);

    async function carregarCategorias() {
        const data = await categoriaService.listar();
        setRows(data);
    }

    const columns: GridColDef[] = [
        { field: "id", headerName: "ID", width: 90, flex: 1 },
        { field: "descricao", headerName: "Descricao", width: 200, flex: 3 },
        { field: "finalidade", headerName: "Finalidade", type: "string", width: 120, flex: 1 },
    ];

    return (
        <Box sx={{ width: "100%", mt: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Categorias</Typography>

                <Stack direction="row" spacing={1}>
                    <Button
                        variant="contained"
                        startIcon={<AddIcon />}
                        onClick={() => navigate("/categorias/novo")}
                    >
                        Novo
                    </Button>

                    <Button
                        variant="contained"
                        startIcon={<RefreshIcon />}
                        onClick={carregarCategorias}
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