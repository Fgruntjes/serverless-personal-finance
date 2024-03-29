import {jest} from "@jest/globals";
import {
    render,
    screen
} from "@testing-library/react";
import React from "react";
import {useRouteError} from "react-router-dom";

import RouteErrorPage from "./RouteErrorPage";

jest.mock("react-router-dom");

describe(RouteErrorPage.name, () => {
    const mockedUseRouteError = jest.mocked(useRouteError);
    
    test("Render page with route error", () => {
        jest.spyOn(console, "error").mockImplementation(() => {});
        
        mockedUseRouteError.mockReturnValue(new Error("route error"))
        
        render(<RouteErrorPage />);
    
        expect(screen.getByText(/route error/i)).toBeInTheDocument();
        expect(console.error).toHaveBeenCalledWith(new Error("route error"))
    });
});