import {jest} from "@jest/globals";
import {act, render, screen} from "@testing-library/react";
import React from "react";
import {Simulate} from "react-dom/test-utils";
import click = Simulate.click;

import DisconnectButton from "./DisconnectButton";

describe(DisconnectButton.name, () => {
    test("Render children without error", async () => {
        const mockedOnClick = jest.fn();
        
        render(<DisconnectButton onClick={mockedOnClick} />);
    
        const button = screen.getByText("button.disconnect");
        expect(button).toBeInTheDocument();
        act(() => {
            click(button);
        });
    
        expect(mockedOnClick.mock.calls.length).toEqual(1);
    });
});