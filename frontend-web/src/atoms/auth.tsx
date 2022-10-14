import {googleLogout, useGoogleLogin} from "@react-oauth/google";
import {atom, useRecoilState} from "recoil";

export type AuthState = {
    token: string;
}|null;

const authState = atom<AuthState>({
    key: 'authState',
    default: null,
});

export function useAuth(): {authState: AuthState, signIn: VoidFunction, signOut: VoidFunction} {
    const [currentAuthState, setAuthState] = useRecoilState(authState);
    const login = useGoogleLogin({
        flow: 'implicit',
        onError: console.error,
        onSuccess: (tokenResponse) => {
            setAuthState({
                token: tokenResponse.access_token,
            });
        },
    });
    
    return {
        authState: currentAuthState,
        signIn: () => {
            login();
        },
        signOut: () => {
            setAuthState(() => {
                googleLogout();
                return null;
            });
        },
    }
}
