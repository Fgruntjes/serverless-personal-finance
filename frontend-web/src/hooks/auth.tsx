import {useAuth0} from "@auth0/auth0-react";
import {useEffect} from "react";
import {useTranslation} from "react-i18next";
import {toast} from "react-toastify";

import {LoginError} from "../errors/LoginError";
import {TranslationNamespaces} from "../locales/namespaces";

type AuthContext = {
    isAuthenticated: boolean,
    getAccessToken: TokenResolver,
    isLoading: boolean,
    signIn: VoidFunction,
    signOut: VoidFunction
};

export type TokenResolver = () => Promise<string|undefined>;

export function useAuth(): AuthContext {
    const {
        loginWithRedirect,
        logout,
        getAccessTokenSilently,
        isAuthenticated,
        isLoading,
        error
    } = useAuth0();
    const {t} = useTranslation(TranslationNamespaces.Auth);

    useEffect(() => {
        if (error) {
            throw new LoginError(error.message || "unknown");
        }
    }, [error]);
    
    return {
        isAuthenticated,
        isLoading,
        getAccessToken: async () => {
            return !isAuthenticated ? "" : await getAccessTokenSilently() ?? "";
        },
        signIn: () => {
            if (isAuthenticated) {
                toast.warning(t("alreadyLoggedIn"))
            } else {
                const loginPromise = loginWithRedirect({appState: {returnTo: `${window.location.pathname}${window.location.search}`}});
                loginPromise.catch(error => { throw error; });
            }
        },
        signOut: () => {
            if (isAuthenticated) {
                logout({logoutParams: {returnTo: ""}});
            } else {
                toast.warning(t("notLoggedIn"))
            }
        },
    }
}
