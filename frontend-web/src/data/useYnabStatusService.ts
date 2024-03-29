import {useQuery} from "@tanstack/react-query";

import {StatusService} from "../generated/App.Function.Integration.Ynab";
import {bindPromiseToSignal} from "../util/bindPromiseToSignal";

export const YnabStatusServiceQueryKey = "YnabStatusService";

export function useYnabStatusService() {
    return useQuery(
        {
            retry: false,
            queryKey: [YnabStatusServiceQueryKey],
            queryFn: async ({signal}) => bindPromiseToSignal(StatusService.status(), signal),
        }
    );
}