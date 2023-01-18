import {act, waitFor} from "@testing-library/react";

import {DisconnectService} from "../generated/App.Function.Integration.Ynab";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabDisconnectService} from "./useYnabDisconnectService";

jest.mock("../generated/App.Function.Integration.Ynab");

describe(useYnabDisconnectService.name, () => {
    const renderYnabHook = () => testRenderQueryHook(
        () => useYnabDisconnectService()
    );
    const mockedDisconnect = jest.fn(DisconnectService.disconnect);
    DisconnectService.disconnect = mockedDisconnect;
    
    test("Call disconnect", async () => {
        const {result} = renderYnabHook();
        
        act(() => {
            result.current.refetch();
        });
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(DisconnectService.disconnect).toHaveBeenCalledTimes(1)
    });
});
