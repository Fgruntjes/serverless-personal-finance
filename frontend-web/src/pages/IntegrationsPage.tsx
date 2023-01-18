import React from "react";
import {useTranslation} from "react-i18next";
import {Outlet} from "react-router-dom";

import PageTitle from "../components/PageTitle";
import {TranslationNamespaces} from "../locales/namespaces";
import IntegrationStatusList from "./integrationsPage/IntegrationStatusList";
import IntegrationStatusYnab from "./integrationsPage/IntegrationStatusYnab";

const IntegrationsPage = () => {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);
    
    return (
        <>
            <PageTitle>{t("title")}</PageTitle>
            <Outlet />
            <IntegrationStatusList>
                <IntegrationStatusYnab />
            </IntegrationStatusList>
        </>
    );
}

export default IntegrationsPage;