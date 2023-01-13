import {render} from "@testing-library/react";
import React from "react";

import Loader from "./Loader"

test("Render Loader without errors", () => {
    render(<Loader />);
});
