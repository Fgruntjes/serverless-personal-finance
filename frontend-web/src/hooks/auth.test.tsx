import {
    googleLogout,
    OverridableTokenClientConfig,
    TokenResponse,
    useGoogleLogin,
    UseGoogleLoginOptionsImplicitFlow
} from "@react-oauth/google";
import {renderHook} from "@testing-library/react";
import {toast} from "react-toastify";
import {SetterOrUpdater, useRecoilState} from "recoil";

import {LoginError} from "../errors/LoginError";
import {AuthState, useAuth} from "./auth";
import {mockLoggedInToken} from "./auth.mock";

jest.mock("@react-oauth/google");
jest.mock("recoil");
jest.mock("react-toastify");

type useGoogleLoginSpy = (options: UseGoogleLoginOptionsImplicitFlow) => (overrideConfig?: OverridableTokenClientConfig) => void;

const mockedLogin = jest.fn();
const mockedUseGoogleLogin = jest.mocked(useGoogleLogin) as jest.MockedFn<useGoogleLoginSpy>;
const mockedUseRecoilState = jest.mocked(useRecoilState<AuthState>);
const setAuthState: SetterOrUpdater<AuthState> = jest.fn();

const successTokenResponse: Omit<TokenResponse, "error" | "error_description" | "error_uri"> = {
    access_token: "fake",
    expires_in: 3600,
    token_type: "Bearer",
    scope: "login",
    prompt: "prompt",
}

test("Call signIn when logged out", () => {
    mockedUseRecoilState.mockReturnValue([null, setAuthState])
    mockedUseGoogleLogin.mockReturnValue(mockedLogin);

    renderHook(() => useAuth()).result.current.signIn();

    expect(mockedLogin).toHaveBeenCalledTimes(1);
});

test("Call signIn when logged in", () => {
    mockedUseRecoilState.mockReturnValue([mockLoggedInToken, setAuthState])
    mockedUseGoogleLogin.mockReturnValue(mockedLogin);

    renderHook(() => useAuth()).result.current.signIn();

    expect(mockedLogin).not.toBeCalled();
    expect(toast.warning).toHaveBeenCalledWith("alreadyLoggedIn")
});

test("Call signOut when logged in", () => {
    mockedUseRecoilState.mockReturnValue([mockLoggedInToken, setAuthState])

    renderHook(() => useAuth()).result.current.signOut();
    
    expect(googleLogout).toHaveBeenCalledTimes(1);
});

test("Call signOut when logged out", () => {
    mockedUseRecoilState.mockReturnValue([null, setAuthState])

    renderHook(() => useAuth()).result.current.signOut();

    expect(mockedLogin).not.toBeCalled();
    expect(toast.warning).toHaveBeenCalledWith("notLoggedIn")
});

test("Succesfull sign in", () => {
    mockedUseRecoilState.mockReturnValue([null, setAuthState])
    mockedUseGoogleLogin.mockImplementation((options) => {
        const onSuccess = options?.onSuccess;
        if (!onSuccess) {
            throw new Error("onSuccess option should not be null");
        }

        onSuccess(successTokenResponse);
        return mockedLogin;
    });

    const dateMock = jest
        .spyOn(global.Date, "now")
        .mockImplementationOnce(() =>
            new Date("2019-05-14T11:01:58.135Z").valueOf()
        );
    renderHook(() => useAuth());

    expect(setAuthState).toHaveBeenCalledWith({
        expiresAt: 1557835318135,
        token: successTokenResponse.access_token
    });
    expect(toast.success).toHaveBeenCalledWith("loggedInSuccessful");

    dateMock.mockRestore();
});

test("Failed sign in", () => {
    jest.spyOn(console, "error").mockImplementation()
    mockedUseRecoilState.mockReturnValue([null, setAuthState])
    mockedUseGoogleLogin.mockImplementation((options) => {
        const onError = options?.onError;
        if (!onError) {
            throw new Error("onError option should not be null");
        }

        onError({error: "server_error", error_description: "Some server error"});
        return mockedLogin;
    });

    try {
        renderHook(() => useAuth())
    } catch (e) {
        // eslint-disable-next-line jest/no-conditional-expect
        expect(e).toEqual(new LoginError("server_error", "Some server error"))
        return;
    }

    throw new Error("should never get here");
});

test("Token expired", () => {
    mockedUseRecoilState.mockReturnValue([{token: "Fake", expiresAt: 123}, setAuthState])
    mockedUseGoogleLogin.mockReturnValue(mockedLogin);

    renderHook(() => useAuth());

    expect(setAuthState).toHaveBeenCalledWith(null);
});