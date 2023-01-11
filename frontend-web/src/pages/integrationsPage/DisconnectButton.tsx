import {Button} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";

const DisconnectButton = (props: {onClick: () => void}) => {
    const {t} = useTranslation("integrationsPage");

    return (
        <Button color="warning"
            onClick={() => props.onClick()}>
            {t("button.disconnect")}
        </Button>
    );
};

export default DisconnectButton;