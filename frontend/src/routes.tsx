import React from "react";
import {RouteObject} from "react-router-dom";
import App from "./components/App";
import AuthRequired from "./components/AuthRequired";
import RouteErrorPage from "./pages/RouteErrorPage";
import LoginPage from "./pages/LoginPage";

export const paths = {
    home: '/',
    login: '/login',
}

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
