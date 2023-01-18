import {useQuery} from "react-query";

import {StatusService} from "../generated/App.Function.Integration.Ynab";

export function useYnabStatusService() {
    return useQuery(
        {
            retry: false,
            queryKey: [StatusService.name],
            queryFn: () => StatusService.status(),
        }
    );
}