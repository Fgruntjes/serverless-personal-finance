import {useTranslation} from "react-i18next";
import {useQuery, useQueryClient} from "react-query";
import {toast} from "react-toastify";

import {ReturnService, StatusService} from "../generated/App.Function.Integration.Ynab";
import {TranslationNamespaces} from "../locales/namespaces";
import stringIsEmpty from "../util/stringIsEmpty";
import useErrorToString from "../util/useErrorToString";

export function useYnabReturnService(returnCode: string|null, returnUrl: string) {
    const {t} = useTranslation(TranslationNamespaces.IntegrationsPage);
    const errorToString = useErrorToString();
    const queryClient = useQueryClient();

    return useQuery(
        {
            retry: false,
            queryKey: [ReturnService.name, {returnCode, returnUrl}],
            queryFn: () => {
                if (!stringIsEmpty(returnCode)) {
                    return ReturnService.return(returnCode as string, returnUrl);
                }
                
                throw new Error(t("error.missingReturnCode"));
            },
            onError: (error) => {
                toast.error(t("error.processReturnCodeFailed", {error: errorToString(error)}));
            },
            onSuccess: () => {
                toast.success(t("processReturnCodeSuccess"));
            },
            onSettled: () => {
                queryClient.invalidateQueries(StatusService.name);
            }
        }
    );
}