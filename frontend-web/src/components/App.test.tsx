import {render, screen} from "@testing-library/react";
import React from "react";

import App from "./App";

test('Render page without error', async () => {
    render(<App />);
    expect(await screen.findByText('Learn React')).toBeInTheDocument();
});