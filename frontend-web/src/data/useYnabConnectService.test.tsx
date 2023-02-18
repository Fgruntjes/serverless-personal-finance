import {act, waitFor} from "@testing-library/react";

import {ConnectService} from "../generated/App.Function.Integration.Ynab";
import queryResultMock from "../util/queryResultMock";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabConnectService} from "./useYnabConnectService";

describe(useYnabConnectService.name, () => {
    const renderYnabHook = (returnUrl: string) => testRenderQueryHook(
        () => useYnabConnectService(returnUrl)
    );

    beforeEach(() => {
        ConnectService.connect = queryResultMock(ConnectService.connect, {data: "done"});
    });
    
    test("Call connect", async () => {
        const {result} = renderYnabHook("https://example.com/return");
        act(() => {
            result.current.refetch();
        });
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(ConnectService.connect).toHaveBeenCalledTimes(1);
        expect(ConnectService.connect).toHaveBeenCalledWith("https://example.com/return");
    });
});
