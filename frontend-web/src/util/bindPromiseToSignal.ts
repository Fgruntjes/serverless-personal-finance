import {CancelablePromise} from "../generated/App.Function.Integration.Ynab";

export function bindPromiseToSignal<T>(resultPromise: CancelablePromise<T>, signal?: AbortSignal): Promise<T> {
    signal?.addEventListener("abort", resultPromise.cancel);
    return resultPromise;
}