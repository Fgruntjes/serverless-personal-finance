import React from 'react';
import {Navigate, useLocation} from "react-router-dom";
import {paths} from "../routes";
import {useAuth} from "../atoms/auth";

function RequireAuth({ children }: { children: JSX.Element }) {
  let auth = useAuth();
  let location = useLocation();
  
  if (!auth) {
    return <Navigate to={paths.login} state={{ from: location }} replace />;
  }

  return children;
}

export default RequireAuth;
