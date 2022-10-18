jest.mock("react-i18next", () => ({
    ...jest.requireActual("react-i18next"),
    useTranslation: () => {
        return {
            t: (str: string) => str,
            i18n: {changeLanguage: () => new Promise(() => {}),},
        };
    },
}));

export {}