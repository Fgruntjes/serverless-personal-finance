import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import './i18n';

import CssBaseline from '@mui/joy/CssBaseline';
import {CssVarsProvider} from "@mui/joy/styles";
import React, {Suspense} from 'react';
import ReactDOM from 'react-dom/client';
import {
    createBrowserRouter,
    RouterProvider,
} from "react-router-dom";
import {RecoilRoot} from "recoil";

import AuthProvider from "./components/AuthProvider";
import reportWebVitals from './reportWebVitals';
import {routes} from "./routes";
import theme from './theme';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);

const router = createBrowserRouter(routes);

root.render(
    <React.StrictMode>
        <Suspense fallback="Loading">
            <RecoilRoot>
                <CssVarsProvider theme={theme}>
                    <CssBaseline/>
                    <AuthProvider>
                        <RouterProvider router={router}/>
                    </AuthProvider>
                </CssVarsProvider>
            </RecoilRoot>
        </Suspense>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
