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
        <div id="error-page">
            <h1>Oops!</h1>
            <p>Sorry, an unexpected error has occurred.</p>
            <p>
                <i>{errorString}</i>
            </p>
        </div>
    );
}

export default ErrorPage;
