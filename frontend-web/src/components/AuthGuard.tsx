import React, {useState} from "react";
import {useEffect} from "react";

import {OpenAPI as FunctionIntegrationYnab} from "../generated/App.Function.Integration.Ynab";
import {useAuth} from "../hooks/auth";
import {useTenant} from "../hooks/tenant";
import Loader from "./Loader";

export type AuthGuardProps = { children: JSX.Element };

function AuthGuard({children}: AuthGuardProps) {
    const [accessTokenSet, setAccessTokenSet] = useState(false);
    const {currentTenant} = useTenant();
    let {
        isAuthenticated,
        isLoading,
        signIn,
        getAccessToken,
    } = useAuth();

    useEffect(() => {
        getAccessToken().then(token => {
            if (token) {
                FunctionIntegrationYnab.TOKEN = token;
                setAccessTokenSet(true);
            }
        });
    }, [getAccessToken]);

    useEffect(() => {
        if (currentTenant) {
            FunctionIntegrationYnab.HEADERS = {"X-App-Tenant": currentTenant};
        } else {
            FunctionIntegrationYnab.HEADERS = {};
        }

    }, [currentTenant]);
    
    FunctionIntegrationYnab.BASE = process.env.REACT_APP_FUNCTION_INTEGRATION_YNAB_BASE || FunctionIntegrationYnab.BASE;
    
    useEffect(
        () => {
            if (!isLoading && !isAuthenticated) {
                signIn();
            }
        },
        [isLoading, isAuthenticated, signIn]
    );

    if (isLoading || !isAuthenticated || !accessTokenSet) {
        return <Loader />
    }
    
    return children;
}

export default AuthGuard;
