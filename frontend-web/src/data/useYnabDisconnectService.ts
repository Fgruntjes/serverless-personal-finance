import {useQuery, useQueryClient} from "@tanstack/react-query";

import {DisconnectService, StatusService} from "../generated/App.Function.Integration.Ynab";
import {bindPromiseToSignal} from "../util/bindPromiseToSignal";

export function useYnabDisconnectService() {
    const queryClient = useQueryClient();
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [DisconnectService.name],
            queryFn: ({signal}) => bindPromiseToSignal(DisconnectService.disconnect(), signal),
            onSettled: () => queryClient.invalidateQueries({queryKey: [StatusService.name]})
        }
    );
}