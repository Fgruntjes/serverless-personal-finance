import Alert from "@mui/joy/Alert";
import CircularProgress from "@mui/joy/CircularProgress";
import Typography from "@mui/joy/Typography";
import React from "react";
import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../locales/namespaces";

const Loader = () => {
    const {t} = useTranslation(TranslationNamespaces.Core);

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

export default Loader;