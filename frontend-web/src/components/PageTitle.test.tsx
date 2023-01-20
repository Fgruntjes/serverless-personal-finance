import {render, screen} from "@testing-library/react";

import PageTitle from "./PageTitle";

describe(PageTitle.name, () => {
    test("Render without error", () => {
        render(<PageTitle>Some title</PageTitle>)
        const button = screen.getByText("Some title");
        expect(button).toBeInTheDocument();
    });
});