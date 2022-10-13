import React from "react";
import {atom, useRecoilState} from "recoil";

type User = {
    email: string;
    token: string;
}

type AuthContext = {
    user: User|null;
    signIn: () => void;
    signOut: () => void;
}

const authState = atom<User|null>({
    key: 'authState',
});

export function useAuth() {
    const [user, setUser] = useRecoilState(authState);
    
    return {
        user: user,
        signIn: () => {
            
        },
        signOut: () => {
            setUser(null)
        },
    }
}
