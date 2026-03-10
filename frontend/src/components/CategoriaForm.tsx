import { useState } from "react";
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

import { categoriaService } from "../services/categoriaService";

interface FormData {
    descricao: string;
    finalidade: string;
}

interface FormErrors {
    descricao?: string;
    finalidade?: string;
}

export default function CategoriaForm() {

    const navigate = useNavigate();

    const [form, setForm] = useState<FormData>({
        descricao: "",
        finalidade: ""
    });

    const [errors, setErrors] = useState<FormErrors>({});
    const [loading, setLoading] = useState(false);

    const [snackbar, setSnackbar] = useState({
        open: false,
        message: "",
        severity: "success" as "success" | "error"
    });

    function validar(): boolean {

        const novosErros: FormErrors = {};

        if (!form.descricao.trim())
            novosErros.descricao = "Descrição é obrigatória";

        if (form.finalidade === "")
            novosErros.finalidade = "Finalidade é obrigatória";

        setErrors(novosErros);

        return Object.keys(novosErros).length === 0;
    }

    async function handleSubmit() {

        if (!validar()) return;

        setLoading(true);

        const payload = {
            descricao: form.descricao.trim(),
            finalidade: Number(form.finalidade)
        };

        try {

            const response = await categoriaService.criar(payload);

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
                message: response.message ?? "Categoria criada",
                severity: "success"
            });

            setTimeout(() => navigate("/categorias"), 800);

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

    return (
        <Box maxWidth={480} mx="auto" mt={4} px={2}>

            <Paper variant="outlined" sx={{ p: 3 }}>

                <Typography variant="h6" mb={3}>
                    Nova Categoria
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

                    <FormControl size="small" error={!!errors.finalidade}>

                        <InputLabel>Finalidade</InputLabel>

                        <Select
                            value={form.finalidade}
                            label="Finalidade"
                            onChange={(e) =>
                                setForm(f => ({
                                    ...f,
                                    finalidade: e.target.value
                                }))
                            }
                        >
                            <MenuItem value={0}>Despesa</MenuItem>
                            <MenuItem value={1}>Receita</MenuItem>
                            <MenuItem value={2}>Ambas</MenuItem>

                        </Select>

                        {errors.finalidade && (
                            <Typography color="error" fontSize={12} mt={0.5}>
                                {errors.finalidade}
                            </Typography>
                        )}

                    </FormControl>

                    <Box display="flex" justifyContent="flex-end" gap={1} pt={1}>

                        <Button
                            variant="outlined"
                            onClick={() => navigate("/categorias")}
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
                    onClose={() => setSnackbar({ ...snackbar, open: false })}
                >
                    {snackbar.message}
                </Alert>
            </Snackbar>

        </Box>
    );
}