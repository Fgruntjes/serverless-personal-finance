import {GoogleOAuthProvider} from "@react-oauth/google";
import React from "react";

import ErrorPage from "../pages/ErrorPage";

function AuthProvider({children, clientId}: { children: JSX.Element, clientId?: string }) {
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
