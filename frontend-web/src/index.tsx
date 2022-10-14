import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

import CssBaseline from '@mui/material/CssBaseline';
import {ThemeProvider} from "@mui/material/styles";
import React from 'react';
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
        <RecoilRoot>
            <ThemeProvider theme={theme}>
                <CssBaseline/>
                <AuthProvider>
                    <RouterProvider router={router}/>
                </AuthProvider>
            </ThemeProvider>
        </RecoilRoot>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
