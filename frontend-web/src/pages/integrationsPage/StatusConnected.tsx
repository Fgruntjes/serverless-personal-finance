import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import Alert from "@mui/joy/Alert";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

const StatusConnected = (props: { children: JSX.Element }) => {
    const {t} = useTranslation("integrationsPage");

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
            <Typography fontSize="sm" sx={{opacity: 0.8}}>
                {props.children}
            </Typography>
        </div>
    </Alert>;
}

export default StatusConnected;