import redirectTo from "./redirect";

describe(redirectTo.name, () => {
    const assignMock = jest.fn();
    beforeAll(() => {
        // @ts-ignore
        delete window.location
        // @ts-ignore
        window.location = {assign: assignMock,}
    })
    
    test("Redirect to url", () => {
        const url = "http://dummy.com";
        
        redirectTo(url);
        expect(assignMock).toBeCalledWith(url);
        expect(assignMock).toBeCalledTimes(1);
    });
});
