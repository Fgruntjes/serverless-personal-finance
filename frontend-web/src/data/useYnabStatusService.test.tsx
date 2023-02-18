import {waitFor} from "@testing-library/react";

import {StatusService} from "../generated/App.Function.Integration.Ynab";
import queryResultMock from "../util/queryResultMock";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabStatusService} from "./useYnabStatusService";

describe(useYnabStatusService.name, () => {
    const renderYnabHook = () => testRenderQueryHook(
        () => useYnabStatusService()
    );
    
    beforeEach(() => {
        StatusService.status = queryResultMock(
            StatusService.status,
            {data: {accountName: "", connected: false}}
        );
    });
    
    test("Call status", async () => {
        const {result} = renderYnabHook();
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(StatusService.status).toHaveBeenCalledTimes(1)
    });
});
