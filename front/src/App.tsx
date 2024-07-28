
import { useEffect, useState, useCallback } from "react";
import { Outlet } from "react-router-dom"

import { useAuth } from './contexts/AuthContext';

import useIdleTimer from './hooks/useIdleTimer';
import NavBar from './components/Navbar/'
import Credits from "./assets/Credits";

import { Container, CssBaseline, Paper, TableContainer, createTheme, styled } from '@mui/material';
import { ThemeProvider } from "@emotion/react";

const StyledContainer = styled(Container)({
    minWidth: '100%',
    display: 'flex',
});

function App() {
    const { logout } = useAuth();

    const [openApiThemeMode, setOpenApiThemeMode] = useState<"light" | "dark">(() => {
        const storedTheme = localStorage.getItem("openApiThemeMode");

        return (storedTheme as "light" | "dark") || "light";
    });

    useEffect(() => {
        localStorage.setItem("openApiThemeMode", openApiThemeMode);
    }, [openApiThemeMode]);

    const theme = createTheme({
        palette: {
            mode: openApiThemeMode,
            background: {
                default: openApiThemeMode === 'light' ? '#E8E8E8' : '#181818',
            }
        },
    });

    const toggleTheme = () => {
        setOpenApiThemeMode((prevMode) => (prevMode === "light" ? "dark" : "light"));
    };

    const logoutDueToInactivity = useCallback(() => {
        logout()
    }, []);

    useIdleTimer(logoutDueToInactivity, 7200000); // 7200000 = 2h

    return (
        <ThemeProvider theme={theme}>
            <NavBar handleTheme={toggleTheme} openApiThemeMode={openApiThemeMode}/>
            <Container maxWidth="xl" style={{ marginBottom: '20px' }}>
                <StyledContainer style={{ marginBottom: '20px' }}>
                    <CssBaseline />
                    <TableContainer
                        component={Paper}
                        sx={{
                            width: '100%',
                            padding: 3,
                            borderRadius: 3,
                            backgroundColor: openApiThemeMode === 'light' ? '#F9F9F9' : theme.palette.background.paper,
                        }}
                    >
                        <Outlet />
                    </TableContainer>
                </StyledContainer>
                <Credits />
            </Container>
        </ThemeProvider>
    )
}

export default App