import WarningIcon from "@mui/icons-material/Warning";
import {
    Alert,
    Typography
} from "@mui/joy";
import React from "react";

import Layout from "../components/Layout"

type ErrorPageProps = {
    error?: any
}

function getErrorString(error: any): string {
    if (typeof error === "string") {
        return error;
    }
    
    if (typeof error !== "object") {
        return `Unknown error type ${typeof error}`;
    }
    
    if (error.statusText) {
        return error.statusText;
    }

    if (error.message) {
        return error.message;
    }
    
    return "Unknown error: " + JSON.stringify(error);
}

function getErrorCode(error: any): string {
    if (typeof error !== "object") {
        return "000";
    }

    if (error.code) {
        return "" + error.code;
    }

    if (error.statusCode) {
        return "" + error.statusCode;
    }

    if (error.status) {
        return "" + error.status;
    }

    return "000";
}

function ErrorPage({error}: ErrorPageProps) {
    console.error(error);
    
    const errorString = getErrorString(error);
    const errorCode = getErrorCode(error);
    return (
        <Layout.Root>
            <Layout.OnePage>
                <Alert
                    startDecorator={<WarningIcon sx={{mx: 0.5}} />}
                    variant="soft"
                    color="danger"
                    size="lg"
                    sx={{alignItems: "flex-start"}}
                    endDecorator={`(${errorCode})`}
                >
                    <div>
                        <Typography color="danger" fontWeight="lg" mt={0.25}>
                            Oops!
                        </Typography>
                        <Typography fontSize="sm" sx={{opacity: 0.8}}>
                            {errorString}
                        </Typography>
                    </div>
                </Alert>
            </Layout.OnePage>
        </Layout.Root>
    );
}

export default ErrorPage;
