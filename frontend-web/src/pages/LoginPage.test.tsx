import {jest} from "@jest/globals";
import {
    act, render, screen
} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";

import {useAuth} from "../atoms/auth";
import LoginPage from "./LoginPage";
import click = Simulate.click;
import {Navigate} from "react-router-dom";

jest.mock("../atoms/auth");
jest.mock("react-router-dom")

const mockedUseAuth = jest.mocked(useAuth);
const mockedSignIn = jest.fn();
const mockedSignOut = jest.fn();

test("Call signIn when Login is pressed", () => {
    mockedUseAuth.mockReturnValue({
        authState: null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });
    
    render(<LoginPage />);
    
    const loginButton = screen.getByText(/Login/i);
    expect(loginButton).toBeInTheDocument();
    act(() => {
        click(loginButton);
    });
    
    expect(mockedSignIn).toHaveBeenCalledTimes(1);
});

test("Redirect when logged in", () => {
    mockedUseAuth.mockReturnValue({
        authState: {
            token: "fake" 
        },
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });

    render(<LoginPage />);

    expect(Navigate).toHaveBeenCalledTimes(1);
});