import React from "react";
import {RouteObject} from "react-router-dom";
import App from "./components/App";
import ErrorPage from "./pages/ErrorPage";
import RequireAuth from "./components/RequireAuth";

export const paths = {
    home: '/',
    login: '/login',
}

export const routes: RouteObject[] = [
    {
        path: paths.home,
        element:  <RequireAuth><App /></RequireAuth>,
        errorElement: <ErrorPage />,
        children: [],
    },
    {
        path: "/",
        element:  <App />,
        errorElement: <ErrorPage />,
        children: [],
    },
];
