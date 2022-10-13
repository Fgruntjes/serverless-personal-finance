import React from 'react';
import {useAuth} from "../atoms/auth";
import {Navigate} from "react-router-dom";
import {paths} from "../routes";

function LoginPage() {
    const {user, signIn} = useAuth();
    if (user) {
        // TODO send status message already logged in
        return <Navigate to={paths.home} replace />;
    }
    
    return (
        <button onClick={() => signIn()}>Login</button>
    );
}

export default LoginPage;
