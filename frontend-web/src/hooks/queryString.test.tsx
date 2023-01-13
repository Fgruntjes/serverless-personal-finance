import {jest} from "@jest/globals";
import {renderHook} from "@testing-library/react";
import {useLocation} from "react-router-dom";

import useQueryString from "./queryString";

jest.mock("react-router-dom");
const mockedUseLocation = jest.mocked(useLocation);

test("Call useQueryString", () => {
    mockedUseLocation.mockReturnValue({
        key: "asdf",
        pathname: "/tmp",
        search: "?foo=bar",
        hash: "",
        state: {}
    })

    const result = renderHook(() => useQueryString()).result.current;
    expect(result.toString()).toBe("foo=bar");
});
