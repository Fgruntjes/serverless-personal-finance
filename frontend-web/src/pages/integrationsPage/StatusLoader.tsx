import Alert from "@mui/joy/Alert";
import CircularProgress from "@mui/joy/CircularProgress";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

const StatusLoader = () => {
    const {t} = useTranslation("integrationsPage");

    return <Alert
        color="info"
        variant="outlined"
        sx={{alignItems: "flex-start"}}
        startDecorator={<CircularProgress sx={{mt: "2px", mx: "4px"}}/>}>
        <div>
            <Typography fontWeight="lg" mt={0.25}>
                {t("label.loading")}
            </Typography>
        </div>
    </Alert>;
}

export default StatusLoader;