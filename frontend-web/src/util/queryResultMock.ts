import {CancelablePromise} from "../generated/App.Function.Integration.Ynab";
import Mock = jest.Mock;

export default function queryResultMock<T, Y extends any[]>(
    implementation: (...args: Y) => CancelablePromise<T>,
    value: T
): Mock<CancelablePromise<T>, Y> {
    return jest.fn(implementation).mockReturnValue(new CancelablePromise<T>(resolve => resolve(value)));
}