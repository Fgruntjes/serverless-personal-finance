import React from 'react';
import {jest} from "@jest/globals";
import {useAuth} from "../atoms/auth";
import {render, screen} from "@testing-library/react";
import {Navigate} from "react-router-dom";
import AuthRequired from "./AuthRequired";

jest.mock('../atoms/auth');
jest.mock("react-router-dom")

const mockedUseAuth = jest.mocked(useAuth);
const mockedSignIn = jest.fn();
const mockedSignOut = jest.fn();

test('Redirect when not logged in', () => {
    mockedUseAuth.mockReturnValue({
        authState: null,
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });

    render(<AuthRequired><p data-testid="loggedin">logged in</p></AuthRequired>);

    expect(Navigate).toHaveBeenCalledTimes(1);
});

test('Redirect when not logged in', () => {
    mockedUseAuth.mockReturnValue({
        authState: {token: "fake"},
        signIn: mockedSignIn,
        signOut: mockedSignOut,
    });

    render(<AuthRequired><p data-testid="loggedin">logged in</p></AuthRequired>);
    expect(screen.findByTestId('loggedin')).toBeInTheDocument();
});