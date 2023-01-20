import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import Alert from "@mui/joy/Alert";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../../locales/namespaces";

const StatusConnected = (props: { children: JSX.Element }) => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return <Alert
        color="success"
        variant="outlined"
        sx={{alignItems: "flex-start"}}
        startDecorator={<CheckCircleIcon sx={{mt: "2px", mx: "4px"}} fontSize="large"/>}
    >
        <div>
            <Typography fontWeight="lg" mt={0.25}>
                {t("label.connected")}
            </Typography>
            {props.children}
        </div>
    </Alert>;
}

export default StatusConnected;