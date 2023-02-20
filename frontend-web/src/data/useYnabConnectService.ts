import {useQuery} from "@tanstack/react-query";

import {ConnectService} from "../generated/App.Function.Integration.Ynab";
import {bindPromiseToSignal} from "../util/bindPromiseToSignal";

export const YnabConnectServiceQueryKey = "YnabConnectService";

export function useYnabConnectService(returnUrl: string) {
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [YnabConnectServiceQueryKey],
            queryFn: ({signal}) => bindPromiseToSignal(ConnectService.connect(returnUrl), signal),
        }
    );
}

