import {waitFor} from "@testing-library/react";

import {StatusService} from "../generated/App.Function.Integration.Ynab";
import testRenderQueryHook from "../util/testRenderQueryHook";
import {useYnabStatusService} from "./useYnabStatusService";

jest.mock("../generated/App.Function.Integration.Ynab");

describe(useYnabStatusService.name, () => {
    const renderYnabHook = () => testRenderQueryHook(
        () => useYnabStatusService()
    );
    const mockedStatus = jest.fn(StatusService.status);
    StatusService.status = mockedStatus;
    
    test("Call status", async () => {
        const {result} = renderYnabHook();
        await waitFor(() => expect(result.current.isSuccess).toBe(true));
        
        expect(StatusService.status).toHaveBeenCalledTimes(1)
    });
});
