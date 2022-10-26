import {useTranslation} from "react-i18next";

import PageTitle from "../components/PageTitle";

const IntegrationsPage = () => {
    const {t} = useTranslation("integrationsPage");
    return (
        <>
            <PageTitle>{t("title")}</PageTitle>
            
        </>
    );
}

export default IntegrationsPage;