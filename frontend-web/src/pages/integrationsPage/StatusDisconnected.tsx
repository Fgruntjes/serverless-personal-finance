import WarningIcon from "@mui/icons-material/Warning";
import Alert from "@mui/joy/Alert";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

const StatusDisconnected = (props: { children: JSX.Element }) => {
    const {t} = useTranslation("integrationsPage");

    return <Alert
        color="danger"
        variant="outlined"
        sx={{alignItems: "flex-start"}}
        startDecorator={<WarningIcon sx={{mt: "2px", mx: "4px"}} fontSize="large"/>}>
        <div>
            <Typography fontWeight="lg" mt={0.25}>
                {t("label.disconnected")}
            </Typography>
            {props.children}
        </div>
    </Alert>;
}

export default StatusDisconnected;