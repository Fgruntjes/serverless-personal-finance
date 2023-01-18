import {Queries,queries} from "@testing-library/dom";
import {
    render, renderHook, RenderHookOptions, RenderHookResult, RenderResult
} from "@testing-library/react";
import React from "react";
import {QueryClientProvider} from "react-query";

import createQueryClient from "../createQueryClient";

export function testCreateQueryClient() {
    return createQueryClient({
        defaultOptions: {
            queries: {retry: false},
            mutations: {retry: false}
        },
    });
}

export function testRender(ui: React.ReactElement): RenderResult {
    const queryClient = testCreateQueryClient();
    const {rerender, ...result} = render(
        <QueryClientProvider client={queryClient}>{ui}</QueryClientProvider>
    )
    return {
        ...result,
        rerender: (rerenderUi: React.ReactElement) =>
            rerender(
                <QueryClientProvider client={queryClient}>{rerenderUi}</QueryClientProvider>
            ),
    }
}

export default function testRenderHook<
    Result,
    Props,
    Q extends Queries = typeof queries,
    Container extends Element | DocumentFragment = HTMLElement,
    BaseElement extends Element | DocumentFragment = Container,
>(
    renderer: (initialProps: Props) => Result,
    options?: RenderHookOptions<Props, Q, Container, BaseElement>
): RenderHookResult<Result, Props> {
    const queryClient = testCreateQueryClient();
    return renderHook(
        renderer,
        {
            ...options,
            wrapper: ({children}) => {
                return (
                    <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
                );
            }
        }
    );
}