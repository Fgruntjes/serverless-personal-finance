import {useAuth0} from "@auth0/auth0-react";
import {renderHook} from "@testing-library/react";
import {toast} from "react-toastify";

import {LoginError} from "../errors/LoginError";
import {useAuth} from "./auth";

jest.mock("@auth0/auth0-react");
jest.mock("react-toastify");
jest.unmock("./auth");

describe(useAuth.name, () => {
    const mockedUseAuth0 = jest.mocked(useAuth0);
    const mockedLoginWithRedirect = jest.fn();
    const mockedLogout = jest.fn();
    const mockedGetAccessTokenSilently = jest.fn(); 

    function mockLoggedIn(): void {
        mockedLoginWithRedirect.mockResolvedValue(undefined);
        
        mockedUseAuth0.mockReturnValue({
            loginWithRedirect: mockedLoginWithRedirect,
            logout: mockedLogout,
            getAccessTokenSilently: mockedGetAccessTokenSilently,
            isAuthenticated: true,
            isLoading: false,
            error: undefined,

            getAccessTokenWithPopup: jest.fn(),
            getIdTokenClaims: jest.fn(),
            loginWithPopup: jest.fn(),
            handleRedirectCallback: jest.fn(),
        });
    }

    function mockLoggedOut(): void {
        mockedLoginWithRedirect.mockResolvedValue(undefined);
        
        mockedUseAuth0.mockReturnValue({
            loginWithRedirect: mockedLoginWithRedirect,
            logout: mockedLogout,
            getAccessTokenSilently: mockedGetAccessTokenSilently,
            isAuthenticated: false,
            isLoading: false,
            error: undefined,

            getAccessTokenWithPopup: jest.fn(),
            getIdTokenClaims: jest.fn(),
            loginWithPopup: jest.fn(),
            handleRedirectCallback: jest.fn(),
        });
    }



    test("Call signIn when logged out", () => {
        mockLoggedOut();
        
        renderHook(() => useAuth()).result.current.signIn();
    
        expect(mockedLoginWithRedirect).toHaveBeenCalledTimes(1);
    });
    
    test("Call signIn when logged in", () => {
        mockLoggedIn();
        
        renderHook(() => useAuth()).result.current.signIn();
    
        expect(mockedLoginWithRedirect).not.toBeCalled();
        expect(toast.warning).toHaveBeenCalledWith("alreadyLoggedIn")
    });
    
    test("Call signOut when logged in", () => {
        mockLoggedIn();
    
        renderHook(() => useAuth()).result.current.signOut();
        
        expect(mockedLogout).toHaveBeenCalledTimes(1);
    });
    
    test("Call signOut when logged out", () => {
        mockLoggedOut();
    
        renderHook(() => useAuth()).result.current.signOut();
    
        expect(mockedLoginWithRedirect).not.toBeCalled();
        expect(toast.warning).toHaveBeenCalledWith("notLoggedIn")
    });
    
    test("Failed sign in", () => {
        jest.spyOn(console, "error").mockImplementation();
        mockedUseAuth0.mockReturnValue({
            loginWithRedirect: mockedLoginWithRedirect,
            logout: mockedLogout,
            getAccessTokenSilently: mockedGetAccessTokenSilently,
            isAuthenticated: false,
            isLoading: false,
            error: new Error("Some server error"),

            getAccessTokenWithPopup: jest.fn(),
            getIdTokenClaims: jest.fn(),
            loginWithPopup: jest.fn(),
            handleRedirectCallback: jest.fn(),
        });
        
        
        try {
            renderHook(() => useAuth())
        } catch (e) {
            // eslint-disable-next-line jest/no-conditional-expect
            expect(e).toEqual(new LoginError("Some server error"))
            return;
        }
    
        throw new Error("should never get here");
    });
});