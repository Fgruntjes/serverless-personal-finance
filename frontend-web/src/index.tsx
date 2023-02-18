import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import "react-toastify/dist/ReactToastify.css";
import "./i18n";

import CssBaseline from "@mui/joy/CssBaseline";
import {CssVarsProvider} from "@mui/joy/styles";
import {QueryClientProvider} from "@tanstack/react-query";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools"
import React, {Suspense} from "react";
import ReactDOM from "react-dom/client";
import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import {ToastContainer} from "react-toastify";
import {RecoilRoot} from "recoil";

import createQueryClient from "./createQueryClient";
import reportWebVitals from "./reportWebVitals";
import {routes} from "./routes";
import theme from "./theme";

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

const queryClient = createQueryClient();
const router = createBrowserRouter(routes);

const ReactQueryDevtoolsProduction = React.lazy(() =>
    import("@tanstack/react-query-devtools/build/lib/index.prod.js").then(
        (d) => ({default: d.ReactQueryDevtools}),
    ),
)

const App = () => {
    const [showDevtools, setShowDevtools] = React.useState(false)

    React.useEffect(() => {
        // @ts-ignore
        window.toggleReactQueryDevtools = () => setShowDevtools((old) => !old)
    }, [])
    
    return (
        <React.StrictMode>
            <Suspense fallback="Loading">
                {/* eslint-disable-next-line react/jsx-no-undef */}
                <QueryClientProvider client={queryClient}>
                    <RecoilRoot>
                        <CssVarsProvider theme={theme}>
                            <CssBaseline/>
                            <RouterProvider router={router}/>
                            <ToastContainer />
                        </CssVarsProvider>
                    </RecoilRoot>
                    {showDevtools && process.env.NODE_ENV === "production" && (
                        <React.Suspense fallback={null}>
                            <ReactQueryDevtoolsProduction />
                        </React.Suspense>
                    )}
                    {process.env.NODE_ENV === "development" && (
                        <ReactQueryDevtools initialIsOpen={false} position="bottom-right" />
                    )}
                </QueryClientProvider>
            </Suspense>
        </React.StrictMode>
    );
}

root.render(<App/>);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
