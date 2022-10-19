import {render, screen} from "@testing-library/react";

import PageTitle from "./PageTitle";

test("Render without error", () => {
    render(<PageTitle>Some title</PageTitle>)
    const button = screen.getByText("Some title");
    expect(button).toBeInTheDocument();
});
