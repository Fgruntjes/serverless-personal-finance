import React from 'react';

type ErrorPageProps = {
    error?: any
}
function ErrorPage({ error }: ErrorPageProps) {
    console.error(error);

    return (
        <div id="error-page">
            <h1>Oops!</h1>
            <p>Sorry, an unexpected error has occurred.</p>
            <p>
                <i>{typeof error === "string" ? error : error?.message}</i>
            </p>
        </div>
    );
}

export default ErrorPage;
