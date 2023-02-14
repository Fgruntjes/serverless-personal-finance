import "./AuthGuard.mock"

import {screen} from "@testing-library/react";
import React from "react";
import {RecoilRoot} from "recoil";

import {mockLoggedIn} from "../hooks/auth.mock";
import renderWithRouter from "../util/testRenderWithRouter";
import AppRoot from "./AppRoot";

jest.mock("./AuthProvider", () => (props: {children: JSX.Element}) => (<>{props.children}</>));

describe(AppRoot.name, () => {
    test("Render current page without error", () => {
        mockLoggedIn();

        renderWithRouter(
            <RecoilRoot>
                <AppRoot/>
            </RecoilRoot>
        );

        expect(screen.getByText("appTitle")).toBeInTheDocument();
    });
});
