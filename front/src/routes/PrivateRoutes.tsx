import { Navigate } from "react-router-dom";
import { useAuth } from './../contexts/AuthContext'
import App from "../App";

export const PrivateRoute: React.FC = () => {
    const { signed } = useAuth();

    return signed ? <App /> : <Navigate to="/" />;
}