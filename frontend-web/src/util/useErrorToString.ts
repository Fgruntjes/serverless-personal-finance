import {useTranslation} from "react-i18next";

import {TranslationNamespaces} from "../locales/namespaces";

const useErrorToString = () => {
    const {t} = useTranslation(TranslationNamespaces.Error);

    return (error: unknown): string => {
        if (!error) {
            return t("emptyError");
        }
        
        if (typeof (error) == "string") {
            return error;
        }

        if (typeof (error) == "object" && "message" in error) {
            return (error as { message: string }).message;
        }

        return `${error}`;
    }
};

export default useErrorToString;