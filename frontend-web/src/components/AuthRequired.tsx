import React from 'react';
import {Navigate, useLocation} from "react-router-dom";
import {paths} from "../paths";
import {useAuth} from "../atoms/auth";

function AuthRequired({ children }: { children: JSX.Element }) {
  let {authState} = useAuth();
  let location = useLocation();
  
  if (!authState) {
    return <Navigate to={paths.login} state={{ from: location }} replace />;
  }

  return children;
}

export default AuthRequired;
