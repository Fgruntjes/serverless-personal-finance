import {Button} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";

const DisconnectButton = (props: {onClick: () => void}) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return (
        <Button color="danger"
            onClick={() => props.onClick()}>
            {t("button.disconnect")}
        </Button>
    );
};

export default DisconnectButton;