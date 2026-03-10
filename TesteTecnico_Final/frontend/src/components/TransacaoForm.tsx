import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Paper from "@mui/material/Paper";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import Alert from "@mui/material/Alert";
import Stack from "@mui/material/Stack";
import Snackbar from "@mui/material/Snackbar";

import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";
import InputLabel from "@mui/material/InputLabel";
import FormControl from "@mui/material/FormControl";

import { transacaoService } from "../services/transacaoService";
import { pessoaService } from "../services/pessoaService";
import { categoriaService } from "../services/categoriaService";

interface FormData {
    descricao: string;
    valor: string;
    tipoTransacao: number | "";
    pessoaId: number | "";
    categoriaId: number | "";
}

interface FormErrors {
    descricao?: string;
    valor?: string;
    tipoTransacao?: string;
    pessoaId?: string;
    categoriaId?: string;
}

interface Pessoa {
    id: number;
    nome: string;
}

interface Categoria {
    id: number;
    descricao: string;
}

export default function TransacaoForm() {

    const navigate = useNavigate();

    const [form, setForm] = useState<FormData>({
        descricao: "",
        valor: "",
        tipoTransacao: "",
        pessoaId: "",
        categoriaId: ""
    });

    const [errors, setErrors] = useState<FormErrors>({});
    const [loading, setLoading] = useState(false);

    const [pessoas, setPessoas] = useState<Pessoa[]>([]);
    const [categorias, setCategorias] = useState<Categoria[]>([]);

    const [loadingData, setLoadingData] = useState(true);

    const [snackbar, setSnackbar] = useState({
        open: false,
        message: "",
        severity: "success" as "success" | "error"
    });

    useEffect(() => {
        carregarDados();
    }, []);

    async function carregarDados() {

        try {

            const [pessoasRes, categoriasRes] = await Promise.all([
                pessoaService.listar(),
                categoriaService.listar()
            ]);

            setPessoas(pessoasRes ?? []);
            setCategorias(categoriasRes ?? []);

        } catch {

            setSnackbar({
                open: true,
                message: "Erro ao carregar dados",
                severity: "error"
            });

        } finally {
            setLoadingData(false);
        }
    }

    function validar(): boolean {

        const novosErros: FormErrors = {};

        if (!form.descricao.trim())
            novosErros.descricao = "Descrição é obrigatória";

        if (!form.valor || Number(form.valor) <= 0)
            novosErros.valor = "Valor deve ser maior que zero";

        if (form.tipoTransacao === "")
            novosErros.tipoTransacao = "Tipo é obrigatório";

        if (form.pessoaId === "")
            novosErros.pessoaId = "Pessoa é obrigatória";

        if (form.categoriaId === "")
            novosErros.categoriaId = "Categoria é obrigatória";

        setErrors(novosErros);

        return Object.keys(novosErros).length === 0;
    }

    async function handleSubmit() {

        if (!validar()) return;

        setLoading(true);

        const payload = {
            descricao: form.descricao.trim(),
            valor: Number(form.valor),
            tipoTransacao: Number(form.tipoTransacao),
            pessoaId: Number(form.pessoaId),
            categoriaId: Number(form.categoriaId)
        };

        try {

            const response = await transacaoService.criar(payload);

            if (!response.success) {

                setSnackbar({
                    open: true,
                    message: response.message ?? "Erro ao salvar",
                    severity: "error"
                });

                return;
            }

            setSnackbar({
                open: true,
                message: response.message ?? "Transação criada",
                severity: "success"
            });

            setTimeout(() => navigate("/transacoes"), 800);

        } catch {

            setSnackbar({
                open: true,
                message: "Erro ao salvar",
                severity: "error"
            });

        } finally {
            setLoading(false);
        }
    }

    if (loadingData) {

        return (
            <Box display="flex" justifyContent="center" mt={6}>
                <CircularProgress />
            </Box>
        );
    }

    return (
        <Box maxWidth={480} mx="auto" mt={4} px={2}>

            <Paper variant="outlined" sx={{ p: 3 }}>

                <Typography variant="h6" mb={3}>
                    Nova Transação
                </Typography>

                <Stack spacing={2}>

                    <TextField
                        label="Descrição"
                        value={form.descricao}
                        onChange={(e) =>
                            setForm(f => ({ ...f, descricao: e.target.value }))
                        }
                        error={!!errors.descricao}
                        helperText={errors.descricao}
                        fullWidth
                        size="small"
                    />

                    <TextField
                        label="Valor"
                        type="number"
                        value={form.valor}
                        onChange={(e) =>
                            setForm(f => ({ ...f, valor: e.target.value }))
                        }
                        error={!!errors.valor}
                        helperText={errors.valor}
                        fullWidth
                        size="small"
                    />

                    <FormControl size="small" error={!!errors.tipoTransacao}>

                        <InputLabel>Tipo</InputLabel>

                        <Select<number>
                            value={form.tipoTransacao}
                            label="Tipo"
                            onChange={(e) =>
                                setForm(f => ({
                                    ...f,
                                    tipoTransacao: e.target.value as number
                                }))
                            }
                        >
                            <MenuItem value={0}>Despesa</MenuItem>
                            <MenuItem value={1}>Receita</MenuItem>
                        </Select>

                        {errors.tipoTransacao && (
                            <Typography color="error" fontSize={12}>
                                {errors.tipoTransacao}
                            </Typography>
                        )}

                    </FormControl>

                    <FormControl size="small" error={!!errors.pessoaId}>

                        <InputLabel>Pessoa</InputLabel>

                        <Select<number>
                            value={form.pessoaId}
                            label="Pessoa"
                            onChange={(e) =>
                                setForm(f => ({
                                    ...f,
                                    pessoaId: e.target.value as number
                                }))
                            }
                        >
                            {pessoas.map(p => (
                                <MenuItem key={p.id} value={p.id}>
                                    {p.nome}
                                </MenuItem>
                            ))}
                        </Select>

                    </FormControl>

                    <FormControl size="small" error={!!errors.categoriaId}>

                        <InputLabel>Categoria</InputLabel>

                        <Select<number>
                            value={form.categoriaId}
                            label="Categoria"
                            onChange={(e) =>
                                setForm(f => ({
                                    ...f,
                                    categoriaId: e.target.value as number
                                }))
                            }
                        >
                            {categorias.map(c => (
                                <MenuItem key={c.id} value={c.id}>
                                    {c.descricao}
                                </MenuItem>
                            ))}
                        </Select>

                    </FormControl>

                    <Box display="flex" justifyContent="flex-end" gap={1} pt={1}>

                        <Button
                            variant="outlined"
                            onClick={() => navigate("/transacoes")}
                            disabled={loading}
                        >
                            Cancelar
                        </Button>

                        <Button
                            variant="contained"
                            onClick={handleSubmit}
                            disabled={loading}
                            startIcon={
                                loading
                                    ? <CircularProgress size={16} color="inherit"/>
                                    : null
                            }
                        >
                            {loading ? "Salvando..." : "Salvar"}
                        </Button>

                    </Box>

                </Stack>

            </Paper>

            <Snackbar
                open={snackbar.open}
                autoHideDuration={3000}
                onClose={() => setSnackbar({ ...snackbar, open: false })}
                anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
            >
                <Alert
                    severity={snackbar.severity}
                    variant="filled"
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>

        </Box>
    );
}