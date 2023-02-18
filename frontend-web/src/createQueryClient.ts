import {QueryClient, QueryClientConfig} from "@tanstack/react-query";

const createQueryClient = (options: Partial<QueryClientConfig> = {}) => 
    new QueryClient({
        defaultOptions: {
            queries: {useErrorBoundary: true},
            mutations: {useErrorBoundary: true}
        },
        ...options
    });

export default createQueryClient;