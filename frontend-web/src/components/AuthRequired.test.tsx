import {jest} from "@jest/globals";
import {render, screen} from "@testing-library/react";
import React from "react";
import {Navigate} from "react-router-dom";

import {useAuth} from "../atoms/auth";
import AuthRequired from "./AuthRequired";

jest.mock("../atoms/auth");
jest.mock("react-router-dom")

const mockedUseAuth = jest.mocked(useAuth);
const mockedSignIn = jest.fn();
const mockedSignOut = jest.fn();

test("Redirect when not logged in", () => {
    mockedUseAuth.mockReturnValue({
        authState: null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });

    render(<AuthRequired><p data-testid="loggedin">logged in</p></AuthRequired>);

    expect(Navigate).toHaveBeenCalledTimes(1);
});

test("Render children when logged in", async () => {
    mockedUseAuth.mockReturnValue({
        authState: {
            token: "fake"
        },
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });

    render(<AuthRequired><p data-testid="loggedin">logged in</p></AuthRequired>);
    expect(await screen.findByTestId("loggedin")).toBeInTheDocument();
});