import { BrowserRouter, Routes, Route } from "react-router-dom";
import { DashboardLayoutBasic as Layout } from "./components/layout/Layout";


import Pessoas from "./pages/PessoasPage.tsx";
import PessoaForm from "./components/PessoaForm.tsx";
import Categorias from "./pages/CategoriaPage.tsx";
import CategoriaForm from "./components/CategoriaForm.tsx";
import TransacaoPage from "./pages/TransacaoPage.tsx";
import TransacaoForm from "./components/TransacaoForm.tsx";

import RelatorioPessoasPage from "./pages/RelatorioPessoasPage.tsx";
import RelatorioCategoriasPage from "./pages/RelatorioCategoriasPage.tsx";

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Layout />}>
                    <Route path="pessoas" element={<Pessoas />} />
                    <Route path="pessoas/novo" element={<PessoaForm />} />
                    <Route path="pessoas/:id/editar" element={<PessoaForm />} />
                    <Route path="categorias" element={<Categorias />} />
                    <Route path="categorias/novo" element={<CategoriaForm />} />
                    <Route path="transacoes" element={<TransacaoPage />} />
                    <Route path="transacoes/nova" element={<TransacaoForm />} />

                    <Route path="relatorios/transacoesporpessoa" element={<RelatorioPessoasPage />} />
                    <Route path="relatorios/transacoesporcategoria" element={<RelatorioCategoriasPage />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}