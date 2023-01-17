import {render} from "@testing-library/react";

import IndexPage from "./IndexPage";

describe(IndexPage.name, () => {
    test("Render without error", () => {
        render(<IndexPage />)
    });
});