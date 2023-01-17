import {Button} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";
const ConnectButton = (props: {onClick: () => void}) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return (
        <Button color="success"
            onClick={() => props.onClick()}>
            {t("button.connect")}
        </Button>
    );
};

export default ConnectButton;