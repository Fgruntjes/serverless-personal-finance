import {jest} from "@jest/globals";
import {
    render,
    screen
} from "@testing-library/react";
import React from "react";
import {Navigate} from "react-router-dom";

import {mockIsLoading, mockLoggedIn, mockLoggedOut} from "../hooks/auth.mock";
import AuthGuard from "./AuthGuard";
import Loader from "./Loader";

jest.mock("react-router-dom")
jest.mock("./Loader")

describe(AuthGuard.name, () => {
    test("Redirect when not logged in", () => {
        mockLoggedOut();
    
        render(<AuthGuard><p data-testid="loggedin">logged in</p></AuthGuard>);
    
        expect(Navigate).toHaveBeenCalledTimes(1);
    });
    
    test("Render children when logged in", async () => {
        mockLoggedIn();
    
        render(<AuthGuard><p data-testid="loggedin">logged in</p></AuthGuard>);
        expect(await screen.findByTestId("loggedin")).toBeInTheDocument();
    });

    test("Render loader when loading", async () => {
        mockIsLoading();

        render(<AuthGuard><p data-testid="loggedin">logged in</p></AuthGuard>);
        
        expect(Loader).toHaveBeenCalled();
    });
});