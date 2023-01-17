import {render, screen} from "@testing-library/react";
import React from "react";

import IntegrationStatusList from "./IntegrationStatusList";

describe(IntegrationStatusList.name, () => {
    test("Render children without error", async () => {
        render(<IntegrationStatusList><div>test123</div></IntegrationStatusList>);
        expect(await screen.findByText("test123")).toBeInTheDocument();
    });
});