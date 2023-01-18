import {waitFor} from "@testing-library/react";

import {ConnectService} from "../generated/App.Function.Integration.Ynab";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabConnectService} from "./useYnabConnectService";

jest.mock("../generated/App.Function.Integration.Ynab");

describe(useYnabConnectService.name, () => {
    const renderYnabHook = (returnUrl: string) => testRenderQueryHook(
        () => useYnabConnectService(returnUrl)
    );
    const mockedDisconnect = jest.fn(ConnectService.connect);
    ConnectService.connect = mockedDisconnect;
    
    test("Call connect", async () => {
        const {result} = renderYnabHook("https://example.com/return");
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(ConnectService.connect).toHaveBeenCalledTimes(1);
        expect(ConnectService.connect).toHaveBeenCalledWith("https://example.com/return");
    });
});
