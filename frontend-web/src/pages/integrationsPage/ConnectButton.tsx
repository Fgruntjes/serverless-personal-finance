import {Button} from "@mui/joy";
import React from "react";
import {useTranslation} from "react-i18next";
const ConnectButton = (props: {onClick: () => void}) => {
    const {t} = useTranslation("integrationsPage");

    return (
        <Button color="success"
            onClick={() => props.onClick()}>
            {t("button.connect")}
        </Button>
    );
};

export default ConnectButton;