import {render} from "@testing-library/react";
import {createMemoryRouter, RouterProvider} from "react-router-dom";

const renderWithRouter = (element: React.ReactNode|null) => {
    const router = createMemoryRouter([{
        path: "/",
        element
    }]);

    render(<RouterProvider router={router} />);
};

export default renderWithRouter;