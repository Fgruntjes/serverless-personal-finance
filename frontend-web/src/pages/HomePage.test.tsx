import {render, screen} from "@testing-library/react";
import React from "react";

import HomePage from "./HomePage";

test('Render page without error', async () => {
    render(<HomePage />);
    expect(await screen.findByText('Learn React')).toBeInTheDocument();
});