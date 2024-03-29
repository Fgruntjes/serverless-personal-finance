import LogoutIcon from "@mui/icons-material/Logout";
import {Button} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

import {useAuth} from "../hooks/auth";
import {TranslationNamespaces} from "../locales/namespaces";

export const LogoutButton = () => {
    const {t} = useTranslation(TranslationNamespaces.Core);
    const {signOut} = useAuth();
    
    return (
        <Button aria-label="Like" variant="outlined" color="neutral" onClick={() => signOut()}>
            <LogoutIcon />{t("button.logout")}
        </Button>
    );
}
