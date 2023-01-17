import {render} from "@testing-library/react";
import React from "react";
import {MemoryRouter} from "react-router-dom";

import {mockLoggedIn} from "../hooks/auth.mock";
import Layout from "./Layout"

describe(Layout.RootDefault.name, () => {
    test("Render RootDefault without errors", () => {
        render(<Layout.RootDefault/>);
    });
});

describe(Layout.RootCenter.name, () => {
    test("Render RootCenter without errors", () => {
        render(<Layout.RootCenter/>);
    });
});

describe(Layout.SideNav.name, () => {
    test("Render SideNav without errors", () => {
        render(<MemoryRouter><Layout.SideNav/></MemoryRouter>);
    });
});

describe(Layout.Main.name, () => {
    test("Render Main without errors", () => {
        render(<Layout.Main/>);
    });
});

describe(Layout.MainWrapper.name, () => {
    test("Render MainWrapper without errors", () => {
        render(<Layout.MainWrapper/>);
    });
});

describe(Layout.Header.name, () => {
    test("Render Header without errors", () => {
        mockLoggedIn();

        render(<Layout.Header/>);
    });
});

describe(Layout.Footer.name, () => {
    test("Render Footer without errors", () => {
        render(<Layout.Footer/>);
    });
});
