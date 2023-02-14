import React from "react";
import {Outlet} from "react-router-dom";

import AuthGuard from "./AuthGuard";
import AuthProvider from "./AuthProvider";
import Layout from "./Layout"

function AppRoot() {
    return (
        <AuthProvider>
            <AuthGuard>
                <Layout.RootDefault>
                    <Layout.Header />
                    <Layout.MainWrapper>
                        <Layout.Main>
                            <Outlet />
                        </Layout.Main>
                        <Layout.SideNav />
                    </Layout.MainWrapper>
                    <Layout.Footer />
                </Layout.RootDefault>
            </AuthGuard>
        </AuthProvider>
    );
}

export default AppRoot;
