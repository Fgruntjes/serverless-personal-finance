import {atom, useRecoilState} from "recoil";
import {googleLogout, useGoogleLogin} from "@react-oauth/google";

type AuthState = {
    token: string;
}

const authState = atom<AuthState|null>({
    key: 'authState',
    default: null,
});

export function useAuth() {
    const [user, setUser] = useRecoilState(authState);
    const login = useGoogleLogin({
        onSuccess: tokenResponse => {
            setUser({
                token: tokenResponse.access_token,
            })
        },
    });
    
    return {
        user: user,
        signIn: () => {
            login();
        },
        signOut: () => {
            setUser(() => {
                googleLogout();
                return null;
            });
        },
    }
}
