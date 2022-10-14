import React from "react";

function DefaultLayout({children}: { children: JSX.Element }) {
    return (
        <>
            {children}
        </>
    );
}

export default DefaultLayout;
