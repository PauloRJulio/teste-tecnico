import { useEffect, useState } from "react";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Typography from "@mui/material/Typography";
import Stack from "@mui/material/Stack";

import { DataGrid, type GridColDef } from "@mui/x-data-grid";
import RefreshIcon from "@mui/icons-material/Refresh";

import type { TotalPorPessoa, RelatorioPessoasResponse } from "../types/Relatorio";
import { relatorioService } from "../services/relatorioService";

export default function RelatorioPessoasPage() {
    const [rows, setRows] = useState<TotalPorPessoa[]>([]);
    const [totals, setTotals] = useState<RelatorioPessoasResponse | null>(null);

    useEffect(() => {
        carregarRelatorio();
    }, []);

    async function carregarRelatorio() {
        const data = await relatorioService.obterTotaisPorPessoa();
        setRows(data.totaisPorPessoas || []);
        setTotals(data);
    }

    const formatCurrency = (value: number) => {
        return Number(value).toLocaleString("pt-BR", {
            style: "currency",
            currency: "BRL",
        });
    };

    const columns: GridColDef<TotalPorPessoa>[] = [
        { field: "id", headerName: "ID", width: 90 },
        { field: "nome", headerName: "Nome", flex: 2, minWidth: 200 },
        {
            field: "totalReceitas",
            headerName: "Receitas",
            flex: 1,
            minWidth: 120,
            renderCell: (params) => (
                <Typography color="success.main" sx={{ display: 'flex', alignItems: 'center', height: '100%' }}>
                    {formatCurrency(params.row.totalReceitas)}
                </Typography>
            ),
        },
        {
            field: "totalDespesas",
            headerName: "Despesas",
            flex: 1,
            minWidth: 120,
            renderCell: (params) => (
                <Typography color="error.main" sx={{ display: 'flex', alignItems: 'center', height: '100%' }}>
                    {formatCurrency(params.row.totalDespesas)}
                </Typography>
            ),
        },
        {
            field: "saldo",
            headerName: "Saldo",
            flex: 1,
            minWidth: 120,
            renderCell: (params) => {
                const isPositive = params.row.saldo >= 0;
                return (
                    <Typography
                        color={isPositive ? "success.main" : "error.main"}
                        fontWeight="bold"
                        sx={{ display: 'flex', alignItems: 'center', height: '100%' }}
                    >
                        {formatCurrency(params.row.saldo)}
                    </Typography>
                );
            },
        },
    ];

    return (
        <Box sx={{ width: "100%", mt: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h6">Transações Por Pessoa</Typography>

                <Button
                    variant="contained"
                    startIcon={<RefreshIcon />}
                    onClick={carregarRelatorio}
                >
                    Atualizar
                </Button>
            </Stack>

            <Box sx={{ height: "60vh", mb: 2 }}>
                <DataGrid
                    rows={rows}
                    columns={columns}
                    pageSizeOptions={[10, 25, 50]}
                    initialState={{
                        pagination: { paginationModel: { pageSize: 10 } },
                    }}
                    disableRowSelectionOnClick
                />
            </Box>

            {totals && (
                <Box
                    sx={{
                        p: 2,
                        bgcolor: 'background.paper',
                        borderRadius: 1,
                        boxShadow: 1,
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center'
                    }}
                >
                    <Typography variant="h6">TOTAL GERAL</Typography>
                    <Stack direction="row" spacing={4}>
                        <Box textAlign="right">
                            <Typography variant="body2" color="text.secondary">Total Receitas</Typography>
                            <Typography variant="h6" color="success.main">{formatCurrency(totals.totalReceitas)}</Typography>
                        </Box>
                        <Box textAlign="right">
                            <Typography variant="body2" color="text.secondary">Total Despesas</Typography>
                            <Typography variant="h6" color="error.main">{formatCurrency(totals.totalDespesas)}</Typography>
                        </Box>
                        <Box textAlign="right">
                            <Typography variant="body2" color="text.secondary">Saldo Líquido</Typography>
                            <Typography variant="h6" color={totals.saldoConsolidado >= 0 ? "success.main" : "error.main"}>
                                {formatCurrency(totals.saldoConsolidado)}
                            </Typography>
                        </Box>
                    </Stack>
                </Box>
            )}
        </Box>
    );
}
