import React from 'react';
import { render, screen } from '@testing-library/react';
import mockConsole, { RestoreConsole } from "jest-mock-console";

import ErrorPage from "./ErrorPage";

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
    render(<ErrorPage error={new Error("Some other thing")} />);
    const linkElement = screen.getByText(/Some other thing/i);
    expect(linkElement).toBeInTheDocument();
});

test('Render page with Error string', () => {
    render(<ErrorPage error="Some other thing" />);
    const linkElement = screen.getByText(/Some other thing/i);
    expect(linkElement).toBeInTheDocument();
});

test('Render page with Error statusText', () => {
    render(<ErrorPage error={({statusText: "Some other thing"})} />);
    const linkElement = screen.getByText(/Some other thing/i);
    expect(linkElement).toBeInTheDocument();
});
