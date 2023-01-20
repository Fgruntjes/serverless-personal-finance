import {jest} from "@jest/globals";

import {AuthState, useAuth} from "./auth";

export const mockLoggedInToken: AuthState = {
    token: "fake",
    expiresAt: Date.now() + 1000 * 1000
}

jest.mock("./auth");

export const mockedUseAuth = jest.mocked(useAuth);
export const mockedSignIn = jest.fn();
export const mockedSignOut = jest.fn();

export function mockLoggedIn(): void {
    mockedUseAuth.mockReturnValue({
        authState: mockLoggedInToken,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
}

export function mockLoggedOut(): void {
    mockedUseAuth.mockReturnValue({
        authState: null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
}