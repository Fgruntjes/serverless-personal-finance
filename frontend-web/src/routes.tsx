import React from "react";
import {RouteObject} from "react-router-dom";

import AppRoot from "./components/AppRoot";
import IndexPage from "./pages/IndexPage";
import IntegrationsPage from "./pages/IntegrationsPage";
import IntegrationReturnYnab from "./pages/integrationsPage/IntegrationReturnYnab";
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
                path: paths.integrations.index,
                element: <IntegrationsPage />,
                children: [
                    {
                        path: paths.integrations.return.ynab,
                        element: <IntegrationReturnYnab />
                    }
                ]
            }
        ],
    },
];
