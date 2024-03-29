import {Auth0Provider} from "@auth0/auth0-react";
import {jest} from "@jest/globals";
import React from "react";

import ErrorPage from "../pages/ErrorPage";
import renderWithRouter from "../util/testRenderWithRouter";
import AuthProvider from "./AuthProvider";

jest.mock("@auth0/auth0-react");
jest.mock("../pages/ErrorPage");

describe(AuthProvider.name, () => {
    test("Render children without error", () => {
        process.env.REACT_APP_AUTH0_DOMAIN = "domain.com";
        process.env.REACT_APP_AUTH0_CLIENT_ID = "clientid";
        renderWithRouter(<AuthProvider><p>child element</p></AuthProvider>);

        expect(Auth0Provider).toHaveBeenCalledWith(
            expect.objectContaining({
                children: expect.anything(),
                clientId: "clientid",
            }),
            expect.anything(),
        );
    });

    test("Render error on missing auth0 domain", () => {
        process.env.REACT_APP_AUTH0_DOMAIN = "";
        process.env.REACT_APP_AUTH0_CLIENT_ID = "clientid";

        renderWithRouter(<AuthProvider><p>child element</p></AuthProvider>);

        expect(ErrorPage).toHaveBeenCalledWith(
            expect.objectContaining({error: expect.stringContaining("REACT_APP_AUTH0_DOMAIN")}),
            expect.anything(),
        );
    });

    test("Render error on missing auth0 client id", () => {
        process.env.REACT_APP_AUTH0_DOMAIN = "domain.com";
        process.env.REACT_APP_AUTH0_CLIENT_ID = "";

        renderWithRouter(<AuthProvider><p>child element</p></AuthProvider>);

        expect(ErrorPage).toHaveBeenCalledWith(
            expect.objectContaining({error: expect.stringContaining("REACT_APP_AUTH0_CLIENT_ID")}),
            expect.anything(),
        );
    });
});