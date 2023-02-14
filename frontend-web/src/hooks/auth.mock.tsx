import {jest} from "@jest/globals";

import {useAuth} from "./auth";

jest.mock("./auth");

export const mockedUseAuth = jest.mocked(useAuth);
export const mockedSignIn = jest.fn();
export const mockedSignOut = jest.fn();

export function mockLoggedIn(): void {
    mockedUseAuth.mockReturnValue({
        isAuthenticated: true,
        isLoading: false,
        getAccessToken: async () => "fake",
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
}

export function mockLoggedOut(): void {
    mockedUseAuth.mockReturnValue({
        isAuthenticated: false,
        isLoading: false,
        getAccessToken: async () => null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
}

export function mockIsLoading(): void {
    mockedUseAuth.mockReturnValue({
        isAuthenticated: false,
        isLoading: true,
        getAccessToken: async () => null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
}