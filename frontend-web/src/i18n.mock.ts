import {DefaultNamespace, TransProps} from "react-i18next";

jest.mock("react-i18next", () => ({
    ...jest.requireActual("react-i18next"),
    useTranslation: () => {
        return {
            t: (str: string) => str,
            i18n: {changeLanguage: () => new Promise(() => {}),},
        };
    },
    Trans: (props: TransProps<any,DefaultNamespace,any,any>) => props.i18nKey
}));


export {}