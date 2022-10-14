import {googleLogout, useGoogleLogin} from "@react-oauth/google";
import {toast} from "react-toastify";
import {SetterOrUpdater, useRecoilState} from "recoil";

import {AuthState, useAuth} from "./auth";

jest.mock("@react-oauth/google");
jest.mock("recoil");
jest.mock("react-toastify");

const mockedLogin = jest.fn();
const mockedUseGoogleLogin = jest.mocked(useGoogleLogin);
const mockedUseRecoilState = jest.mocked(useRecoilState<AuthState>);
const setAuthState: SetterOrUpdater<AuthState> = (callback) => {
    if (typeof callback === "function") {
        callback(null);       
    }
};

test("Call signIn when logged out", () => {
    mockedUseRecoilState.mockReturnValue([null, setAuthState])
    mockedUseGoogleLogin.mockReturnValue(mockedLogin);

    const auth = useAuth();
    auth.signIn();

    expect(mockedLogin).toHaveBeenCalledTimes(1);
});

test("Call signIn when logged in", () => {
    mockedUseRecoilState.mockReturnValue([{token: "fake"}, setAuthState])
    mockedUseGoogleLogin.mockReturnValue(mockedLogin);

    const auth = useAuth();
    auth.signIn();

    expect(mockedLogin).not.toBeCalled();
    expect(toast.warning).toHaveBeenCalledWith("alreadyLoggedIn")
});

test("Call signOut when logged out", () => {
    mockedUseRecoilState.mockReturnValue([{token: "fake"}, setAuthState])
    
    const auth = useAuth();
    auth.signOut();
    
    expect(googleLogout).toHaveBeenCalledTimes(1);
});

test("Call signOut when logged in", () => {
    mockedUseRecoilState.mockReturnValue([null, setAuthState])
    
    const auth = useAuth();
    auth.signOut();

    expect(mockedLogin).not.toBeCalled();
    expect(toast.warning).toHaveBeenCalledWith("notLoggedIn")
});

test("Succesfull sign in", () => {
});