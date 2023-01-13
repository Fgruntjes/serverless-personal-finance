import stringIsEmpty from "./stringIsEmpty";

test("Validate stringIsEmpty values", () => {
    expect(stringIsEmpty("")).toEqual(true)
    expect(stringIsEmpty("  ")).toEqual(true)
    expect(stringIsEmpty("    ")).toEqual(true)
    expect(stringIsEmpty("\t\r\n")).toEqual(true)
    expect(stringIsEmpty(null)).toEqual(true)
    expect(stringIsEmpty(undefined)).toEqual(true)

    expect(stringIsEmpty("a")).toEqual(false);
    expect(stringIsEmpty(" a")).toEqual(false);
    expect(stringIsEmpty("a ")).toEqual(false);
});

export {}