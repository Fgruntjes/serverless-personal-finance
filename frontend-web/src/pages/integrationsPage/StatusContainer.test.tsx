import {render, screen} from "@testing-library/react";
import React from "react";

import iconLogo from "./integrationStatusYnab/logo.svg"
import StatusContainer from "./StatusContainer";

test("Render without error", async () => {
    render(<StatusContainer icon={iconLogo} name={"somename"}><div>test123</div></StatusContainer>);
    expect(await screen.findByText("test123")).toBeInTheDocument();
    expect(await screen.findByAltText("name.somename")).toBeInTheDocument();
    
    const image = screen.getByAltText<HTMLImageElement>("name.somename");
    expect(image).toBeInTheDocument();
    expect(image.src).toContain(iconLogo);
});

export {};