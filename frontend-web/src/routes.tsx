import React from "react";
import {RouteObject} from "react-router-dom";

import AppRoot from "./components/AppRoot";
import ImportsPage from "./pages/ImportsPage";
import IndexPage from "./pages/IndexPage";
import LoginPage from "./pages/LoginPage";
import RouteErrorPage from "./pages/RouteErrorPage";
import {paths} from "./paths";

export const routes: RouteObject[] = [
    {
        path: paths.home,
        element:  <AppRoot />,
        errorElement: <RouteErrorPage />,
        children: [
            {
                index: true,
                element: <IndexPage />
            },
            {
                path: paths.imports,
                element: <ImportsPage />,
            }
        ],
    },
    {
        path: paths.login,
        element:  <LoginPage />,
        errorElement: <RouteErrorPage />,
        children: [],
    },
];
