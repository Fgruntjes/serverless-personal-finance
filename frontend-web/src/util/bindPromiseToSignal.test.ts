import {CancelablePromise} from "../generated/App.Function.Integration.Ynab";
import {bindPromiseToSignal} from "./bindPromiseToSignal";

describe(bindPromiseToSignal.name, () => {
    test("CancelablePromise.cancel is called", () => {
        const cancelFn = jest.fn();
        
        const signalController = new AbortController();
        const cancelablePromise = new CancelablePromise((resolve, reject, onCancel) => {
            onCancel(cancelFn);
        });
        cancelablePromise.catch(() => {});

        bindPromiseToSignal(cancelablePromise, signalController.signal);
        signalController.abort();
        
        expect(cancelFn).toBeCalledTimes(1);
    });
});