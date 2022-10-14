import {googleLogout, useGoogleLogin} from "@react-oauth/google";
import {useTranslation} from "react-i18next";
import {toast} from "react-toastify";
import {atom, useRecoilState} from "recoil";

export type AuthState = {
    token: string;
}|null;

const authState = atom<AuthState>({
    key: "authState",
    default: null,
});

export function useAuth(): {authState: AuthState, signIn: VoidFunction, signOut: VoidFunction} {
    const {t} = useTranslation("auth");
    const [currentAuthState, setAuthState] = useRecoilState(authState);
    const login = useGoogleLogin({
        flow: "implicit",
        onError: console.error,
        onSuccess: (tokenResponse) => {
            toast.success(t("loggedInSuccessful"))
            setAuthState({
                token: tokenResponse.access_token,
            });
        },
    });
    
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
                setAuthState(() => {
                    googleLogout();
                    return null;
                });
            } else {
                toast.warning(t("notLoggedIn"))
            }
        },
    }
}
