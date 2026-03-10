import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import CircularProgress from "@mui/material/CircularProgress";
import Paper from "@mui/material/Paper";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import Alert from "@mui/material/Alert";
import Stack from "@mui/material/Stack";

import { pessoaService } from "../services/pessoaService";
import {Snackbar} from "@mui/material";

interface FormData {
    nome: string;
    idade: string;
}

interface FormErrors {
    nome?: string;
    idade?: string;
}

export default function PessoaForm() {
    const navigate = useNavigate();
    const { id } = useParams<{ id?: string }>();
    const isEdicao = !!id;

    const [form, setForm] = useState<FormData>({ nome: "", idade: "" });
    const [errors, setErrors] = useState<FormErrors>({});
    const [loading, setLoading] = useState(false);
    const [loadingDados, setLoadingDados] = useState(false);
    const [erro, setErro] = useState<string | null>(null);
    const [snackbar, setSnackbar] = useState<{
        open: boolean;
        message: string;
        severity: "success" | "error";
    }>({
        open: false,
        message: "",
        severity: "success",
    });

    useEffect(() => {
        if (isEdicao) {
            carregarPessoa();
        }
    }, [id]);

    async function carregarPessoa() {
        setLoadingDados(true);
        try {
            const data = await pessoaService.buscarPorId(Number(id));
            setForm({ nome: data.nome, idade: String(data.idade) });
        } catch {
            setErro("Erro ao carregar os dados da pessoa.");
        } finally {
            setLoadingDados(false);
        }
    }

    function validar(): boolean {
        const novosErros: FormErrors = {};

        if (!form.nome.trim()) {
            novosErros.nome = "Nome é obrigatório";
        } else if (form.nome.length > 200) {
            novosErros.nome = "Nome não pode ter mais de 200 caracteres";
        }

        const idadeNum = Number(form.idade);
        if (form.idade === "" || isNaN(idadeNum)) {
            novosErros.idade = "Idade é obrigatória";
        } else if (idadeNum < 0 || idadeNum > 150) {
            novosErros.idade = "Idade deve estar entre 0 e 150";
        }

        setErrors(novosErros);
        return Object.keys(novosErros).length === 0;
    }

    async function handleSubmit() {
        if (!validar()) return;

        setLoading(true);
        setErro(null);

        const payload = {
            nome: form.nome.trim(),
            idade: Number(form.idade),
        };

        try {
            let response;

            if (isEdicao) {
                response = await pessoaService.atualizar(Number(id), payload);
            } else {
                response = await pessoaService.criar(payload);
            }

            if (!response.success) {
                setSnackbar({
                    open: true,
                    message: response.message ?? "Erro ao salvar",
                    severity: "error",
                });
                return;
            }

            setSnackbar({
                open: true,
                message: response.message ?? "Salvo com sucesso",
                severity: "success",
            });

            setTimeout(() => navigate("/pessoas"), 800);

        } catch {
            setSnackbar({
                open: true,
                message: "Erro ao salvar. Tente novamente.",
                severity: "error",
            });
        } finally {
            setLoading(false);
        }
    }

    if (loadingDados) {
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
                    {isEdicao ? "Editar Pessoa" : "Nova Pessoa"}
                </Typography>

                {erro && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                        {erro}
                    </Alert>
                )}

                <Stack spacing={2}>
                    <TextField
                        label="Nome"
                        value={form.nome}
                        onChange={(e) =>
                            setForm((f) => ({ ...f, nome: e.target.value }))
                        }
                        error={!!errors.nome}
                        helperText={errors.nome}
                        inputProps={{ maxLength: 200 }}
                        fullWidth
                        size="small"
                    />

                    <TextField
                        label="Idade"
                        type="number"
                        value={form.idade}
                        onChange={(e) =>
                            setForm((f) => ({ ...f, idade: e.target.value }))
                        }
                        error={!!errors.idade}
                        helperText={errors.idade}
                        inputProps={{ min: 0, max: 150 }}
                        fullWidth
                        size="small"
                    />

                    <Box display="flex" justifyContent="flex-end" gap={1} pt={1}>
                        <Button
                            variant="outlined"
                            onClick={() => navigate("/pessoas")}
                            disabled={loading}
                        >
                            Cancelar
                        </Button>
                        <Button
                            variant="contained"
                            onClick={handleSubmit}
                            disabled={loading}
                            startIcon={
                                loading ? <CircularProgress size={16} color="inherit" /> : null
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
                    onClose={() => setSnackbar({ ...snackbar, open: false })}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Box>
    );
}