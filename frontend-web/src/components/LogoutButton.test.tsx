import "./AuthGuard.mock"
import "../hooks/auth.mock"

import {act, render, screen} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";

import {mockedSignOut, mockLoggedIn} from "../hooks/auth.mock";
import click = Simulate.click;
import {LogoutButton} from "./LogoutButton";

test("Render without error", () => {
    mockLoggedIn();
    
    render(<LogoutButton />);
});

test("Press button", () => {
    mockLoggedIn();

    render(<LogoutButton />);

    const logoutButton = screen.getByText("button.logout");
    expect(logoutButton).toBeInTheDocument();
    act(() => {
        click(logoutButton);
    });
    
    expect(mockedSignOut.mock.calls.length).toEqual(1);
});