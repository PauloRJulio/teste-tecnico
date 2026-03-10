import * as React from "react";
import Box from "@mui/material/Box";
import { createTheme } from "@mui/material/styles";
import { Outlet, useLocation, useNavigate } from "react-router-dom";

import PeopleIcon from "@mui/icons-material/People";
import ReceiptLongIcon from "@mui/icons-material/ReceiptLong";
import CategoryIcon from "@mui/icons-material/Category";
import BarChartIcon from "@mui/icons-material/BarChart";
import DescriptionIcon from "@mui/icons-material/Description";
import CodeIcon from "@mui/icons-material/Code";

import { AppProvider, type Navigation } from "@toolpad/core/AppProvider";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import {ptBR} from "@mui/x-data-grid/locales";

const NAVIGATION: Navigation = [
    {
        kind: "header",
        title: "Menu",
    },
    {
        segment: "pessoas",
        title: "Pessoas",
        icon: <PeopleIcon />,
    },
    {
        segment: "categorias",
        title: "Categorias",
        icon: <CategoryIcon />,
    },
    {
        segment: "transacoes",
        title: "Transações",
        icon: <ReceiptLongIcon />,
    },
    {
        kind: "divider",
    },
    {
        kind: "header",
        title: "Analítico",
    },
    {
        segment: "relatorios",
        title: "Relatórios",
        icon: <BarChartIcon />,
        children: [
            {
                segment: "transacoesporpessoa",
                title: "Transações Por Pessoa",
                icon: <DescriptionIcon />,
            },
            {
                segment: "transacoesporcategoria",
                title: "Transações Por Categoria",
                icon: <DescriptionIcon />,
            },
        ],
    },
];

const theme = createTheme({
    cssVariables: {
        colorSchemeSelector: "data-toolpad-color-scheme",
    },
    colorSchemes: { light: true, dark: true }
}, ptBR);

export function DashboardLayoutBasic() {
    const location = useLocation();
    const navigate = useNavigate();

    const router = React.useMemo(
        () => ({
            pathname: location.pathname,
            searchParams: new URLSearchParams(location.search),
            navigate: (url: string | URL) => navigate(url.toString()),
        }),
        [location, navigate]
    );

    return (
        <AppProvider
            navigation={NAVIGATION}
            router={router}
            theme={theme}
            branding={{
                title: "Dev: Paulo Julio",
                logo: <CodeIcon/>,
            }}
        >
            <DashboardLayout>
                <Box sx={{p: 3}}>
                    <Outlet/>
                </Box>
            </DashboardLayout>
        </AppProvider>
    );
}