import React from 'react';
import {Navigate, useLocation} from "react-router-dom";
import {paths} from "../routes";
import {useAuth} from "../atoms/auth";

function AuthRequired({ children }: { children: JSX.Element }) {
  let auth = useAuth();
  let location = useLocation();
  
  if (!auth.user) {
    return <Navigate to={paths.login} state={{ from: location }} replace />;
  }

  return children;
}

export default AuthRequired;
