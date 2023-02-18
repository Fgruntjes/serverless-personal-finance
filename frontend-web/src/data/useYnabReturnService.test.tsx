import {waitFor} from "@testing-library/react";
import {toast} from "react-toastify";

import {ReturnService} from "../generated/App.Function.Integration.Ynab";
import queryResultMock from "../util/queryResultMock";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabReturnService} from "./useYnabReturnService";

jest.mock("react-toastify");

describe(useYnabReturnService.name, () => {
    const renderYnabHook = (returnCode: string|null, returnUrl: string) => testRenderQueryHook(
        () => useYnabReturnService(returnCode, returnUrl)
    );

    beforeEach(() => {
        ReturnService.return = queryResultMock(
            ReturnService.return,
            {data: {accountName: "", connected: false}}
        );
    });
    
    test("Call connect and show success", async () => {
        const {result} = renderYnabHook("123", "https://www.example.com/returnsomething");
        await waitFor(() => expect(result.current.isSuccess).toBe(true));

        expect(ReturnService.return).toHaveBeenCalledWith("123", "https://www.example.com/returnsomething");
        expect(ReturnService.return).toHaveBeenCalledTimes(1)
        expect(toast.success).toHaveBeenCalledWith("processReturnCodeSuccess");
    });

    test("Show error on failure", async () => {
        jest.spyOn(console, "error").mockImplementation()
        jest.mocked(ReturnService.return).mockImplementation(() => {
            throw new Error("Some Error");
        });

        const {result} = renderYnabHook("123", "https://www.example.com/returnsomething");
        await waitFor(() => expect(result.current.isLoadingError).toBe(true));
        
        expect(toast.error).toHaveBeenCalledWith("error.processReturnCodeFailed");
    });

    const emptyCodeValues = [null, "", " ", "\t", "\n"];
    test.each(emptyCodeValues)("Show missing code", async (code) => {
        jest.spyOn(console, "error").mockImplementation()
        const {result} = renderYnabHook(code, "https://www.example.com/returnsomething");

        await waitFor(() => expect(result.current.isLoadingError).toBe(true));

        expect(toast.error).toHaveBeenCalledWith("error.processReturnCodeFailed");
    });
});
