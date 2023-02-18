import {act, waitFor} from "@testing-library/react";

import {DisconnectService} from "../generated/App.Function.Integration.Ynab";
import queryResultMock from "../util/queryResultMock";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabDisconnectService} from "./useYnabDisconnectService";

describe(useYnabDisconnectService.name, () => {
    const renderYnabHook = () => testRenderQueryHook(
        () => useYnabDisconnectService()
    );

    beforeEach(() => {
        DisconnectService.disconnect = queryResultMock(DisconnectService.disconnect, {data: "done"});
    });
    
    test("Call disconnect", async () => {
        const {result} = renderYnabHook();
        
        act(() => {
            result.current.refetch();
        });
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(DisconnectService.disconnect).toHaveBeenCalledTimes(1)
    });
});
