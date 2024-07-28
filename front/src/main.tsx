import { StrictMode } from 'react';
import ReactDOM from 'react-dom/client';
import { AuthProvider } from './contexts/AuthContext';
import AppRouter from './routes/'

ReactDOM.createRoot(document.getElementById('root') as HTMLElement).render(
  <StrictMode>
    <AuthProvider>
        <AppRouter />
    </AuthProvider>
  </StrictMode>
);