import {jest} from "@jest/globals";
import {
    act,
    render,
    screen
} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";

import LoginPage from "./LoginPage";
import click = Simulate.click;
import {Navigate} from "react-router-dom";

import {mockedSignIn, mockLoggedIn, mockLoggedOut} from "../hooks/auth.mock";

jest.mock("react-router-dom")

test("Call signIn when Login is pressed", () => {
    mockLoggedOut();
    
    render(<LoginPage />);
    
    const loginButton = screen.getByText("button.login");
    expect(loginButton).toBeInTheDocument();
    act(() => {
        click(loginButton);
    });
    
    expect(mockedSignIn).toHaveBeenCalledTimes(1);
});

test("Redirect when logged in", () => {
    mockLoggedIn();

    render(<LoginPage />);

    expect(Navigate).toHaveBeenCalledTimes(1);
});