import React from 'react';
import {GoogleOAuthProvider} from "@react-oauth/google";
import ErrorPage from "../pages/ErrorPage";

function AuthProvider({ children }: { children: JSX.Element }) {
    const clientId = process.env.REACT_APP_GOOGLE_AUTH_CLIENT_ID;
    if (!clientId) {
        return <ErrorPage error="REACT_APP_GOOGLE_AUTH_CLIENT_ID not set"></ErrorPage>
    }
    
    return (
        <GoogleOAuthProvider clientId={clientId}>
            {children}
        </GoogleOAuthProvider>
    );
}

export default AuthProvider;
