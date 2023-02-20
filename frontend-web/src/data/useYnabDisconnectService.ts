import {useQuery, useQueryClient} from "@tanstack/react-query";

import {DisconnectService} from "../generated/App.Function.Integration.Ynab";
import {bindPromiseToSignal} from "../util/bindPromiseToSignal";
import {YnabStatusServiceQueryKey} from "./useYnabStatusService";

export const YnabDisconnectServiceQueryKey = "YnabDisconnectService";

export function useYnabDisconnectService() {
    const queryClient = useQueryClient();
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [YnabDisconnectServiceQueryKey],
            queryFn: ({signal}) => bindPromiseToSignal(DisconnectService.disconnect(), signal),
            onSettled: () => queryClient.invalidateQueries({queryKey: [YnabStatusServiceQueryKey]})
        }
    );
}