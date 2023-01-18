import {renderHook, waitFor} from "@testing-library/react";

import useErrorToString from "./useErrorToString";

type TestSet = { error: unknown, expectedResult: string };
const testErrors: TestSet[] = [
    {error: "string", expectedResult: "string"},
    {error: new Error("simple"), expectedResult: "simple"},
    {error: null, expectedResult: "emptyError"},
    {error: undefined, expectedResult: "emptyError"},
    {error: false, expectedResult: "emptyError"},
    {error: "", expectedResult: "emptyError"},
];

describe.each(testErrors)(useErrorToString.name, ({error, expectedResult}: TestSet) => {
    test(`Produces: ${expectedResult}`, async () => {
        const {result} = renderHook(() => {
            const errorToString = useErrorToString()
            return errorToString(error);
        });

        await waitFor(() => expect(result.current).toBe(expectedResult));
    });
});
