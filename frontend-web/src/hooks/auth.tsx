import {
    googleLogout,
    useGoogleLogin
} from "@react-oauth/google";
import {useEffect} from "react";
import {useTranslation} from "react-i18next";
import {toast} from "react-toastify";
import {atom, useRecoilState} from "recoil";
import {recoilPersist} from "recoil-persist";

import {LoginError} from "../errors/LoginError";

const {persistAtom} = recoilPersist()

export type AuthState = {
    token: string;
    expiresAt: number;
}|null;

const authState = atom<AuthState>({
    key: "authState",
    default: null,
    effects: [persistAtom],
});

export function useAuth(): {authState: AuthState, signIn: VoidFunction, signOut: VoidFunction} {
    const {t} = useTranslation("auth");
    const [currentAuthState, setAuthState] = useRecoilState(authState);
    const login = useGoogleLogin({
        flow: "implicit",
        onError: (error) => {
            throw new LoginError(
                error.error || "unknown",
                error.error_description || "Unknown error thrown"
            );
        },
        onSuccess: (tokenResponse) => {
            toast.success(t("loggedInSuccessful"))
            setAuthState({
                token: tokenResponse.access_token,
                expiresAt: Date.now() + (tokenResponse.expires_in * 1000)
            });
        },
    });
    
    useEffect(() => {
        if (currentAuthState && currentAuthState.expiresAt < Date.now()) {
            setAuthState(null);
        }
    }, [currentAuthState, setAuthState]);
    
    return {
        authState: currentAuthState,
        signIn: () => {
            if (currentAuthState) {
                toast.warning(t("alreadyLoggedIn"))
            } else {
                login();
            }
        },
        signOut: () => {
            if (currentAuthState) {
                setAuthState(null);
                googleLogout();
            } else {
                toast.warning(t("notLoggedIn"))
            }
        },
    }
}
