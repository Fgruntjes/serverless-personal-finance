import { jest } from '@jest/globals';
import React from 'react';
import { render, screen } from '@testing-library/react';
import { useRouteError } from "react-router-dom";
import mockConsole, { RestoreConsole } from "jest-mock-console";

import ErrorPage from "./ErrorPage";

jest.mock('react-router-dom');
const mockedUseRouteError = jest.mocked(useRouteError);
let restoreConsole: RestoreConsole;

beforeEach(() => {
    restoreConsole = mockConsole();
})

afterEach(() => {
    restoreConsole();
})

test('Render page without error', () => {
    render(<ErrorPage />);

    expect(screen.getByText(/an unexpected error has occurred/i)).toBeInTheDocument();
    expect(console.error).toHaveBeenCalled();
});

test('Render page with Error object', () => {
    mockedUseRouteError.mockReturnValue(new Error("Some other thing"))

    render(<ErrorPage />);
    const linkElement = screen.getByText(/Some other thing/i);
    expect(linkElement).toBeInTheDocument();
});

test('Render page with Error string', () => {
    mockedUseRouteError.mockReturnValue(("Some other thing"))

    render(<ErrorPage />);
    const linkElement = screen.getByText(/Some other thing/i);
    expect(linkElement).toBeInTheDocument();
});