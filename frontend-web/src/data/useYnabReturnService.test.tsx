
import {ReturnService} from "../generated/App.Function.Integration.Ynab";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabReturnService} from "./useYnabReturnService";

jest.mock("../generated/App.Function.Integration.Ynab");

describe(useYnabReturnService.name, () => {
    const renderYnabHook = (returnCode: string|null, returnUrl: string) => testRenderQueryHook(
        () => useYnabReturnService(returnCode, returnUrl)
    );
    
    test("Call connect and show success", async () => {
        ReturnService.return = jest.fn(ReturnService.return);
        
        renderYnabHook("123", "https://www.example.com/returnsomething");

        expect(jest.mocked(ReturnService.return)).toHaveBeenCalledWith("123", "https://www.example.com/returnsomething");
        expect(jest.mocked(ReturnService.return)).toHaveBeenCalledTimes(1);
    });
});

export {};