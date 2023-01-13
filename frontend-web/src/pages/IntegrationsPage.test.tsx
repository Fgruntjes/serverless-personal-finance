import {render, screen} from "@testing-library/react";
import React from "react";

import IntegrationsPage from "./IntegrationsPage";

jest.mock("./integrationsPage/IntegrationStatusYnab");

test("Render without error", async () => {
    render(<IntegrationsPage />);
    expect(await screen.findByText("title")).toBeInTheDocument();
});

export {};