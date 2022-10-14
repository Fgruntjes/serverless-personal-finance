import {render} from "@testing-library/react";
import React from "react";
import {MemoryRouter, Route, Routes} from "react-router-dom";

import AppRoot from "./AppRoot";

test('Render current page without error', () => {
    render(
        <MemoryRouter>
            <Routes>
                <Route path="/" element={<AppRoot />} />
            </Routes>
        </MemoryRouter>
    );
});
