import IconGoogle from "@mui/icons-material/Google";
import {
    Box,
    Button,
    Typography
} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";
import {Navigate} from "react-router-dom";

import Layout from "../components/Layout";
import {useAuth} from "../hooks/auth";
import {paths} from "../paths";

function LoginPage() {
    const {t} = useTranslation("loginPage");
    const {authState, signIn} = useAuth();
    if (authState) {
        return <Navigate to={paths.home} replace />;
    }
    
    return (
        <Layout.RootCenter>
            <Box sx={{
                padding: 1,
                border: 1,
            }}>
                <Typography color="primary" fontWeight="lg" mt={0.25}>
                    {t("title")}
                </Typography>
                <Button startDecorator={<IconGoogle />} variant="outlined" onClick={signIn} size="lg">
                    {t("button.login")}
                </Button>
            </Box>
        </Layout.RootCenter>
    );
}

export default LoginPage;
