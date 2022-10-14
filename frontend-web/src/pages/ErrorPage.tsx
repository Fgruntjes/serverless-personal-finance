import {Sheet, Typography} from "@mui/joy";
import React from 'react';

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
    
    return `Unknown error missing field statusText or message: ` + JSON.stringify(error);
}

function ErrorPage({error}: ErrorPageProps) {
    console.error(error);
    
    const errorString = getErrorString(error);
    return (
        <Sheet variant="outlined">
            <Typography component="h2" level="h4">Oops!</Typography>
            <Typography id="modal-desc" textColor="text.tertiary">
                {errorString}
            </Typography>
        </Sheet>
    );
}

export default ErrorPage;
