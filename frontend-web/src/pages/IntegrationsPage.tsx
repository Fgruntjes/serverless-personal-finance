import React from "react";
import {useTranslation} from "react-i18next";

import PageTitle from "../components/PageTitle";
import IntegrationStatusList from "./integrationsPage/IntegrationStatusList";
import IntegrationStatusYnab from "./integrationsPage/IntegrationStatusYnab";

const IntegrationsPage = () => {
    const {t} = useTranslation("integrationsPage");
    return (
        <>
            <PageTitle>{t("title")}</PageTitle>
            <IntegrationStatusList>
                <IntegrationStatusYnab />
            </IntegrationStatusList>
        </>
    );
}

export default IntegrationsPage;