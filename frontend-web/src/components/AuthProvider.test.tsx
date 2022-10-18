import {jest} from "@jest/globals";
import {GoogleOAuthProvider} from "@react-oauth/google";
import {render} from "@testing-library/react";
import React from "react";

import ErrorPage from "../pages/ErrorPage";
import AuthProvider from "./AuthProvider";

jest.mock("@react-oauth/google");
jest.mock("../pages/ErrorPage");

test("Render children without error", () => {
    render(<AuthProvider clientId="clientid"><p>child element</p></AuthProvider>);

    expect(GoogleOAuthProvider).toHaveBeenCalledWith(
        expect.objectContaining({
            children: expect.anything(),
            clientId: "clientid",
        }),
        expect.anything(),
    );
});

test("Render error on missing setting", () => {
    render(<AuthProvider><p>child element</p></AuthProvider>);

    expect(ErrorPage).toHaveBeenCalledWith(
        expect.objectContaining({error: expect.stringContaining("REACT_APP_GOOGLE_AUTH_CLIENT_ID")}),
        expect.anything(),
    );
});