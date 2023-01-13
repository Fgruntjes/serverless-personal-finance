import {jest} from "@jest/globals";
import {act, render, screen} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";
import click = Simulate.click;

import ConnectButton from "./ConnectButton";

test("Render children without error", async () => {
    const mockedOnClick = jest.fn();
    
    render(<ConnectButton onClick={mockedOnClick} />);

    const button = screen.getByText("button.connect");
    expect(button).toBeInTheDocument();
    act(() => {
        click(button);
    });

    expect(mockedOnClick.mock.calls.length).toEqual(1);
});

export {};