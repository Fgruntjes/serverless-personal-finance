import React from 'react';
import {Navigate} from "react-router-dom";

import {useAuth} from "../atoms/auth";
import OnePageLayout from "../layouts/OnePageLayout";
import {paths} from "../paths";

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
