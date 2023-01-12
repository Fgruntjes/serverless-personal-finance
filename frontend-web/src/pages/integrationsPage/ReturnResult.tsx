import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import InfoIcon from "@mui/icons-material/InfoOutlined";
import WarningIcon from "@mui/icons-material/Warning";
import Alert from "@mui/joy/Alert";
import {DefaultColorPalette} from "@mui/joy/styles/types/colorSystem";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

import Loader from "../../components/Loader";

type ReturnResultProps = {
    variant: "success"|"error"|"loading";
    integration: string
    error?: string;
}

const ReturnResult = (props: ReturnResultProps) => {
    const {t} = useTranslation("integrationsPage");
    
    let containerColor: DefaultColorPalette = "info";
    let helpText = null;
    let Icon = null;
    switch (props.variant) {
    case "loading":
        containerColor = "info"
        Icon = InfoIcon;
        break;
    case "error":
        containerColor = "warning"
        Icon = WarningIcon;
        break;
    case "success":
        containerColor = "success"
        Icon = CheckCircleIcon;
        break;
    }

    return <Alert
        color={containerColor}
        variant="outlined"
        sx={{alignItems: "flex-start"}}
        startDecorator={<Icon sx={{mt: "2px", mx: "4px"}} fontSize="large"/>}
    >
        <div>
            <Typography fontWeight="lg" mt={0.25}>
                {t("label.processingReturn", {integration: props.integration})}
            </Typography>
            <Typography fontSize="sm" sx={{opacity: 0.8}}>
                {props.variant === "loading" ? <Loader /> : null}
                {helpText}
            </Typography>
        </div>
    </Alert>;
}

export default ReturnResult;