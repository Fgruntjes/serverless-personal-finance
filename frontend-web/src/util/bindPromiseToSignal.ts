import {CancelablePromise as FunctionBanktransactionImportPromise} from "../generated/App.Function.Banktransaction.Import";
import {CancelablePromise as FunctionIntegrationYnabPromise} from "../generated/App.Function.Integration.Ynab";

type CancelablePromise<T> = FunctionIntegrationYnabPromise<T>|FunctionBanktransactionImportPromise<T>;

export function bindPromiseToSignal<T>(resultPromise: CancelablePromise<T>, signal?: AbortSignal): Promise<T> {
    signal?.addEventListener("abort", resultPromise.cancel.bind(resultPromise));
    return resultPromise;
}