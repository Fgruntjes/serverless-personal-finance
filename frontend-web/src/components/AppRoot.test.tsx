import "./AuthGuard.mock"
import "../hooks/auth.mock"

import {act, render, screen} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";
import {RecoilRoot} from "recoil";

import {mockedSignOut, mockLoggedIn} from "../hooks/auth.mock";
import AppRoot from "./AppRoot";
import click = Simulate.click;

test("Render current page without error", () => {
    mockLoggedIn();
    
    render(
        <RecoilRoot>
            <AppRoot />
        </RecoilRoot>
    );
    
    expect(screen.getByText("appTitle")).toBeInTheDocument();
});

test("Press logout button", () => {
    mockLoggedIn();

    render(
        <RecoilRoot>
            <AppRoot />
        </RecoilRoot>
    );

    const logoutButton = screen.getByText("button.logout");
    expect(logoutButton).toBeInTheDocument();
    act(() => {
        click(logoutButton);
    });
    
    expect(mockedSignOut.mock.calls.length).toEqual(1);
});