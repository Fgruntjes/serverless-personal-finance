import {render} from "@testing-library/react";
import React from "react";

import Layout from "./Layout"

test("Render Root without errors", () => {
    render(<Layout.Root />);
});

test("Render SideNav without errors", () => {
    render(<Layout.SideNav />);
});

test("Render Main without errors", () => {
    render(<Layout.Main />);
});

test("Render Header without errors", () => {
    render(<Layout.Header />);
});