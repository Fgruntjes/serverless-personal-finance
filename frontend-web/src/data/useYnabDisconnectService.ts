import {useQuery} from "react-query";

import {DisconnectService} from "../generated/App.Function.Integration.Ynab";

export function useYnabDisconnectService() {
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [DisconnectService.name],
            queryFn: () => DisconnectService.disconnect(),
        }
    );
}