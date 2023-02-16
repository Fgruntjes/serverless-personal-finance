import React from "react";
import {useEffect} from "react";

import {useAuth} from "../hooks/auth";
import Loader from "./Loader";

export type AuthGuardProps = { children: JSX.Element };

function AuthGuard({children}: AuthGuardProps) {
    let {
        isAuthenticated, isLoading, signIn
    } = useAuth();
    
    useEffect(
        () => {
            if (!isLoading && !isAuthenticated) {
                signIn();
            }
        },
        [isLoading, isAuthenticated, signIn]
    );

    if (isLoading) {
        return <Loader />
    }
    
    return children;
}

export default AuthGuard;
