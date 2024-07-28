import { useEffect } from 'react';
import { useForm, SubmitHandler } from 'react-hook-form';
import { useAuth } from '../../../contexts/AuthContext';
import { useNavigate } from 'react-router-dom';
import { SnackbarProvider } from 'notistack';

import { HOME_PAGE } from './../../../config';
import LogoOpenERP from '../../../assets/LogoOpenErp';

import { ThemeProvider } from '@emotion/react';
import {
    TextField,
    Button,
    styled,
    TableContainer,
    CssBaseline,
    Paper,
    Container,
    createTheme,
    CircularProgress
} from '@mui/material';

const StyledForm = styled('form')({
    padding: 20,
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
});

interface FormInputProps {
  username: string;
  password: string;
}

const Login = () => {
    const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormInputProps>();
    const { login, signed } = useAuth();
    const navigate = useNavigate();
    const openApiThemeMode = (localStorage.getItem("openApiThemeMode") as "light" | "dark") || "light";

    const theme = createTheme({
        palette: {
            mode: openApiThemeMode,
            background: {
                default: openApiThemeMode === 'light' ? '#E8E8E8' : '#181818',
            }
        },
    });

    useEffect(() => {
        if (signed) {
            navigate(HOME_PAGE)
        }
    }, []);

    const onSubmit: SubmitHandler<FormInputProps> = async (data) => {
        login(data.username, data.password);
    };

    return (
        <ThemeProvider theme={theme}>
            <Container
                sx={{
                    minHeight: '100vh',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <CssBaseline />
                <TableContainer
                    component={Paper}
                    sx={{
                        padding: 2,
                        borderRadius: 3,
                        maxWidth: 400,
                    }}
                >
                    <StyledForm
                        onSubmit={handleSubmit(onSubmit)}
                        noValidate
                        autoComplete="off"
                    >
                        <LogoOpenERP />
                        <TextField
                            label="Username"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            {...register('username', { required: 'Username is required' })}
                            error={!!errors.username}
                            helperText={errors.username?.message}
                        />
                        <TextField
                            label="Password"
                            variant="outlined"
                            margin="normal"
                            fullWidth
                            type="password"
                            {...register('password', {
                                required: 'Password is required',
                                minLength: {
                                    value: 5,
                                    message: 'Password must be at least 5 characters long'
                                },
                                maxLength: {
                                    value: 255,
                                    message: 'Password cannot exceed 255 characters'
                                }
                            })}
                            error={!!errors.password}
                            helperText={errors.password?.message}
                        />
                        { isSubmitting
                            ? <CircularProgress sx={{ marginTop: '15px' }} />
                            : <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                sx={{ marginTop: '15px' }}
                            >
                                Sign In
                            </Button>
                        }
                    </StyledForm>
                </TableContainer>
            </Container>
            <SnackbarProvider/>
        </ThemeProvider>
    );
};

export default Login;