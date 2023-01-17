import {useTranslation} from "react-i18next";
import {useQuery} from "react-query";
import {toast} from "react-toastify";

import {ReturnService} from "../generated/App.Function.Integration.Ynab";
import {TranslationNamespaces} from "../locales/namespaces";

export function useYnabReturnService(returnCode: string|null, returnUrl: string) {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);

    return useQuery(
        {
            retry: false,
            queryKey: ["YnabReturnService", {returnCode, returnUrl}],
            queryFn: () => returnCode ? ReturnService.return(returnCode, returnUrl) : null,
            onError: (error) => {
                toast.error(t("error.processReturnCodeFailed", {error}));
            },
            onSuccess: () => {
                toast.success(t("processReturnCodeSuccess"));
            }
        }
    );
}