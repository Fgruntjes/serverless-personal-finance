import {render, screen} from "@testing-library/react";
import React from "react";

import StatusDisconnected from "./StatusDisconnected";

describe(StatusDisconnected.name, () => {
    test("Render children without error", async () => {
        render(<StatusDisconnected><div>test123</div></StatusDisconnected>);
        expect(await screen.findByText("test123")).toBeInTheDocument();
    });
});
