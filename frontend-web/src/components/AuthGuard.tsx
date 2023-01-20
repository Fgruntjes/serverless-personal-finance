import React from "react";
import {Navigate, useLocation} from "react-router-dom";

import {useAuth} from "../hooks/auth";
import {paths} from "../paths";

export type AuthGuardProps = { children: JSX.Element };

function AuthGuard({children}: AuthGuardProps) {
    let {authState} = useAuth();
    let location = useLocation();
  
    if (!authState) {
        return <Navigate to={paths.login} state={{from: location}} replace />;
    }

    return children;
}

export default AuthGuard;
