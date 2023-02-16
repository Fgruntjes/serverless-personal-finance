import {useQuery, useQueryClient} from "react-query";

import {DisconnectService, StatusService} from "../generated/App.Function.Integration.Ynab";

export function useYnabDisconnectService() {
    const queryClient = useQueryClient();
    return useQuery(
        {
            retry: false,
            enabled: false,
            queryKey: [DisconnectService.name],
            queryFn: () => DisconnectService.disconnect(),
            onSettled: () => {
                queryClient.invalidateQueries(StatusService.name);
            }
        }
    );
}