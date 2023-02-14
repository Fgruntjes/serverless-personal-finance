import {useQuery} from "react-query";

import {ConnectService} from "../generated/App.Function.Integration.Ynab";

export function useYnabConnectService(returnUrl: string) {
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [ConnectService.name],
            queryFn: () => ConnectService.connect(returnUrl),
        }
    );
}