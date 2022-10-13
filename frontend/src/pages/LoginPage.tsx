import React from 'react';
import {GoogleLogin} from "@react-oauth/google";

function LoginPage() {
    return (
        <GoogleLogin
            onSuccess={credentialResponse => {
                console.log(credentialResponse);
            }}
            onError={() => {
                console.log('Login Failed');
            }}
        />
    );
}

export default LoginPage;
