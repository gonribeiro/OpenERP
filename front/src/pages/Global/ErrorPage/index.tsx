import { Link, useNavigate } from "react-router-dom";
import { Container, Typography, Button, CssBaseline, createTheme } from "@mui/material";
import { useEffect } from "react";
import { useAuth } from "../../../contexts/AuthContext";
import { ThemeProvider } from "@emotion/react";

const ErrorPage = () => {
  const { signed } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (!signed) {
        navigate("/")
    }
  }, []);

  const theme = createTheme({
    palette: {
      mode: (localStorage.getItem("openApiThemeMode") as "light" | "dark") || "light"
    },
  });

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Container
        maxWidth="sm"
        style={{
          marginTop: "50px",
          height: "100vh",
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <Typography variant="h3" gutterBottom>
          404 - Page Not Found
        </Typography>
        <Button component={Link} to="/" variant="contained" color="primary">
          Back to Home Page
        </Button>
      </Container>
    </ThemeProvider>
  );
};

export default ErrorPage;