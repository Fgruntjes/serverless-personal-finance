import {Queries,queries} from "@testing-library/dom";
import {renderHook, RenderHookOptions} from "@testing-library/react";
import React from "react";
import {QueryClientProvider} from "react-query";

import createQueryClient from "../setup/createQueryClient";

export default function testRenderQueryHook<
    Result,
    Props,
    Q extends Queries = typeof queries,
    Container extends Element | DocumentFragment = HTMLElement,
    BaseElement extends Element | DocumentFragment = Container,
>(
    renderer: (initialProps: Props) => Result,
    options?: RenderHookOptions<Props, Q, Container, BaseElement>
) {
    return renderHook(
        renderer,
        {
            ...options,
            wrapper: ({children}) => {
                const queryClient = createQueryClient({
                    defaultOptions: {
                        queries: {retry: false},
                        mutations: {retry: false}
                    },
                });

                return (
                    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
                );
            }
        }
    );
}