import "../hooks/auth.mock"

import {render} from "@testing-library/react";
import React from "react";
import {MemoryRouter} from "react-router-dom";

import {mockLoggedIn} from "../hooks/auth.mock";
import Layout from "./Layout"

test("Render RootDefault without errors", () => {
    render(<Layout.RootDefault />);
});

test("Render RootCenter without errors", () => {
    render(<Layout.RootCenter />);
});

test("Render SideNav without errors", () => {
    render(<MemoryRouter><Layout.SideNav /></MemoryRouter>);
});

test("Render Main without errors", () => {
    render(<Layout.Main />);
});

test("Render MainWrapper without errors", () => {
    render(<Layout.MainWrapper />);
});

test("Render Header without errors", () => {
    mockLoggedIn();
    
    render(<Layout.Header />);
});

test("Render Footer without errors", () => {
    render(<Layout.Footer />);
});
