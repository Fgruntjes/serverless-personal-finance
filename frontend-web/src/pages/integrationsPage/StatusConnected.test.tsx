import {render, screen} from "@testing-library/react";
import React from "react";

import StatusConnected from "./StatusConnected";

describe(StatusConnected.name, () => {
    test("Render children without error", async () => {
        render(<StatusConnected><div>test123</div></StatusConnected>);
        expect(await screen.findByText("test123")).toBeInTheDocument();
    });
});
