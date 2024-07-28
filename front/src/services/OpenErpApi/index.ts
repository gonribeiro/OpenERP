import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { enqueueSnackbar } from 'notistack';
import { OPEN_ERP_API_URL } from './../../config';

const openErpApi = axios.create({
    baseURL: OPEN_ERP_API_URL,
});

openErpApi.interceptors.request.use(
    async (config) => {
        const accessToken = localStorage.getItem('openApiAccessToken');

        if (config.url == 'login' || (accessToken && !isTokenExpired(accessToken))) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        } else if (config.url != 'refreshToken') {
            try {
                const refreshToken = localStorage.getItem('openApiRefreshToken');

                if (accessToken && refreshToken) {
                    const decodedToken: any = jwtDecode(accessToken);
                    const userId = decodedToken["id"];
                    const newAccessToken = await refreshAccessToken(refreshToken, userId);

                    localStorage.setItem('openApiAccessToken', newAccessToken);

                    config.headers.Authorization = `Bearer ${newAccessToken}`;
                } else {
                    destroySession();
                }
            } catch (error) {
                destroySession();
            }
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

openErpApi.interceptors.response.use(
    (response) => {
        if (response.data.message) {
            enqueueSnackbar(response.data.message, {
                variant: "success",
                anchorOrigin: {
                    vertical: 'bottom',
                    horizontal: 'right'
                }
            });
        }

        return response;
    },
    (error) => {
        if (!error.response) {
            enqueueSnackbar('Server Unavailable', {
                variant: "error",
                anchorOrigin: {
                    vertical: 'bottom',
                    horizontal: 'right'
                }
            })

            return Promise.reject(new Error('Server Unavailable'));
        }

        const err = error.response;

        if (err.config.url != 'login' && err.status === 401) {
            const accessToken = localStorage.getItem('openApiAccessToken');

            if (!accessToken || isTokenExpired(accessToken))
                destroySession();
        } else if (!err.data.errors) {
            enqueueSnackbar(err.data, {
                variant: "error",
                anchorOrigin: {
                    vertical: 'bottom',
                    horizontal: 'right'
                }
            })
        } else {
            enqueueSnackbar(err.data.title, {
                variant: "error",
                anchorOrigin: {
                    vertical: 'bottom',
                    horizontal: 'right'
                }
            })
        }

        return Promise.reject(error);
    }
);

function isTokenExpired(accessToken: string | null): boolean {
    try {
        if (!accessToken) return true;

        const decodedToken: any = jwtDecode(accessToken);

        return decodedToken.exp < Date.now() / 1000;
    } catch (error) {
        return true;
    }
}

async function refreshAccessToken(refreshToken: string, userId: string): Promise<string> {
    try {
        const data = {
            refreshToken: refreshToken,
            userId: userId
        };

        const response = await openErpApi.post('refreshToken', data, {
            headers: {
                'Content-Type': 'application/json'
            }
        }).then((response) => {
            return response.data.newAccessToken;
        });

        return response;
    } catch (error) {
        throw error;
    }
}

function destroySession(): void {
    localStorage.removeItem('openApiAccessToken');
    localStorage.removeItem('openApiRefreshToken');

    enqueueSnackbar('Session expired - Redirecting to login', {
        variant: "error",
        anchorOrigin: {
            vertical: 'bottom',
            horizontal: 'right'
        }
    })

    setTimeout(() => {
        window.location.href = '/'
    }, 2000)
}

export default openErpApi;