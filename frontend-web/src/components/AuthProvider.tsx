import {AppState, Auth0Provider} from "@auth0/auth0-react";
import React from "react";
import {useNavigate} from "react-router-dom";

import {configure} from "../configure";
import {useAuth} from "../hooks/auth";
import ErrorPage from "../pages/ErrorPage";

const AuthProviderConfigure = ({children}: { children: JSX.Element }) => {
    const {getAccessToken} = useAuth();
    
    configure(getAccessToken);
    
    return children;
}


function AuthProvider({children}: { children: JSX.Element }) {
    const navigate = useNavigate();
    
    if (!process.env.REACT_APP_AUTH0_DOMAIN) {
        return <ErrorPage error="REACT_APP_AUTH0_DOMAIN not set"></ErrorPage>
    }

    if (!process.env.REACT_APP_AUTH0_CLIENT_ID) {
        return <ErrorPage error="REACT_APP_AUTH0_CLIENT_ID not set"></ErrorPage>
    }

    const onRedirectCallback = async (state?: AppState) => {
        if (state?.returnTo) {
            await navigate(state.returnTo);
        }
    }
    
    return (
        <Auth0Provider
            domain={process.env.REACT_APP_AUTH0_DOMAIN}
            clientId={process.env.REACT_APP_AUTH0_CLIENT_ID}
            authorizationParams={{redirect_uri: window.location.origin}}
            onRedirectCallback={onRedirectCallback}
        >
            <AuthProviderConfigure>{children}</AuthProviderConfigure>
        </Auth0Provider>
    );
}

export default AuthProvider;
