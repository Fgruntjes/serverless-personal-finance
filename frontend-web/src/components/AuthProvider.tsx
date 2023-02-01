import {GoogleOAuthProvider} from "@react-oauth/google";
import React from "react";

import {configure} from "../configure";
import {useAuth} from "../hooks/auth";
import ErrorPage from "../pages/ErrorPage";

const AuthProviderConfigure = ({children}: { children: JSX.Element }) => {

    const {authState} = useAuth();
    configure(authState);
    
    return children;
}


function AuthProvider({children, clientId}: { children: JSX.Element, clientId?: string }) {
    if (!clientId) {
        return <ErrorPage error="REACT_APP_OAUTH_CLIENT_ID not set"></ErrorPage>
    }
    
    return (
        <GoogleOAuthProvider clientId={clientId}>
            <AuthProviderConfigure>{children}</AuthProviderConfigure>
        </GoogleOAuthProvider>
    );
}

export default AuthProvider;
