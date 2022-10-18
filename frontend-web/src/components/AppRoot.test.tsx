import "./AuthGuard.mock"
import "../hooks/auth.mock"

import {render, screen} from "@testing-library/react";
import React from "react";
import {MemoryRouter} from "react-router-dom";
import {RecoilRoot} from "recoil";

import {mockLoggedIn} from "../hooks/auth.mock";
import AppRoot from "./AppRoot";

test("Render current page without error", () => {
    mockLoggedIn();
    
    render(
        <RecoilRoot>
            <MemoryRouter>
                <AppRoot />
            </MemoryRouter>
        </RecoilRoot>
    );
    
    expect(screen.getByText("appTitle")).toBeInTheDocument();
});
