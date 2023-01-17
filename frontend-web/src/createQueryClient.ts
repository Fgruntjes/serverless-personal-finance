import {QueryClient} from "react-query";
import {QueryClientConfig} from "react-query/types/core/types";

const createQueryClient = (options: Partial<QueryClientConfig> = {}) => 
    new QueryClient({
        defaultOptions: {
            queries: {useErrorBoundary: true},
            mutations: {useErrorBoundary: true}
        },
        ...options
    });

export default createQueryClient;