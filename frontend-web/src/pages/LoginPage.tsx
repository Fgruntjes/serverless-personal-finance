import {Modal, ModalDialog, Typography} from "@mui/joy";
import React from 'react';
import {Navigate} from "react-router-dom";

import {useAuth} from "../atoms/auth";
import {paths} from "../paths";

function LoginPage() {
    const {authState, signIn} = useAuth();
    if (authState) {
        return <Navigate to={paths.home} replace />;
    }
    
    return (
        <Modal open={true}>
            <ModalDialog>
                <Typography>
                    <button onClick={() => signIn()}>Login</button>
                </Typography>
            </ModalDialog>
        </Modal>
    );
}

export default LoginPage;
