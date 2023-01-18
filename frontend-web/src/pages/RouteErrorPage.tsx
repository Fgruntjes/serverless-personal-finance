import React from "react";
import {useRouteError} from "react-router-dom";

import ErrorPage from "./ErrorPage";

function RouteErrorPage() {
    const routeError = useRouteError();

    return <ErrorPage error={routeError} />;
}

export default RouteErrorPage;
