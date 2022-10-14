import IconGoogle from "@mui/icons-material/Google";
import {Button, Typography} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";
import {Navigate} from "react-router-dom";

import {useAuth} from "../atoms/auth";
import Layout from "../components/Layout";
import {paths} from "../paths";

function LoginPage() {
    const {t} = useTranslation("loginPage");
    const {authState, signIn} = useAuth();
    if (authState) {
        return <Navigate to={paths.home} replace />;
    }
    
    return (
        <Layout.Root>
            <Layout.OnePage sx={{
                bgcolor: "background.surface",
                padding: 1,
                border: 1,
            }}>
                <Typography color="primary" fontWeight="lg" mt={0.25}>
                    {t("title")}
                </Typography>
                <Button startDecorator={<IconGoogle />} variant="outlined" onClick={signIn} size="lg">
                    {t("button.login")}
                </Button>
            </Layout.OnePage>
        </Layout.Root>
    );
}

export default LoginPage;
