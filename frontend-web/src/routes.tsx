import React from "react";
import {RouteObject} from "react-router-dom";

import App from "./components/App";
import AuthRequired from "./components/AuthRequired";
import LoginPage from "./pages/LoginPage";
import RouteErrorPage from "./pages/RouteErrorPage";
import {paths} from "./paths";

export const routes: RouteObject[] = [
    {
        path: paths.home,
        element:  <AuthRequired><App /></AuthRequired>,
        errorElement: <RouteErrorPage />,
        children: [],
    },
    {
        path: paths.login,
        element:  <LoginPage />,
        errorElement: <RouteErrorPage />,
        children: [],
    },
];
