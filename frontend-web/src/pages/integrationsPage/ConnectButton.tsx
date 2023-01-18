import {Button, ButtonProps} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";
const ConnectButton = (props: ButtonProps) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return (
        <Button color="success" {...props}>
            {t("button.connect")}
        </Button>
    );
};

export default ConnectButton;