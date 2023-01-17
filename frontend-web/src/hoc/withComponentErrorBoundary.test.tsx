import {act, render, screen} from "@testing-library/react";
import {useEffect} from "react";
import {Simulate} from "react-dom/test-utils";
import click = Simulate.click;
import {useQueryErrorResetBoundary} from "react-query";

import withComponentErrorBoundary from "./withComponentErrorBoundary";

jest.mock("react-query");

describe(withComponentErrorBoundary.name, () => {
    const TestComponent = withComponentErrorBoundary(
        () => {
            useEffect(() => {
                throw new Error("SomeError");
            });
            return <div>test123</div>;
        });
    const mockedReset = jest.fn();

    beforeEach(() => {
        const mockedUseQueryErrorResetBoundary = jest.mocked(useQueryErrorResetBoundary);
        mockedUseQueryErrorResetBoundary.mockReturnValue({
            reset: mockedReset,
            isReset: () => false,
            clearReset: () => {
            },
        });

        jest.spyOn(console, "error").mockImplementation();
    });

    test("Render component without error", () => {
        const TestComponent = withComponentErrorBoundary(() => {
            return <div>test123</div>;
        });

        render(<TestComponent/>);

        expect(screen.getByText("test123")).toBeInTheDocument();
    });

    test("Render error and retry", () => {
        render(<TestComponent/>);

        expect(mockedReset.mock.calls.length).toBe(0);

        const retryButton = screen.getByText("button.retry");
        expect(retryButton).toBeInTheDocument();
        act(() => {
            click(retryButton);
        });

        expect(mockedReset.mock.calls.length).toBe(1);
    });

    test("Alternative title", () => {
        render(<TestComponent/>);

        expect(screen.queryByText("test123")).not.toBeInTheDocument();
        expect(screen.getByText("component.label")).toBeInTheDocument();
    });
});