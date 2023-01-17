import {jest} from "@jest/globals";
import {
    render,
    screen
} from "@testing-library/react";
import React from "react";

import ErrorPage from "./ErrorPage";

describe(ErrorPage.name, () => {
    beforeEach(() => {
        jest.spyOn(console, "error").mockImplementation(() => {});
    });

    test("Render page without error", () => {
        render(<ErrorPage />);
        expect(screen.getByText(/Oops!/i)).toBeInTheDocument();
        expect(console.error).toHaveBeenCalled();
    });

    test("Render page with Error object", () => {
        render(<ErrorPage error={new Error("Some other thing")} />);
        const linkElement = screen.getByText(/Some other thing/i);
        expect(linkElement).toBeInTheDocument();
    });

    test("Render page with Error string", () => {
        render(<ErrorPage error="Some other thing" />);
        const linkElement = screen.getByText(/Some other thing/i);
        expect(linkElement).toBeInTheDocument();
    });

    test("Render page with Error statusText and statusCode", () => {
        render(<ErrorPage error={({
            statusText: "Some other thing",
            statusCode: 500,
        })} />);
        expect(screen.getByText(/Some other thing/i)).toBeInTheDocument();
        expect(screen.getByText(/500/i)).toBeInTheDocument();
    });

    test("Render page with 404", () => {
        render(<ErrorPage error={({
            statusText: "Page not found",
            status: 404,
        })} />);
        expect(screen.getByText(/Page not found/i)).toBeInTheDocument();
        expect(screen.getByText(/404/i)).toBeInTheDocument();
    });

    test("Render page with unknown error", () => {
        render(<ErrorPage error={({other: "things"})} />);
        expect(screen.getByText(/things/i)).toBeInTheDocument();
        expect(screen.getByText(/Unknown error/i)).toBeInTheDocument();
    });    
});
