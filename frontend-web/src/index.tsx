import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import "react-toastify/dist/ReactToastify.css";
import "./i18n";

import CssBaseline from "@mui/joy/CssBaseline";
import {CssVarsProvider} from "@mui/joy/styles";
import React, {Suspense} from "react";
import ReactDOM from "react-dom/client";
import {QueryClientProvider} from "react-query"
import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import {ToastContainer} from "react-toastify";
import {RecoilRoot} from "recoil";

import AuthProvider from "./components/AuthProvider";
import reportWebVitals from "./reportWebVitals";
import {routes} from "./routes";
import {setup} from "./setup";
import createQueryClient from "./setup/createQueryClient";
import theme from "./theme";

setup();
const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

const queryClient = createQueryClient();
const router = createBrowserRouter(routes);

root.render(
    <React.StrictMode>
        <Suspense fallback="Loading">
            <QueryClientProvider client={queryClient}>
                <RecoilRoot>
                    <CssVarsProvider theme={theme}>
                        <CssBaseline/>
                        <AuthProvider clientId={process.env.REACT_APP_GOOGLE_AUTH_CLIENT_ID}>
                            <RouterProvider router={router}/>
                        </AuthProvider>
                        <ToastContainer />
                    </CssVarsProvider>
                </RecoilRoot>
            </QueryClientProvider>
        </Suspense>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
