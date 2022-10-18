import LogoutIcon from "@mui/icons-material/Logout";
import MonetizationOnIcon from "@mui/icons-material/MonetizationOn";
import {
    Box,
    Button,
    IconButton,
    Typography
} from "@mui/joy";
import React from "react";
import {Trans, useTranslation} from "react-i18next";
import {Outlet} from "react-router-dom";

import {useAuth} from "../hooks/auth";
import AuthGuard from "./AuthGuard";
import Layout from "./Layout"

function AppRoot() {
    const {t} = useTranslation();
    const {signOut} = useAuth();
    
    return (
        <AuthGuard>
            <Layout.Root>
                <Layout.Header>
                    <Box
                        sx={{
                            display: "flex",
                            flexDirection: "row",
                            alignItems: "center",
                            gap: 1.5,
                        }}
                    >
                        <IconButton
                            size="sm"
                            variant="solid"
                            sx={{display: {sm: "inline-flex"}}}
                        >
                            <MonetizationOnIcon />
                        </IconButton>
                        <Typography component="h1" fontWeight="xl">
                            {t("appTitle")}
                        </Typography>
                    </Box>
                    
                    <Box sx={{
                        display: "flex", flexDirection: "row", gap: 1.5
                    }}>
                        <Button aria-label="Like" variant="outlined" color="neutral" onClick={() => signOut()}>
                            <LogoutIcon />{t("button.logout")}
                        </Button>
                    </Box>
                </Layout.Header>
                <Layout.Main>
                    <Outlet />
                </Layout.Main>
                <small>
                    <Trans i18nKey="runMode" values={{mode: process.env.NODE_ENV}} components={{bold: <strong />}} />
                </small>
            </Layout.Root>
        </AuthGuard>
    );
}

export default AppRoot;
