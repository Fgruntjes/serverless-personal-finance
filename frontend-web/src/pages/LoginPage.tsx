import React from 'react';
import {useAuth} from "../atoms/auth";
import {Navigate} from "react-router-dom";
import {paths} from "../paths";
import OnePageLayout from "../layouts/OnePageLayout";

function LoginPage() {
    const {authState, signIn} = useAuth();
    if (authState) {
        return <Navigate to={paths.home} replace />;
    }
    
    return (
        <OnePageLayout>
            <button onClick={() => signIn()}>Login</button>
        </OnePageLayout>
    );
}

export default LoginPage;
