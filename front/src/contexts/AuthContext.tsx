import React, { ReactNode, createContext, useContext, useEffect } from 'react';
import openErpApi from '../services/OpenErpApi';

import { HOME_PAGE } from './../config';

type AuthContextType = {
    login: (username: string, password: string) => void;
    logout: () => void;
    signed: boolean;
};

const AuthContext = createContext<AuthContextType>({
    login: () => {},
    logout: () => {},
    signed: false
});

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    useEffect(() => {
        const accessToken = localStorage.getItem('openApiAccessToken')

        if (!accessToken) logout;
    }, []);

    const login = async (username: string, password: string) => {
        const data = {
            username: username,
            password: password
        };

        await openErpApi.post('login', data)
            .then(response => {
                localStorage.setItem('openApiAccessToken', response.data.accessToken);
                localStorage.setItem('openApiRefreshToken', response.data.refreshToken);

                if (response.data.lastPasswordUpdatedAt.length === 0) {
                    window.location.href = '/users/updatePassword'
                } else {
                    window.location.href = HOME_PAGE
                }
            });

        return;
    };

    const logout = async () => {
        localStorage.removeItem('openApiAccessToken');
        localStorage.removeItem('openApiRefreshToken');

        window.location.href = '/';
    };

    return (
        <AuthContext.Provider value={{ login, logout, signed: !!localStorage.getItem('openApiAccessToken') }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => useContext(AuthContext);
