import i18n from "i18next";
import resourcesToBackend from "i18next-resources-to-backend";
import {initReactI18next} from "react-i18next";

i18n
    .use(resourcesToBackend((language, namespace, callback) => {
        import(`./locales/${language}/${namespace}.json`)
            .then((resources) => {
                callback(null, resources)
            })
            .catch((error) => {
                callback(error, null)
            })
    }))
    .use(initReactI18next)
    .init({
        debug: process.env.NODE_ENV === "development",
        lng: "en",
        fallbackLng: "en",
        interpolation: {escapeValue: false}
    });

export default i18n;