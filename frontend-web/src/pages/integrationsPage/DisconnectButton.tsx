import {Button, ButtonProps} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";

const DisconnectButton = (props: ButtonProps) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return (
        <Button color="danger" {...props}>
            {t("button.disconnect")}
        </Button>
    );
};

export default DisconnectButton;